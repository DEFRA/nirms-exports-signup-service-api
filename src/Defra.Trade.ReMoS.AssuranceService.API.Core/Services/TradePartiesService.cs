﻿using AutoMapper;
using Azure.Core;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Defra.Trade.Common.Security.Authentication;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Models;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.FeatureManagement;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Constants;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Services
{
    public class TradePartiesService : ITradePartiesService
    {
        private readonly ITradePartyRepository _tradePartyRepository;
        private readonly IMapper _mapper;
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly IOptions<TradePlatform> _tradePlatformIntegrationSettings;
        private readonly IFeatureManager _featureManager;
        private readonly ServiceBusClient _serviceBusClient;

        public TradePartiesService(
            ITradePartyRepository tradePartyRepository,
            IMapper mapper,
            IEstablishmentRepository establishmentRepository,
            IOptions<TradePlatform> tradePlatformIntegrationSettings,
            IFeatureManager featureManager,
            ServiceBusClient serviceBusClient)
        {
            _tradePartyRepository = tradePartyRepository ?? throw new ArgumentNullException(nameof(tradePartyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _establishmentRepository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
            _tradePlatformIntegrationSettings = tradePlatformIntegrationSettings;
            _featureManager = featureManager;
            _serviceBusClient = serviceBusClient;
        }

        public async Task<TradePartyDto?> AddTradePartyAsync(TradePartyDto tradePartyRequest)
        {
            var tradeParty = _mapper.Map<TradeParty>(tradePartyRequest);
            tradeParty.CreatedDate = DateTime.UtcNow;
            tradeParty.LastUpdateDate = DateTime.UtcNow;

            await _tradePartyRepository.AddTradePartyAsync(tradeParty);
            await _tradePartyRepository.SaveChangesAsync();
            return _mapper.Map<TradePartyDto>(tradeParty);

        }

        public async Task<IEnumerable<TradePartyDto>> GetTradePartiesAsync()
        {
            var tradeParties = await _tradePartyRepository.GetAllTradeParties();
            var tradePartyDtos = _mapper.Map<IEnumerable<TradePartyDto>>(tradeParties);
            return tradePartyDtos;
        }

        public async Task<TradePartyDto?> GetTradePartyAsync(Guid tradePartyId)
        {
            var tradeParty = await _tradePartyRepository
                .GetTradePartyAsync(tradePartyId);

            return _mapper.Map<TradePartyDto>(tradeParty);
        }

        public async Task<TradePartyDto?> GetTradePartyByDefraOrgIdAsync(Guid orgId)
        {
            var tradeParty = await _tradePartyRepository
                .GetTradePartyByDefraOrgIdAsync(orgId);

            return _mapper.Map<TradePartyDto>(tradeParty);
        }

        public async Task<TradePartyDto?> UpdateTradePartyAsync(Guid tradePartyId, TradePartyDto tradePartyRequest)
        {
            var submittedApplication = tradePartyRequest.SignUpRequestSubmittedBy != Guid.Empty;

            TradeParty? tradeParty = await FindTradeParty(tradePartyId);
            if (tradeParty == null)
                return null;

            tradeParty.LastUpdateDate = DateTime.UtcNow;
            _mapper.Map(tradePartyRequest, tradeParty);

            if (tradePartyRequest.FboPhrOption == "none")
            {
                tradeParty.FboNumber = null;
                tradeParty.PhrNumber = null;
            }

            var updatedTradeParty = _tradePartyRepository.UpdateTradeParty(tradeParty);
            await _tradePartyRepository.SaveChangesAsync();

            if (await _featureManager.IsEnabledAsync(FeatureFlags.SignUpApplication) && submittedApplication)
            {
                await SendSignUpApplication(updatedTradeParty!.Id);
            }
            return _mapper.Map<TradePartyDto>(tradeParty);
        }

        public async Task<TradePartyDto?> UpdateTradePartyAddressAsync(Guid tradePartyId, TradePartyDto tradePartyRequest)
        {
            TradeParty? tradeParty = await FindTradeParty(tradePartyId);
            if (tradeParty == null)
                return null;

            _mapper.Map(tradePartyRequest, tradeParty);
            _tradePartyRepository.UpdateTradePartyAddress(tradeParty);
            await _tradePartyRepository.SaveChangesAsync();

            return _mapper.Map<TradePartyDto>(tradeParty);
        }

        public async Task<TradePartyDto?> AddTradePartyAddressAsync(Guid tradePartyId, TradeAddressDto tradeAddressRequest)
        {
            TradeParty? tradeParty = await FindTradeParty(tradePartyId);
            if (tradeParty == null)
                return null;

            _tradePartyRepository.AddTradePartyAddress(tradeParty, _mapper.Map<TradeAddress>(tradeAddressRequest));
            tradeParty.RemosBusinessSchemeNumber ??= AssignRemosBusinessSchemeNumber(tradeParty);
            await _tradePartyRepository.SaveChangesAsync();
            return _mapper.Map<TradePartyDto>(tradeParty);
        }

        public async Task<TradePartyDto?> UpdateTradePartyContactAsync(Guid tradePartyId, TradePartyDto tradePartyRequest)
        {
            TradeParty? tradeParty = await FindTradeParty(tradePartyId);
            if (tradeParty == null)
            {
                return null;
            }

            _mapper.Map(tradePartyRequest, tradeParty);
            _tradePartyRepository.UpsertTradePartyContact(tradeParty!);

            return _mapper.Map<TradePartyDto>(tradeParty);
        }

        public async Task<TradePartyDto?> UpdateAuthorisedSignatoryAsync(Guid tradePartyId, TradePartyDto tradePartyRequest)
        {
            var tradeParty = await FindTradeParty(tradePartyId);

            if (tradeParty == null)
            {
                return null;
            }

            bool authSignatoryRequestContactDetailsAreEmpty = tradePartyRequest.AuthorisedSignatory?.Name == null
                && tradePartyRequest.AuthorisedSignatory?.Position == null
                && tradePartyRequest.AuthorisedSignatory?.EmailAddress == null;

            bool authSignatoryContactDetailsSaved = tradeParty.AuthorisedSignatory?.Name != null
                || tradeParty.AuthorisedSignatory?.Position != null
                || tradeParty.AuthorisedSignatory?.EmailAddress != null;


            if (tradePartyRequest.Contact?.IsAuthorisedSignatory == false && authSignatoryRequestContactDetailsAreEmpty && authSignatoryContactDetailsSaved)
            {
                return _mapper.Map<TradePartyDto>(tradeParty);
            }

            _mapper.Map(tradePartyRequest, tradeParty);

            if (tradeParty!.TradeContact?.IsAuthorisedSignatory != null)
            {
                _tradePartyRepository.UpsertTradePartyContact(tradeParty);
            }

            _tradePartyRepository.UpsertAuthorisedSignatory(tradeParty!);

            return _mapper.Map<TradePartyDto>(tradeParty);
        }

        [FeatureGate(FeatureFlags.SelfServe)]
        public async Task<TradePartyDto?> UpdateContactSelfServeAsync(Guid tradePartyId, TradePartyDto tradePartyRequest)
        {
            var tradeParty = await FindTradeParty(tradePartyId);

            if (tradeParty == null)
            {
                return null;
            }

            _mapper.Map(tradePartyRequest, tradeParty);
            _tradePartyRepository.UpsertTradePartyContact(tradeParty!);

            await SendSelfServeApplicationAsync(tradePartyId, true, false);

            return _mapper.Map<TradePartyDto>(tradeParty);
        }

        [FeatureGate(FeatureFlags.SelfServe)]
        public async Task<TradePartyDto?> UpdateAuthRepSelfServeAsync(Guid tradePartyId, TradePartyDto tradePartyRequest)
        {
            var tradeParty = await FindTradeParty(tradePartyId);

            if (tradeParty == null)
            {
                return null;
            }

            _mapper.Map(tradePartyRequest, tradeParty);
            _tradePartyRepository.UpsertAuthorisedSignatory(tradeParty!);

            await SendSelfServeApplicationAsync(tradePartyId, false, true);

            return _mapper.Map<TradePartyDto>(tradeParty);
        }

        #region private methods
        private string AssignRemosBusinessSchemeNumber(TradeParty party)
        {
            return _tradePartyRepository.AssignRemosBusinessSchemeNumber(party).Result;
        }

        private async Task<TradeParty?> FindTradeParty(Guid id)
        {
            var tradeParty = await _tradePartyRepository
                .FindTradePartyByIdAsync(id);

            if (tradeParty == null)
            {
                return null;
            }
            return tradeParty;
        }

        [ExcludeFromCodeCoverage]
        private async Task SendSignUpApplication(Guid tradepartyId)
        {
            var tradeParty = await FindTradeParty(tradepartyId);
            var logisticsLocations = await _establishmentRepository.GetActiveLogisticsLocationsForTradePartyAsync(tradepartyId);
            try
            {
                var signUpPayload = JsonSerializer.Serialize(new SignUpApplicationMessage
                {
                    TradeParty = new TradePartyData
                    {
                        Id = tradeParty!.Id,
                        OrgId = tradeParty.OrgId,
                        FboNumber = tradeParty.FboNumber,
                        PhrNumber = tradeParty.PhrNumber,
                        CountryName = tradeParty.TradeAddress?.TradeCountry,
                        RemosBusinessSchemeNumber = tradeParty.RemosBusinessSchemeNumber,
                        TermsAndConditionsSignedDate = tradeParty.TermsAndConditionsSignedDate,
                        SignUpRequestSubmittedBy = tradeParty.SignUpRequestSubmittedBy,
                        TradeContact = new TradeContact
                        {
                            Id = tradeParty.TradeContact!.Id,
                            TradePartyId = tradeParty.TradeContact!.Id,
                            PersonName = tradeParty.TradeContact.PersonName,
                            TelephoneNumber = tradeParty.TradeContact.TelephoneNumber,
                            Email = tradeParty.TradeContact.Email,
                            Position = tradeParty.TradeContact.Position,
                            IsAuthorisedSignatory = tradeParty.TradeContact.IsAuthorisedSignatory,
                        },
                        AuthorisedSignatory = new AuthorisedSignatory
                        {
                            Id = tradeParty.AuthorisedSignatory!.Id,
                            TradePartyId = tradeParty.AuthorisedSignatory!.TradePartyId,
                            Name = tradeParty.AuthorisedSignatory.Name,
                            EmailAddress = tradeParty.AuthorisedSignatory.EmailAddress,
                            Position = tradeParty.AuthorisedSignatory.Position
                        },
                        LogisticsLocations = logisticsLocations.Select(x => new LogisticsLocationData
                        {
                            Id = x.Id,
                            Name = x.Name,
                            EmailAddress = x.Email,
                            TradePartyId = x.TradePartyId,
                            RemosEstablishmentSchemeNumber = x.RemosEstablishmentSchemeNumber,
                            Address = new AddressData
                            {
                                LineOne = x.Address?.LineOne,
                                LineTwo = x.Address?.LineTwo,
                                PostCode = x.Address?.PostCode,
                                CityName = x.Address?.CityName,
                                County = x.Address?.County
                            }
                        }).ToList()
                    }
                });

                // Env variables will be added to secrets once we have details from TP
                await SendtoServiceBus(signUpPayload, "sus.remos.signup", tradeParty!.Id);
                
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        [FeatureGate(FeatureFlags.SelfServe)]
        private async Task SendSelfServeApplicationAsync(Guid tradePartyId, bool ContactUpdated, bool AuthRepUpdated)
        {
            var tradeParty = await FindTradeParty(tradePartyId);
            try
            {
                var selfServeMessagePayload = JsonSerializer.Serialize(new SelfServeApplicationMessage
                {
                    TradeParty = new TradePartyUpdateData
                    {
                        Id = tradeParty!.Id,
                        OrgId = tradeParty!.OrgId,
                        SignUpRequestSubmittedBy = tradeParty.SignUpRequestSubmittedBy,
                        TradeContact = ContactUpdated ? new TradeContact
                        {
                            Id = tradeParty!.TradeContact!.Id,
                            PersonName = tradeParty.TradeContact.PersonName,
                            Position = tradeParty.TradeContact.Position,
                            Email = tradeParty.TradeContact.Email,
                            TelephoneNumber = tradeParty.TradeContact.TelephoneNumber
                        }
                        : null,
                        AuthorisedSignatory = AuthRepUpdated ? new AuthorisedSignatory
                        {
                            Id = tradeParty!.AuthorisedSignatory!.Id,
                            Name = tradeParty.AuthorisedSignatory.Name,
                            Position = tradeParty.AuthorisedSignatory.Position,
                            EmailAddress = tradeParty.AuthorisedSignatory.EmailAddress
                        }
                        : null,
                    }
                });
                await SendtoServiceBus(selfServeMessagePayload, "sus.remos.update", tradeParty!.Id ,"2", "Complete");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        private async Task SendtoServiceBus(string payload, string subject, Guid tradePartyId, string schemaVersion = "1", string status = "Created")
        {
            try
            {
                var sender = _serviceBusClient.CreateSender(_tradePlatformIntegrationSettings.Value.ServiceBusName);

                using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
                var message = new ServiceBusMessage(payload)
                {
                    ContentType = "application/json",
                    Subject = subject,
                    CorrelationId = Guid.NewGuid().ToString()
                };
                message.ApplicationProperties.Add("MessageId", Guid.NewGuid().ToString());
                message.ApplicationProperties.Add("EntityKey", tradePartyId.ToString());
                message.ApplicationProperties.Add("PublisherId", "SuS");
                message.ApplicationProperties.Add("SchemaVersion", schemaVersion);
                message.ApplicationProperties.Add("Type", "Internal");
                message.ApplicationProperties.Add("Status", status);
                message.ApplicationProperties.Add("TimestampUtc", ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds());


                if (!messageBatch.TryAddMessage(message))
                {
                    throw new InvalidOperationException($"Message is too large to fit in the batch.");
                }

                await sender.SendMessagesAsync(messageBatch);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion private methods
    }
}


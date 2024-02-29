using AutoMapper;
using Azure.Messaging.ServiceBus;
using Defra.Trade.Address.V1.ApiClient.Api;
using Defra.Trade.Address.V1.ApiClient.Model;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Constants;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Enums;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Services
{
    public class EstablishmentsService : IEstablishmentsService
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ITradePartyRepository _tradePartyRepository;
        private readonly IMapper _mapper;
        private readonly IPlacesApi _placesApi;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly IOptions<TradePlatform> _tradePlatformIntegrationSettings;

        public EstablishmentsService(
            IEstablishmentRepository establishmentRepository, 
            IAddressRepository addressRepository,
            ITradePartyRepository tradePartyRepository,
            IMapper mapper,
            IPlacesApi placesApi,
            ServiceBusClient serviceBusClient,
            IOptions<TradePlatform> tradePlatformIntegrationSettings)
        {
            _establishmentRepository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
            _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
            _tradePartyRepository = tradePartyRepository ?? throw new ArgumentNullException(nameof(tradePartyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _placesApi = placesApi ?? throw new ArgumentNullException(nameof(placesApi));
            _serviceBusClient = serviceBusClient;
            _tradePlatformIntegrationSettings = tradePlatformIntegrationSettings ?? throw new ArgumentNullException(nameof(tradePlatformIntegrationSettings));
        }

        public async Task<LogisticsLocationDto?> GetLogisticsLocationByIdAsync(Guid id)
        {
            var location = await _establishmentRepository.GetLogisticsLocationByIdAsync(id);
            var locationDto = _mapper.Map<LogisticsLocationDto>(location);
            return locationDto;
        }

        public async Task<IEnumerable<LogisticsLocationDto>?> GetAllLogisticsLocationsAsync()
        {
            var locations = await _establishmentRepository.GetAllLogisticsLocationsAsync();
            var locationDtos = _mapper.Map<IEnumerable<LogisticsLocationDto>>(locations);
            return locationDtos;
        }

        public async Task<IEnumerable<LogisticsLocationDto>?> GetLogisticsLocationsByPostcodeAsync(string postcode)
        {
            var locations = await _establishmentRepository.GetLogisticsLocationByPostcodeAsync(postcode);
            var locationDtos = _mapper.Map<IEnumerable<LogisticsLocationDto>>(locations);

            return locationDtos;
        }

        public LogisticsLocationDto? GetLogisticsLocationByUprnAsync(string uprn)
        {
            var tradeAddressApi = _placesApi.UprnLookupAsync(uprn);

            if (tradeAddressApi != null)
            {
                var tradeAddress = _mapper.Map<TradeAddressAndBusinessNameDto>(tradeAddressApi.FirstOrDefault());

                var postcodeRegion = Regex.Match(tradeAddress.TradeAddress!.PostCode!, @"^[^0-9]*", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(60)).Value;
                var logisticsLocatinDto = new LogisticsLocationDto
                {
                    Address = tradeAddress.TradeAddress,
                    Name = tradeAddress.BusinessName,
                    NI_GBFlag = postcodeRegion.Contains("BT") ? "NI" : "GB"
                };

                return logisticsLocatinDto;
            }
            else
            {
                return null;
            }

        }

        public async Task<IEnumerable<LogisticsLocationDto>?> GetLogisticsLocationsForTradePartyAsync(Guid tradePartyId)
        {
            if (!await _tradePartyRepository.TradePartyExistsAsync(tradePartyId))
                return null;

            var locations = await _establishmentRepository.GetActiveLogisticsLocationsForTradePartyAsync(tradePartyId);
            var locationDtos = _mapper.Map<IEnumerable<LogisticsLocationDto>>(locations);
            return locationDtos;

        }

        public async Task<IEnumerable<LogisticsLocationDto>?> GetAllLogisticsLocationsForTradePartyAsync(Guid tradePartyId)
        {
            if (!await _tradePartyRepository.TradePartyExistsAsync(tradePartyId))
                return null;

            var locations = await _establishmentRepository.GetAllLogisticsLocationsForTradePartyAsync(tradePartyId);
            var locationDtos = _mapper.Map<IEnumerable<LogisticsLocationDto>>(locations.Where(loc => !loc.IsRemoved));
            return locationDtos;

        }

        public async Task<LogisticsLocationDto?> AddLogisticsLocationAsync(Guid tradePartyId, LogisticsLocationDto dto)
        {            
            if (!await _tradePartyRepository.TradePartyExistsAsync(tradePartyId))
            {
                return null;
            }

            if (dto.TradeAddressId != null && dto.TradeAddressId != Guid.Empty && !await _addressRepository.AddressExistsAsync(dto.TradeAddressId.Value))
            {
                return null;
            }

            var addressEntity = _mapper.Map<TradeAddress>(dto.Address);
            await _addressRepository.AddAddressAsync(addressEntity);
            await _addressRepository.SaveChangesAsync();

            dto.TradeAddressId = addressEntity.Id;
            dto.TradePartyId = tradePartyId;
            LogisticsLocation newLocation = _mapper.Map<LogisticsLocation>(dto);
            
            newLocation.Id = Guid.NewGuid();
            newLocation.CreatedDate = DateTime.UtcNow;
            newLocation.LastModifiedDate = DateTime.UtcNow;
            newLocation.RemosEstablishmentSchemeNumber = await GenerateEstablishmentRemosSchemeNumber(tradePartyId);

            await _establishmentRepository.AddLogisticsLocationAsync(newLocation);
            await _establishmentRepository.SaveChangesAsync();

            var locationToReturnDto = _mapper.Map<LogisticsLocationDto>(newLocation);
            return locationToReturnDto;
        }       

        public List<AddressDto> GetTradeAddressApiByPostcode(string postcode)
        {
            var addressList = new List<AddressDto>();
            try
            {
                return addressList = _placesApi.PostCodeLookupAsync(postcode);
            }
            catch
            {
                return addressList;
            }
        }

        public async Task<LogisticsLocationDto?> UpdateLogisticsLocationAsync(Guid id, LogisticsLocationDto logiticsLocationRequest)
        {
            LogisticsLocation? logisticsLocation = await _establishmentRepository.GetLogisticsLocationByIdAsync(id);
            if (logisticsLocation == null)
                return null;

            var remosNo = logisticsLocation.RemosEstablishmentSchemeNumber;

            _mapper.Map(logiticsLocationRequest, logisticsLocation);

            logisticsLocation.RemosEstablishmentSchemeNumber = remosNo;
            _establishmentRepository.UpdateLogisticsLocation(logisticsLocation);
            await _establishmentRepository.SaveChangesAsync();

            return _mapper.Map<LogisticsLocationDto>(logisticsLocation);
        }

        public async Task<bool> RemoveLogisticsLocationAsync(Guid id)
        {
            var location = await _establishmentRepository.GetLogisticsLocationByIdAsync(id);
            
            if (location == null) return false;

            _establishmentRepository.RemoveLogisticsLocation(location);
            await _establishmentRepository.SaveChangesAsync();

            return true;
        }

        public async Task<string> GenerateEstablishmentRemosSchemeNumber(Guid tradePartyId)
        {
            var tradeParty = await _tradePartyRepository.GetTradePartyAsync(tradePartyId);
            var locations = await _establishmentRepository.GetAllLogisticsLocationsForTradePartyAsync(tradePartyId);

            // +1 includes the newly created location about to be created
            var totalLocations = locations.Count() + 1;

            var remosNumber = $"{tradeParty!.RemosBusinessSchemeNumber}-{totalLocations.ToString().PadLeft(3, '0')}";

            return remosNumber;            
        }

        public async Task<bool> EstablishmentAlreadyExists(LogisticsLocationDto dto)
        {
            if (await _establishmentRepository.LogisticsLocationAlreadyExists(
               dto.Name ?? string.Empty,
               dto.Address?.LineOne ?? string.Empty,
               dto.Address?.PostCode ?? string.Empty,
               dto.Id))
            {
                return true;
            }

            return false;
        }


        [FeatureGate(FeatureFlags.SelfServeMvpPlus)]
        public async Task<LogisticsLocationDto?> UpdateLogisticsLocationSelfServeAsync(Guid id, LogisticsLocationDto logisticsLocationRequest)
        {
            var logisticsLocation = await UpdateLogisticsLocationAsync(id, logisticsLocationRequest);

            if (logisticsLocation == null)
            {
                return null;
            }

            var partyId = (Guid)logisticsLocation.TradePartyId!;

            if (logisticsLocation.ApprovalStatus == LogisticsLocationApprovalStatus.Removed)
            {
                await SendSelfServeApplicationAsync(partyId, logisticsLocation.Id, "update");
                return logisticsLocation;
            }

            await SendSelfServeApplicationAsync(partyId, logisticsLocation.Id, "add");
            return logisticsLocation;

        }

        [FeatureGate(FeatureFlags.SelfServeMvpPlus)]
        private async Task SendSelfServeApplicationAsync(Guid tradePartyId, Guid establishmentId, string addOrUpdate)
        {
            var tradeParty = await _tradePartyRepository.FindTradePartyByIdAsync(tradePartyId);
            var establishment = await _establishmentRepository.GetLogisticsLocationByIdAsync(establishmentId);
            try
            {
                string? selfServeMessagePayload;
                switch (addOrUpdate)
                {
                    case "add":
                        selfServeMessagePayload = BuildSelfServeAddEstablishmentMessage(tradeParty, establishment);
                        if(!string.IsNullOrEmpty(selfServeMessagePayload)) 
                            await SendtoServiceBus(selfServeMessagePayload, "sus.remos.establishment.create", tradePartyId, "3", "Created");
                        break;
                    case "update":
                        selfServeMessagePayload = BuildSelfServeUpdateEstablishmentMessage(tradeParty, establishment);
                        if (!string.IsNullOrEmpty(selfServeMessagePayload))
                            await SendtoServiceBus(selfServeMessagePayload, "sus.remos.establishment.update", tradePartyId, "4", "Completed");
                        break;
                    default:
                        break;
                }
                
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        [FeatureGate(FeatureFlags.SelfServeMvpPlus)]
        private string? BuildSelfServeUpdateEstablishmentMessage(TradeParty? tradeParty, LogisticsLocation? establishment)
        {
            var selfServeMessagePayload = JsonSerializer.Serialize(new SelfServeUpdateEstablishmentMessage
            {
                TradePartyWithLogicsLocationUpdateData = new TradePartyWithLogicsLocationUpdateData
                {
                    Id = tradeParty!.Id,
                    OrgId = tradeParty!.OrgId,
                    LogisticsLocationStatusUpdate = new LogisticsLocationDataForUpdate
                    {
                        Id = establishment!.Id,
                        TradePartyId = tradeParty!.Id,
                        InspectionLocationId = establishment.InspectionLocationId,
                        Status = establishment.ApprovalStatus.ToString(),
                    }
                }
            });

            return selfServeMessagePayload;
        }

        [FeatureGate(FeatureFlags.SelfServeMvpPlus)]
        private string? BuildSelfServeAddEstablishmentMessage(TradeParty? tradeParty, LogisticsLocation? establishment)
        {
            var selfServeMessagePayload = JsonSerializer.Serialize(new SelfServeAddEstablishmentMessage
            {
                TradePartyWithLogicsLocationData = new TradePartyWithLogicsLocationData
                {
                    Id = tradeParty!.Id,
                    OrgId = tradeParty!.OrgId,

                    LogisticsLocation = new LogisticsLocationData
                    {
                        Id = establishment!.Id,
                        Name = establishment!.Name,
                        EmailAddress = establishment!.Email,
                        TradePartyId = tradeParty!.Id,
                        RemosEstablishmentSchemeNumber = establishment!.RemosEstablishmentSchemeNumber,
                        Address = new AddressData
                        {
                            LineOne = establishment.Address?.LineOne,
                            LineTwo = establishment.Address?.LineTwo,
                            PostCode = establishment.Address?.PostCode,
                            CityName = establishment.Address?.CityName,
                            County = establishment.Address?.County,
                        }
                    }
                }
            });

            return selfServeMessagePayload;
        }

        private async Task SendtoServiceBus(string payload, string subject, Guid tradePartyId, string schemaVersion = "3", string status = "Created")
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
    }
}

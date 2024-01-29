using AutoMapper;
using Defra.Trade.Address.V1.ApiClient.Api;
using Defra.Trade.Address.V1.ApiClient.Model;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Microsoft.Azure.Management.AppService.Fluent.Models;
using System.Text.RegularExpressions;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Services
{
    public class EstablishmentsService : IEstablishmentsService
    {
        private readonly IEstablishmentRepository _establishmentRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ITradePartyRepository _tradePartyRepository;
        private readonly IOsPlacesRepository _osPlacesRepository;
        private readonly IMapper _mapper;
        private readonly IPlacesApi _placesApi;

        public EstablishmentsService(
            IEstablishmentRepository establishmentRepository, 
            IAddressRepository addressRepository,
            ITradePartyRepository tradePartyRepository,
            IOsPlacesRepository osPlacesRepository,
            IMapper mapper,
            IPlacesApi placesApi)
        {
            _establishmentRepository = establishmentRepository ?? throw new ArgumentNullException(nameof(establishmentRepository));
            _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
            _tradePartyRepository = tradePartyRepository ?? throw new ArgumentNullException(nameof(tradePartyRepository));
            _osPlacesRepository = osPlacesRepository ?? throw new ArgumentNullException(nameof(osPlacesRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _placesApi = placesApi ?? throw new ArgumentNullException(nameof(placesApi));
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

        public async Task<IEnumerable<LogisticsLocationDto>> GetOSPlacesTradeLocations(string PostCode)
        {
            var logisticLocationDtos = new List<LogisticsLocationDto>();
            var osPlaces = await _osPlacesRepository.GetOSPlacesLocationsFromPostCodeAsync(PostCode);

            if (osPlaces != null)
            {
                var tradeAddresses = _mapper.Map<IEnumerable<TradeAddressDto>>(osPlaces.Results!.Select(x => x.Dpa));
                foreach (var tradeAddress in tradeAddresses)
                {
                    var logisticLocationDto = new LogisticsLocationDto
                    {
                        Address = tradeAddress
                    };

                    logisticLocationDtos.Add(logisticLocationDto);
                }
            }

            return logisticLocationDtos;
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
    }
}

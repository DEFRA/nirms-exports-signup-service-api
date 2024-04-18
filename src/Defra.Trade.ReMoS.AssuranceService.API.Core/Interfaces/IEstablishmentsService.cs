using Defra.Trade.Address.V1.ApiClient.Model;
using Defra.Trade.ReMoS.AssuranceService.Shared.DTO;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Interfaces
{
    public interface IEstablishmentsService
    {
        public Task<LogisticsLocationDto?> GetLogisticsLocationByIdAsync(Guid id);
        public Task<IEnumerable<LogisticsLocationDto>?> GetAllLogisticsLocationsAsync();
        public Task<IEnumerable<LogisticsLocationDto>?> GetLogisticsLocationsByPostcodeAsync(string postcode);
        public Task<IEnumerable<LogisticsLocationDto>?> GetLogisticsLocationsForTradePartyAsync(Guid tradePartyId);
        public Task<IEnumerable<LogisticsLocationDto>?> GetAllLogisticsLocationsForTradePartyAsync(Guid tradePartyId);
        public Task<LogisticsLocationDto?> AddLogisticsLocationAsync(Guid tradePartyId, LogisticsLocationDto dto);
        Task<LogisticsLocationDto?> UpdateLogisticsLocationAsync(Guid id, LogisticsLocationDto logiticsLocationRequest);
        Task<bool> RemoveLogisticsLocationAsync(Guid id);
        LogisticsLocationDto? GetLogisticsLocationByUprnAsync(string uprn);
        List<AddressDto> GetTradeAddressApiByPostcode(string postcode);
        Task<bool> EstablishmentAlreadyExists(LogisticsLocationDto dto);
        Task<LogisticsLocationDto?> UpdateLogisticsLocationSelfServeAsync(Guid id, LogisticsLocationDto logisticsLocationRequest);
    }
}

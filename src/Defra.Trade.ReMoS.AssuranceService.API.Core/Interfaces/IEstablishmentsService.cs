using Defra.Trade.Address.V1.ApiClient.Model;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Helpers;
using Defra.Trade.ReMoS.AssuranceService.Shared.DTO;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Interfaces
{
    public interface IEstablishmentsService
    {
        public Task<LogisticsLocationDto?> GetLogisticsLocationByIdAsync(Guid id);
        public Task<IEnumerable<LogisticsLocationDto>?> GetAllLogisticsLocationsAsync();
        public Task<IEnumerable<LogisticsLocationDto>?> GetLogisticsLocationsByPostcodeAsync(string postcode);
        public Task<PagedList<LogisticsLocationDto>?> GetLogisticsLocationsForTradePartyAsync(Guid tradePartyId, int pageNumber = 1, int pageSize = 10);
        public Task<PagedList<LogisticsLocationDto>?> GetAllLogisticsLocationsForTradePartyAsync(Guid tradePartyId, int pageNumber = 1, int pageSize = 10);
        public Task<LogisticsLocationDto?> AddLogisticsLocationAsync(Guid tradePartyId, LogisticsLocationDto dto);
        Task<LogisticsLocationDto?> UpdateLogisticsLocationAsync(Guid id, LogisticsLocationDto logiticsLocationRequest);
        Task<bool> RemoveLogisticsLocationAsync(Guid id);
        LogisticsLocationDto? GetLogisticsLocationByUprnAsync(string uprn);
        List<AddressDto> GetTradeAddressApiByPostcode(string postcode);
        Task<bool> EstablishmentAlreadyExists(LogisticsLocationDto dto, Guid? partyId = null);
        Task<LogisticsLocationDto?> UpdateLogisticsLocationSelfServeAsync(Guid id, LogisticsLocationDto logisticsLocationRequest);
    }
}

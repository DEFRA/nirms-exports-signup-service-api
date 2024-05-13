using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces
{
    public interface IEstablishmentRepository
    {
        public Task<LogisticsLocation> AddLogisticsLocationAsync(LogisticsLocation location);
        public Task<LogisticsLocation?> GetLogisticsLocationByIdAsync(Guid id);
        public Task<IEnumerable<LogisticsLocation>> GetAllLogisticsLocationsAsync();
        public Task<IEnumerable<LogisticsLocation>?> GetLogisticsLocationByPostcodeAsync(string postcode);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        public void UpdateLogisticsLocation(LogisticsLocation logisticsLocation);
        Task<IEnumerable<LogisticsLocation>> GetActiveLogisticsLocationsForTradePartyAsync(Guid tradePartyId, string? NI_GBFlag);
        Task<IEnumerable<LogisticsLocation>> GetAllLogisticsLocationsForTradePartyAsync(Guid tradePartyId, string? NI_GBFlag);
        public void RemoveLogisticsLocation(LogisticsLocation logisticsLocation);
        public Task<bool> LogisticsLocationAlreadyExists(string name, string addressLineOne, string postcode, Guid? exceptThisLocationId = null, Guid? partyId = null);
    }
}

using Microsoft.FeatureManagement.Mvc;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Interfaces
{
    public interface ITradePartiesService
    {
        public Task<IEnumerable<TradePartyDto>> GetTradePartiesAsync();
        public Task<TradePartyDto?> AddTradePartyAsync(TradePartyDto tradePartyRequest);
        public Task<TradePartyDto?> UpdateTradePartyAsync(Guid tradePartyId, TradePartyDto tradePartyRequest);
        public Task<TradePartyDto?> UpdateTradePartyAddressAsync(Guid tradePartyId, TradePartyDto tradePartyRequest);
        public Task<TradePartyDto?> UpdateTradePartyContactAsync(Guid tradePartyId, TradePartyDto tradePartyRequest);
        public Task<TradePartyDto?> GetTradePartyAsync(Guid tradePartyId);
        public Task<TradePartyDto?> UpdateAuthorisedSignatoryAsync(Guid tradePartyId, TradePartyDto tradePartyRequest);
        Task<TradePartyDto?> AddTradePartyAddressAsync(Guid tradePartyId, TradeAddressDto tradeAddressRequest);
        Task<TradePartyDto?> GetTradePartyByDefraOrgIdAsync(Guid orgId);

        [FeatureGate(FeatureFlags.SelfServe)]
        Task<TradePartyDto?> UpdateContactSelfServeAsync(Guid tradePartyId, TradePartyDto tradePartyRequest);
        [FeatureGate(FeatureFlags.SelfServe)]
        Task<TradePartyDto?> UpdateAuthRepSelfServeAsync(Guid tradePartyId, TradePartyDto tradePartyRequest);
    }
}

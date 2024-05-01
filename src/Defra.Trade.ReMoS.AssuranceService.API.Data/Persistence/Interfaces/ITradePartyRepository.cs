using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces;
public interface ITradePartyRepository
{
    Task AddTradePartyAsync(TradeParty party, CancellationToken cancellationToken = default);
    TradeParty? UpdateTradeParty(TradeParty party, CancellationToken cancellationToken = default);
    TradeParty? UpdateTradePartyAddress(TradeParty party, CancellationToken cancellationToken = default);
    Task <TradeParty?> UpsertAuthorisedSignatory(TradeParty party, CancellationToken cancellationToken = default);
    Task <TradeParty?> UpsertTradePartyContact(TradeParty party, CancellationToken cancellationToken = default);    
    Task<TradeParty?> FindTradePartyByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TradeParty>> GetAllTradeParties();
    Task<TradeParty?> GetTradePartyAsync(Guid tradePartyId);
    Task<TradeParty?> GetTradePartyByDefraOrgIdAsync(Guid orgId);
    Task<bool> TradePartyExistsAsync(Guid partyId);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<string> AssignRemosBusinessSchemeNumber(TradeParty party);
    void AddTradePartyAddress(TradeParty party, TradeAddress address, CancellationToken cancellationToken = default);
}
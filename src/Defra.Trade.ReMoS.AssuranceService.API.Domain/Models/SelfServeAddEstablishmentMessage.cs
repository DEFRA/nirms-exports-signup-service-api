namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Models;

public class SelfServeAddEstablishmentMessage
{
    public TradePartyWithLogicsLocationData? TradeParty { get; set; }
}

public record TradePartyWithLogicsLocationData
{
    public Guid Id { get; init; }
    public Guid OrgId { get; init; }
    public LogisticsLocationData? LogisticsLocation { get; init; }
}

public class SelfServeUpdateEstablishmentMessage
{
    public TradePartyWithLogicsLocationUpdateData? TradeParty { get; set; }
}

public record TradePartyWithLogicsLocationUpdateData
{
    public Guid Id { get; init; }
    public Guid OrgId { get; init; }
    public LogisticsLocationDataForUpdate? LogisticsLocation { get; init; }
}
using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Models;

public class SelfServeAddEstablishmentMessage
{
    public TradePartyWithLogicsLocationData? TradePartyWithLogicsLocationData { get; set; }
}

[ExcludeFromCodeCoverage]
public record TradePartyWithLogicsLocationData
{
    public Guid Id { get; init; }
    public Guid OrgId { get; init; }
    public LogisticsLocationData? LogisticsLocation { get; init; }
}

[ExcludeFromCodeCoverage]
public class SelfServeUpdateEstablishmentMessage
{
    public TradePartyWithLogicsLocationUpdateData? TradePartyWithLogicsLocationUpdateData { get; set; }
}
[ExcludeFromCodeCoverage]
public record TradePartyWithLogicsLocationUpdateData
{
    public Guid Id { get; init; }
    public Guid OrgId { get; init; }
    public LogisticsLocationDataForUpdate? LogisticsLocationStatusUpdate { get; init; }
}
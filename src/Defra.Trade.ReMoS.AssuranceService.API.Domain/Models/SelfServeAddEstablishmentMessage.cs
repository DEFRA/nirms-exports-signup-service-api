using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Models;

public class SelfServeAddEstablishmentMessage
{
    public TradePartyWithLogicsLocationData? TradeParty { get; set; }
}

[ExcludeFromCodeCoverage]
public record TradePartyWithLogicsLocationData
{
    public Guid Id { get; init; }
    public Guid OrgId { get; init; }
    public LogisticsLocationData? LogisticsLocation { get; init; }
}
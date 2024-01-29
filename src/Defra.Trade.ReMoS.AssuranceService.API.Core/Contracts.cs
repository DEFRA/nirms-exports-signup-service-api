using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core;
[ExcludeFromCodeCoverage]
public class Contracts
{
    public record AddTradePartyContract(
        string? Name);
}

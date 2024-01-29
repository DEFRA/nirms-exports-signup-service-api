using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Constants;

[ExcludeFromCodeCoverage]
public static class FeatureFlags
{
    public const string SignUpApplication = "Nirms-SuS-SusToIdcomsSync";
    public const string SelfServe = "Nirms-SuS-SelfServe";
}

using Defra.Trade.Common.Security.AzureKeyVault.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Models
{
    [ExcludeFromCodeCoverage]
    public class AppConfigurationSettings
    {
        public const string KeyVaultSecretsSettingsName = "KeyVaultSecretsSettings";

        [SecretName("GCOsplace-ApiKey")]
        public string? OsPlacesApiKey { get; set; }

        public string? OsPlacesUrl { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class AddressApiConfig
    {
        public string? BaseUrl { get; set; }
    }
}

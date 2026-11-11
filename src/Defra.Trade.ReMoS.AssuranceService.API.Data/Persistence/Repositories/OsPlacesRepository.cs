using Defra.Trade.ReMoS.AssuranceService.API.Core.Models;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Repositories;

[ExcludeFromCodeCoverage]
public class OsPlacesRepository(IOptions<AppConfigurationSettings> appConfiguration, ILogger<OsPlacesRepository> logger) : IOsPlacesRepository
{
    private readonly IOptions<AppConfigurationSettings> _appConfiguration = appConfiguration;
    private readonly ILogger<OsPlacesRepository> _logger = logger;

    public async Task<OsPlaces> GetOSPlacesLocationsFromPostCodeAsync(string postCode)
    {
        _logger.LogInformation("GetOSPlacesLocationsFromPostCodeAsync for Postcode: {PostCode}", postCode);

        var osPlaces = new OsPlaces
        {
            Results = []
        };

        using (var client = new HttpClient())
        {
            var osPlacesUrl = _appConfiguration.Value.OsPlacesUrl;
            var osKey = _appConfiguration.Value.OsPlacesApiKey;
  
            var osPlacesEndpoint = $"{osPlacesUrl}/postcode?postcode=" + postCode + "&dataset=DPA&lr=EN&maxresults=100&key=" + osKey;

            _logger.LogInformation("Contacting {OsPlacesEndpoint}", osPlacesEndpoint);

            var response = await client.GetAsync(osPlacesEndpoint).ConfigureAwait(true);

            if (response.IsSuccessStatusCode)
            {
                var deserializedObject = JsonConvert.DeserializeObject<OsPlaces>(response.Content.ReadAsStringAsync().Result);
                osPlaces.Header = deserializedObject!.Header;

                if (deserializedObject.Results != null)
                {
                    foreach (var result in deserializedObject.Results)
                    {
                        osPlaces.Results.Add(result);
                    }
                }
                _logger.LogInformation($"Sucess code returned from OSPlaces API");
            }
        }

        return osPlaces;
    }
}

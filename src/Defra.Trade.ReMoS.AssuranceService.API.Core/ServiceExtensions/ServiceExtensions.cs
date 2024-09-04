using Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Profiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Services;
using System.Diagnostics.CodeAnalysis;
using Defra.Trade.Address.V1.ApiClient.Api;
using Defra.Trade.Address.V1.ApiClient.Client;
using Defra.Trade.Common.Config;
using Defra.Trade.Common.Security.Authentication.Infrastructure;
using Microsoft.Extensions.Options;
using Defra.Trade.Common.Security.Authentication.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Models;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Models;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.ServiceExtensions;

[ExcludeFromCodeCoverage]
public static class ServiceExtensions
{
    public static IServiceCollection AddServiceConfigurations(this IServiceCollection services)
    {
        services.AddTransient<ITradePartiesService, TradePartiesService>();
        services.AddTransient<IEstablishmentsService, EstablishmentsService>();
        AddAutoMapperConfiguration(services);
        return services;
    }

    public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
    {
        var type = typeof(TradePartyProfiler);
        var assemblyFromType = type.Assembly;
        services.AddAutoMapper(assemblyFromType);

        return services;
    }

    public static IServiceCollection AddApplicationApis(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<AddressApiConfig>(configuration.GetSection("AddressApi"))
            .Configure<ApimInternalSettings>(configuration.GetSection(ApimInternalSettings.OptionsName))
            .Configure<TradePlatform>(configuration.GetSection("TradePlatform"));
        services.AddApimAuthentication(configuration.GetSection(ApimSettings.InternalApim));
        services.AddTransient<IPlacesApi>((provider) =>
            new PlacesApi(CreateApiClientConfigurationSettings(provider, configuration)));
        return services;
    }

    private static Configuration CreateApiClientConfigurationSettings(IServiceProvider provider, IConfiguration configuration)
    {
        var authService = provider.GetService<IAuthenticationService>();
        var apimInternalApisSettings = provider.GetRequiredService<IOptionsSnapshot<ApimInternalSettings>>().Value;
        var authToken = authService!.GetAuthenticationHeaderAsync().Result.ToString();
        var config = new Configuration
        {
            BasePath = configuration.GetValue<string>("AddressApi:BaseUrl"),
            DefaultHeaders = new Dictionary<string, string>
                {
                    { apimInternalApisSettings.AuthorizationHeaderName, authToken },
                    { apimInternalApisSettings.SubscriptionKeyHeaderName, apimInternalApisSettings.SubscriptionKey}
                }
        };
        return config;
    }
}
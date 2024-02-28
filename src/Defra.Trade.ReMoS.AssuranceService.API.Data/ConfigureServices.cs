using System.Diagnostics.CodeAnalysis;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Models;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Context;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class ConfigureServices
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddTransient<ITradePartyRepository, TradePartyRepository>();
        services.AddTransient<IEstablishmentRepository, EstablishmentRepository>();
        services.AddTransient<IAddressRepository, AddressRepository>();
        services.AddTransient<IOsPlacesRepository, OsPlacesRepository>();

        services.Configure<AppConfigurationSettings>(configuration.GetSection(AppConfigurationSettings.KeyVaultSecretsSettingsName));

        return services;
    }

    public static async Task<IApplicationBuilder> InitialiseDatabaseAsync(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
            await initialiser.InitialiseAsync();
        }

        return app;
    }
}

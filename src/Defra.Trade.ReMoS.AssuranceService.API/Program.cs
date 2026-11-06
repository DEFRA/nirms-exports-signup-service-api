using System.Diagnostics.CodeAnalysis;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Defra.Trade.Common.AppConfig;
using Defra.Trade.Common.Api.Infrastructure;
using Defra.Trade.Common.Sql.Infrastructure;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Models;
using Defra.Trade.ReMoS.AssuranceService.API.Core.ServiceExtensions;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Context;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;

namespace Defra.Trade.ReMoS.AssuranceService.API.Controllers;

/// <summary>
/// Main program class
/// </summary>
[ExcludeFromCodeCoverage]
public class Program
{
    /// <summary>
    /// Main method to start the application
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.ConfigureTradeAppConfiguration(options =>
                {
                    options.UseKeyVaultSecrets = true;
                    options.RefreshKeys.Add("RemosSignUpService:Sentinel");
                });

        var configuration = builder.Configuration;
        var services = builder.Services;

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("sql_db")));

        services.AddControllers();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddTradeApi(configuration);
        services.AddTradeSql(configuration);
        services.AddHealthChecks();

        services.AddDataServices(configuration);

        services.AddServiceConfigurations();
        services.AddApplicationApis(configuration);

        services.AddFeatureManagement();

        services.Configure<AppConfigurationSettings>(options =>
        {
            options.OsPlacesUrl = configuration.GetValue<string>("OsPlaces:Url");
            options.OsPlacesApiKey = configuration.GetValue<string>("OsPlaces:ApiKey");
        });

        var serviceBusString = configuration.GetValue<string>("TradePlatform:ServiceBusConnectionString");
        services.Configure<TradePlatform>(options =>
        {
            options.ServiceBusConnectionString = serviceBusString;
            options.ServiceBusName = configuration.GetValue<string>("TradePlatform:ServiceBusName");
        });

        var serviceBusOptions = new ServiceBusClientOptions() { TransportType = ServiceBusTransportType.AmqpWebSockets };
        if (configuration.GetValue<string>("ServiceBusEnvironment") == "Local")
        {
            services.AddSingleton((s) =>
            {
                return new ServiceBusClient(serviceBusString, serviceBusOptions);
            });
        }
        else
        {
            services.AddSingleton((s) =>
            {
                return new ServiceBusClient(serviceBusString, new ManagedIdentityCredential(), serviceBusOptions);
            });
        }

        var app = builder.Build();

        app.UseTradeApp(app.Environment);

        app.Run();
    }
}

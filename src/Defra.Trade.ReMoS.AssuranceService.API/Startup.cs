using Defra.Trade.Common.Api.Infrastructure;
using Defra.Trade.Common.Sql.Infrastructure;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Models;
using Defra.Trade.ReMoS.AssuranceService.API.Core.ServiceExtensions;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Models;
using Azure.Messaging.ServiceBus;
using Azure.Identity;
using System.Net.Http;
using Microsoft.FeatureManagement;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace Defra.Trade.ReMoS.AssuranceService.API
{
    /// <summary>
    /// Startup
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Startup
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configure services
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("sql_db")));            

            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTradeApi(Configuration);
            services.AddTradeSql(Configuration);
            services.AddHealthChecks();

            services.AddDataServices(Configuration);

            services.AddServiceConfigurations();
            services.AddApplicationApis(Configuration);

            services.AddFeatureManagement();

            services.Configure<AppConfigurationSettings>(options =>
            {
                options.OsPlacesUrl = Configuration.GetValue<string>("OsPlaces:Url");
                options.OsPlacesApiKey = Configuration.GetValue<string>("OsPlaces:ApiKey");
            });

            var serviceBusString = Configuration.GetValue<string>("TradePlatform:ServiceBusConnectionString");
            services.Configure<TradePlatform>(options =>
            {
                options.ServiceBusConnectionString = serviceBusString;
                options.ServiceBusName = Configuration.GetValue<string>("TradePlatform:ServiceBusName");
            });

            var serviceBusOptions = new ServiceBusClientOptions() { TransportType = ServiceBusTransportType.AmqpWebSockets };
            if (Configuration.GetValue<string>("ServiceBusEnvironment") == "Local")
            {
                services.AddSingleton((s) => {
                    return new ServiceBusClient(serviceBusString, serviceBusOptions);
                });
            }
            else
            {
                services.AddSingleton((s) => {
                    return new ServiceBusClient(serviceBusString, new ManagedIdentityCredential(), serviceBusOptions);
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseTradeApp(env);
        }
    }

}

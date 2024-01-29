using System.Diagnostics.CodeAnalysis;
using Defra.Trade.Common.AppConfig;

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
        CreateHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// Creates build host
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((config) =>
            {
                var settings = config.Build();
                if (settings.GetValue<bool>("enableAppConfigServer"))
                {
                    config.ConfigureTradeAppConfiguration(true, "RemosSignUpService:Sentinel");
                }
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}

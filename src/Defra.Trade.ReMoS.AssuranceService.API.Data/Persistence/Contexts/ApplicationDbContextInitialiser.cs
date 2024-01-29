using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Context;

[ExcludeFromCodeCoverage]
public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.GetPendingMigrationsAsync().Result.Any())
            {   
                await _context.Database.MigrateAsync();
                await SeedAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }   

    private async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (!_context.TradeParties.Any())
        {
            _context.TradeParties.AddRange(
                new TradeParty
                {
                    Id = Guid.Parse("032d2ed8-f8d9-4675-a04c-af9e8368faaf"),
                    Name = "Company A Ltd",
                    
                },
                new TradeParty
                {
                    Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689"),
                    Name = "Company B Ltd"
                }
            );

            await _context.SaveChangesAsync();
        }
    }
}


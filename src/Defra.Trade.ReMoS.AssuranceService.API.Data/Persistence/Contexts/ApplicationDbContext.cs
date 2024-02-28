using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;


namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Context;

[ExcludeFromCodeCoverage]
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<TradeParty> TradeParties { get; set; } = default!;
    public DbSet<TradeContact> TradeContacts { get; set; } = default!;
    public DbSet<TradeAddress> TradeAddresses { get; set; } = default!;
    public DbSet<LogisticsLocation> LogisticsLocation { get; set; } = default!;
    public DbSet<AuthorisedSignatory> AuthorisedSignatory { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TradePartyConfiguration).Assembly);

        modelBuilder.HasSequence<int>("RemosGbIdHelper", schema: "dbo").StartsAt(000001).IncrementsBy(1);
        modelBuilder.HasSequence<int>("RemosNiIdHelper", schema: "dbo").StartsAt(000001).IncrementsBy(1);

        base.OnModelCreating(modelBuilder);
    }
}

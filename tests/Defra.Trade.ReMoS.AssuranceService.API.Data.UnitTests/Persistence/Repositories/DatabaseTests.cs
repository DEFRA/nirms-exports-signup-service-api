using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Context;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Repositories;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq.EntityFrameworkCore;
using Moq;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.UnitTests.Persistence.Repositories;


public class DatabaseTests
{
    private const string ConnectionString = @"Server=LUK-00427871445;Database=AssuranceServiceDb;Trusted_Connection=True;MultipleActiveResultSets=true";
    private static readonly object _lock = new();
    private static readonly bool _databaseInitialised = true;

    public DatabaseTests()
    {
        lock (_lock)
        {
            if (!_databaseInitialised)
            {
                using var context = CreateContext();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
    }


    public static ApplicationDbContext CreateContext()
        => new(
            new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(ConnectionString)
            .Options);
}
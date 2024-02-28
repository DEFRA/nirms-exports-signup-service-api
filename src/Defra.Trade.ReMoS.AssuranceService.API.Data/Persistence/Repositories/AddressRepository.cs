using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Context;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Repositories;

[ExcludeFromCodeCoverage]
public class AddressRepository : IAddressRepository
{
    private readonly ApplicationDbContext _context;

    public AddressRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddAddressAsync(TradeAddress address, CancellationToken cancellationToken = default)
    {
        await _context.TradeAddresses.AddAsync(address, CancellationToken.None);
    }

    public async Task<bool> AddressExistsAsync(Guid tradeAddressId)
    {
        return await _context.TradeAddresses.AnyAsync(t => t.Id == tradeAddressId);
    }

    public async Task<TradeAddress?> GetAddressByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context
            .TradeAddresses
            .AsNoTracking()
            .FirstOrDefaultAsync(address => address.Id == id, CancellationToken.None);
    }
    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) >= 0;
    }
}

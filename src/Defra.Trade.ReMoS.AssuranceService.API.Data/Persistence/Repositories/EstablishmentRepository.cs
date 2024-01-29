using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Configurations;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Context;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Repositories;

[ExcludeFromCodeCoverage]
public class EstablishmentRepository : IEstablishmentRepository
{
    private readonly ApplicationDbContext _context;

    public EstablishmentRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<LogisticsLocation> AddLogisticsLocationAsync(LogisticsLocation location)
    {
        location.Address = await _context.TradeAddresses.FirstOrDefaultAsync(x => x.Id == location.TradeAddressId);
        await _context.LogisticsLocation.AddAsync(location);
        await _context.SaveChangesAsync();
        return location;

    }

    public async Task<LogisticsLocation?> GetLogisticsLocationByIdAsync(Guid id)
    {
        return await _context
            .LogisticsLocation
            .AsNoTracking()
            .Include(x => x.Address)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<LogisticsLocation>> GetAllLogisticsLocationsAsync()
    {
        return await _context
            .LogisticsLocation
            .AsNoTracking()
            .Where(loc => loc.ApprovalStatus != LogisticsLocationApprovalStatus.Rejected)
            .ToListAsync();
    }

    public async Task<IEnumerable<LogisticsLocation>?> GetLogisticsLocationByPostcodeAsync(string postcode)
    {
        return await _context
            .LogisticsLocation
            .AsNoTracking()
            .Where(loc => loc.ApprovalStatus != LogisticsLocationApprovalStatus.Rejected)
            .Include(x => x.Address)
            .Where(x => x.Address != null && x.Address.PostCode == postcode)
            .ToListAsync();
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) >= 0;
    }

    public void UpdateLogisticsLocation(LogisticsLocation logisticsLocation)
    {
        _context.LogisticsLocation.Update(logisticsLocation);
    }

    public async Task<IEnumerable<LogisticsLocation>> GetActiveLogisticsLocationsForTradePartyAsync(Guid tradePartyId)
    {
        return await _context.LogisticsLocation
            .AsNoTracking()
            .Where(x => x.TradePartyId == tradePartyId)
            .Where(loc => loc.ApprovalStatus != LogisticsLocationApprovalStatus.Rejected)
            .Where(loc => !loc.IsRemoved)
            .Include(x => x.Address)
            .ToListAsync();
    }

    public async Task<IEnumerable<LogisticsLocation>> GetAllLogisticsLocationsForTradePartyAsync(Guid tradePartyId)
    {
        return await _context.LogisticsLocation
            .AsNoTracking()
            .Where(x => x.TradePartyId == tradePartyId)
            .Include(x => x.Address)
            .ToListAsync();
    }

    public void RemoveLogisticsLocation(LogisticsLocation logisticsLocation)
    {
        _context.LogisticsLocation.Remove(logisticsLocation);
    }

    public async Task<bool> LogisticsLocationAlreadyExists(string name, string addressLineOne, string postcode, Guid? exceptThisLocationId = null)
    {
        var query = _context.LogisticsLocation
            .Where(loc => loc.Name!.ToUpper() == name.ToUpper()
            && loc.Address!.LineOne!.ToUpper() == addressLineOne.ToUpper()
            && loc.Address!.PostCode!.Replace(" ", "").ToUpper() == postcode!.Replace(" ", "").ToUpper()
            && loc.ApprovalStatus != LogisticsLocationApprovalStatus.Rejected
            && !loc.IsRemoved);
        
        if (exceptThisLocationId != null) 
        {
            query = query.Where(loc => loc.Id != exceptThisLocationId);
        }

        return await query.AnyAsync();
    }
}

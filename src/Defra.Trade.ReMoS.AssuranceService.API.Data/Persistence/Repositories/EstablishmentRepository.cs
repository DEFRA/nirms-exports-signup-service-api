using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Context;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Defra.Trade.ReMoS.AssuranceService.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Repositories;

[ExcludeFromCodeCoverage]
public class EstablishmentRepository(ApplicationDbContext context) : IEstablishmentRepository
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

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

    public async Task<IEnumerable<LogisticsLocation>> GetActiveLogisticsLocationsForTradePartyAsync(Guid tradePartyId, string? NI_GBFlag, string? searchTerm, string? sortColumn, string? sortDirection)
    {
        var locations = _context.LogisticsLocation
            .AsNoTracking()
            .Where(x => x.TradePartyId == tradePartyId)
            .Where(loc => loc.ApprovalStatus != LogisticsLocationApprovalStatus.Rejected)
            .Where(loc => !loc.IsRemoved)
            .Include(x => x.Address) as IQueryable<LogisticsLocation>;

        if (!string.IsNullOrEmpty(NI_GBFlag))
            locations = locations.Where(loc => loc.NI_GBFlag == NI_GBFlag);

        if (!string.IsNullOrEmpty(searchTerm))
            locations = locations.Where(loc => loc.Name!.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) || 
                                               loc.RemosEstablishmentSchemeNumber!.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                                               loc.Address!.PostCode!.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase));

        locations = AddSortClause(sortColumn, sortDirection, locations);

        return await locations.ToListAsync();
    }

    public async Task<IEnumerable<LogisticsLocation>> GetAllLogisticsLocationsForTradePartyAsync(Guid tradePartyId, string? NI_GBFlag, string? searchTerm, string? sortColumn, string? sortDirection)
    {
        var locations = _context.LogisticsLocation
            .AsNoTracking()
            .Where(x => x.TradePartyId == tradePartyId)
            .Include(x => x.Address) as IQueryable<LogisticsLocation>;

        if (!string.IsNullOrEmpty(NI_GBFlag))
            locations = locations.Where(loc => loc.NI_GBFlag == NI_GBFlag);

        if (!string.IsNullOrEmpty(searchTerm))
            locations = locations.Where(loc => loc.Name!.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) || 
                                               loc.RemosEstablishmentSchemeNumber!.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) || 
                                               loc.Address!.PostCode!.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase));

        locations = AddSortClause(sortColumn, sortDirection, locations);

        return await locations.ToListAsync();
    }

    public static IQueryable<LogisticsLocation> AddSortClause(string? sortColumn, string? sortDirection, IQueryable<LogisticsLocation> locations)
    {
        if (string.IsNullOrEmpty(sortColumn))
            locations = locations.OrderByDescending(loc => loc.LastModifiedDate);
        else
        {
            Dictionary<string, Expression<Func<LogisticsLocation, object?>>> sortColumns = new()
            {
                { "0", loc => loc.Name },
                { "1", loc => loc.Address!.PostCode },
                { "2", loc => loc.RemosEstablishmentSchemeNumber },
                { "3", loc => loc.ApprovalStatus },
                { "4", loc => loc.LastModifiedDate }
            };

            if (!sortColumns.ContainsKey(sortColumn))
            {
                if (string.IsNullOrEmpty(sortDirection) || sortDirection == "ascending")
                {
                    locations = locations.OrderBy(sortColumns[sortColumn]);
                }
                else if (sortDirection == "descending")
                {
                    locations = locations.OrderByDescending(sortColumns[sortColumn]);
                }
            }
        }
        return locations;
    }

    public void RemoveLogisticsLocation(LogisticsLocation logisticsLocation)
    {
        _context.LogisticsLocation.Remove(logisticsLocation);
    }

    public async Task<bool> LogisticsLocationAlreadyExists(string name, string addressLineOne, string postcode, Guid? exceptThisLocationId = null, Guid? partyId = null)
    {
        var query = _context.LogisticsLocation
            .Where(loc => loc.Address != null 
            && loc.Name!.Equals(name, StringComparison.CurrentCultureIgnoreCase)
            && loc.Address.LineOne!.Equals(addressLineOne, StringComparison.CurrentCultureIgnoreCase)
            && loc.Address.PostCode!.Replace(" ", "").Equals(postcode.Replace(" ", ""), StringComparison.CurrentCultureIgnoreCase)
            && loc.ApprovalStatus != LogisticsLocationApprovalStatus.Rejected
            && loc.ApprovalStatus != LogisticsLocationApprovalStatus.Removed
            && !loc.IsRemoved);

        if (exceptThisLocationId != null)
        {
            query = query.Where(loc => loc.Id != exceptThisLocationId);
        }

        if (partyId != null && partyId != Guid.Empty)
        {
            query = query.Where(loc => loc.TradePartyId == partyId);
        }

        return await query.AnyAsync();
    }
}
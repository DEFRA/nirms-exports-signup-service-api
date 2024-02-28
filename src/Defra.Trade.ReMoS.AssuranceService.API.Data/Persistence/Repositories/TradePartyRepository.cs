using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Context;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Repositories;

[ExcludeFromCodeCoverage]
public class TradePartyRepository : ITradePartyRepository
{
    private readonly ApplicationDbContext _context;

    public TradePartyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TradeParty?> FindTradePartyByIdAsync(Guid id, CancellationToken cancellationToken = default )
    {
        return await _context.TradeParties
                .Include(t => t.TradeAddress)
                .Include(t => t.TradeContact)
                .Include(t => t.AuthorisedSignatory)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<TradeParty>> GetAllTradeParties()
    {
        return await _context.TradeParties
            .AsNoTracking()
               .Include(t => t.TradeAddress)
               .Include(t => t.TradeContact)
               .ToListAsync();
    }

    public async Task AddTradePartyAsync(TradeParty party, CancellationToken cancellationToken = default)
    {
        await _context.TradeParties.AddAsync(party, cancellationToken);
    }

    public TradeParty? UpdateTradeParty(TradeParty party, CancellationToken cancellationToken = default)
    {
       _context.TradeParties.Update(party);
        return party;
    }

    public TradeParty? UpdateTradePartyAddress(TradeParty party, CancellationToken cancellationToken = default)
    {
        if(party.TradeAddress != null)
            _context.TradeAddresses.Update(party.TradeAddress);
        return party;
    }

    public void AddTradePartyAddress(TradeParty party, TradeAddress address, CancellationToken cancellationToken = default)
    {
        party.TradeAddress = address;
    }

    public async Task<TradeParty?> GetTradePartyAsync(Guid tradePartyId)
    {
        return await _context.TradeParties.AsNoTracking()
               .Include(t => t.TradeAddress)
               .Include(t => t.TradeContact)
               .Include(t => t.AuthorisedSignatory)
               .FirstOrDefaultAsync(t => t.Id == tradePartyId).ConfigureAwait(false);
    }

    public async Task<bool> TradePartyExistsAsync(Guid partyId)
    {
        return await _context.TradeParties.AnyAsync(t => t.Id == partyId);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) >= 0;
    }

    public TradeParty UpsertAuthorisedSignatory(TradeParty party, CancellationToken cancellationToken = default)
    {
        if (party.AuthorisedSignatory != null)
        {
            var dbSignatory = _context.AuthorisedSignatory.FirstOrDefault(a => a.Id == party.AuthorisedSignatory.Id);
            if (dbSignatory != null)
            {
                dbSignatory = party.AuthorisedSignatory;
                _context.AuthorisedSignatory.Update(dbSignatory);
                _context.SaveChanges();
            }
            else
            {
                _context.AuthorisedSignatory.Add(party.AuthorisedSignatory);
                _context.SaveChanges();
            }
        }
            
        return party;
    }

    public TradeParty? UpsertTradePartyContact(TradeParty party, CancellationToken cancellationToken = default)
    {
        if (party.TradeContact != null)
        {
            var dbTradeContact = _context.TradeContacts.FirstOrDefault(a => a.Id == party.TradeContact.Id);
            if (dbTradeContact != null)
            {
                dbTradeContact = party.TradeContact;
                _context.TradeContacts.Update(dbTradeContact);
                _context.SaveChanges();
            }
            else
            {
                _context.TradeContacts.Add(party.TradeContact);
                _context.SaveChanges();
            }
        }

        return party;
    }

    public async Task<string> AssignRemosBusinessSchemeNumber(TradeParty party)
    {
        var sqlCommand = "SELECT NEXT VALUE FOR dbo.RemosGbIdHelper";
        var remosNumber = "RMS-GB-";

        if (party.TradeAddress?.TradeCountry == "NI")
        {
            sqlCommand = "SELECT NEXT VALUE FOR dbo.RemosNiIdHelper";
            remosNumber = "RMS-NI-";
        }

        var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText = sqlCommand;
            var sqlResult = await cmd.ExecuteScalarAsync();
            var remosId = (Int64)sqlResult!;
            remosNumber += remosId.ToString().PadLeft(6, '0');
        }

        await connection.CloseAsync();

        return remosNumber;
    }

    public async Task<TradeParty?> GetTradePartyByDefraOrgIdAsync(Guid orgId)
    {
        return await _context.TradeParties.AsNoTracking()
            .Include(t => t.TradeAddress)
            .Include(t => t.TradeContact)
            .Include(t => t.AuthorisedSignatory)
            .Include( t=> t.LogisticsLocations)
            .OrderByDescending(t => t.CreatedDate)
            .FirstOrDefaultAsync(x => x.OrgId == orgId);
    }
}

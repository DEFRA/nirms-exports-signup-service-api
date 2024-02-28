using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces;

public interface IAddressRepository
{
    Task AddAddressAsync(TradeAddress address, CancellationToken cancellationToken = default);
    Task<bool> AddressExistsAsync(Guid tradeAddressId);
    Task<TradeAddress?> GetAddressByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}

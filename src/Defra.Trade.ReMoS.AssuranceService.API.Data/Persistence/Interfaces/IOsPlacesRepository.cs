using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces
{
    public interface IOsPlacesRepository
    {
        Task<OsPlaces> GetOSPlacesLocationsFromPostCodeAsync(string postCode);
    }
}
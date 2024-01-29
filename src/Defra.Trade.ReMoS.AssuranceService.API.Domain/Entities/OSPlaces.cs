using System.Collections.Generic;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities
{
    public class OsPlaces
    {
        public OsPlacesHeader? Header { get; set; }
        public List<OsPlacesResult>? Results { get; set; }
    }
}
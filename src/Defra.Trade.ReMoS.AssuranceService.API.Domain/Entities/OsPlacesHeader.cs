using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities
{
    public class OsPlacesHeader
    {
        public string? uri { get; set; }
        public string? query { get; set; }
        public int offset { get; set; }
        public int totalresults { get; set; }
        public string? format { get; set; }
        public string? dataset { get; set; }
        public string? lr { get; set; }
        public int maxresults { get; set; }
        public string? epoch { get; set; }
        public string? output_srs { get; set; }
        public string? filter { get; set; }
    }
}

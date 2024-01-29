using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class TradePlatform
    {
        public string ServiceBusConnectionString { get; set; } = string.Empty;
        public string ServiceBusName { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class AuthorisedSignatory
    {
        public Guid Id { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Guid TradePartyId { get; set; }
        public string? Name { get; set; }
        public string? EmailAddress { get; set; }
        public string? Position { get; set; }
        [JsonIgnore]
        public DateTime SubmittedDate { get; set; }
        [JsonIgnore]
        public DateTime LastModifiedDate { get; set; }
        [JsonIgnore]
        public Guid ModifiedBy { get; set; }
    }
}

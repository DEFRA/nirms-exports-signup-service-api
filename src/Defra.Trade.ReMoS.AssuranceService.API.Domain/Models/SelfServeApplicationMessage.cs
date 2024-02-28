using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Models
{
    public class SelfServeApplicationMessage
    {
        public TradePartyUpdateData? TradeParty { get; set; }
    }

    public class TradePartyUpdateData
    {
        public Guid Id { get; set; }

        public Guid OrgId { get; set; }

        public Guid SignUpRequestSubmittedBy { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public TradeContact? TradeContact { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AuthorisedSignatory? AuthorisedSignatory { get; set; }
    }
}

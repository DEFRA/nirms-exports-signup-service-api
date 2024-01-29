using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO
{
    public class LogisticsLocationDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public Guid? TradePartyId { get; set; }
        public Guid? TradeAddressId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string? NI_GBFlag { get; set; }

        public TradePartyDto? Party { get; set; }
        public TradeAddressDto? Address { get; set; }
        public string? RemosEstablishmentSchemeNumber { get; set; }
        public LogisticsLocationApprovalStatus ApprovalStatus { get; set; }
        public bool IsRemoved { get; set; }
    }
}

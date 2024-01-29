using Defra.Trade.ReMoS.AssuranceService.API.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;

public class LogisticsLocation
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public Guid TradePartyId { get; set; }
    public Guid? TradeAddressId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public string? NI_GBFlag { get; set; }

    public TradeParty Party { get; set; } = default!;
    public virtual TradeAddress? Address { get; set; } = default!;
    public string? RemosEstablishmentSchemeNumber { get; set; }
    public LogisticsLocationApprovalStatus ApprovalStatus { get; set; }
    public bool IsRemoved { get; set; }
}

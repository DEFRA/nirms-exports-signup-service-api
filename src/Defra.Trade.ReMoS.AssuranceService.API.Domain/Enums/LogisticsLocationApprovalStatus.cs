using System.ComponentModel;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Enums;

public enum LogisticsLocationApprovalStatus
{
    [Description("None")]
    None,
    [Description("Active")]
    Approved,
    [Description("Rejected")]
    Rejected,
    [Description("Draft")]
    Draft,
    [Description("Pending approval")]
    PendingApproval,
    [Description("Suspended")]
    Suspended,
    [Description("Removed")]
    Removed
}

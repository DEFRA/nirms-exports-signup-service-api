using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.Shared.Enums;

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

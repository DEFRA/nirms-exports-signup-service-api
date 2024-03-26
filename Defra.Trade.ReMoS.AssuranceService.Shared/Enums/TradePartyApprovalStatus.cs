using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.Shared.Enums;

public enum TradePartyApprovalStatus
{
    [Description("NOT SIGNED-UP")]
    NotSignedUp,

    [Description("APPROVED FOR NIRMS")]
    Approved,

    [Description("SIGN-UP REJECTED")]
    Rejected,

    [Description("SIGN-UP STARTED")]
    SignupStarted,

    [Description("PENDING APPROVAL")]
    PendingApproval,

    [Description("SUSPENDED")]
    Suspended
}

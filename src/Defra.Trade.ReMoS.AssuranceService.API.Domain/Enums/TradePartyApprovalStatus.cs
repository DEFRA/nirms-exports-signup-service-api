using System.ComponentModel;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Enums;

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

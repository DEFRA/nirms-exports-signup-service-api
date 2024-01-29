using Defra.Trade.ReMoS.AssuranceService.API.Domain.Enums;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;

public class TradeParty
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid OrgId { get; set; }
    public string? PracticeName { get; set; }
    //RENAME TO ROLE CODE
    public string? NatureOfBusiness { get; set; }
    public string? FboNumber { get; set; }
    public string? PhrNumber { get; set; }
    public string? FboPhrOption { get; set; }
    public bool RegulationsConfirmed { get; set; }
    public Guid? TradeAddressId { get; set; }
    public TradeAddress? TradeAddress { get; set; }
    public TradeContact? TradeContact { get; set; }
    public ICollection<LogisticsLocation>? LogisticsLocations { get; set; }
    public AuthorisedSignatory? AuthorisedSignatory { get; set; }
    public string? RemosBusinessSchemeNumber { get; set; } 
    public DateTime AssuranceCommitmentSignedDate { get; set; }
    public DateTime TermsAndConditionsSignedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
    public Guid SignUpRequestSubmittedBy { get; set; }
    public TradePartyApprovalStatus ApprovalStatus { get; set; }
}

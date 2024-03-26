using Defra.Trade.ReMoS.AssuranceService.Shared.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.Shared.DTO;

[ExcludeFromCodeCoverage]
public class TradePartyDto
{
    public Guid Id { get; set; }
    public string? PartyName { get; set; }
    public Guid OrgId { get; set; }
    public string? PracticeName { get; set; }
    public string? CountryName { get; set; }
    public string? NatureOfBusiness { get; set; }
    public string? FboNumber { get; set; }
    public string? PhrNumber { get; set; }
    public string? FboPhrOption { get; set; }
    public bool RegulationsConfirmed { get; set; }
    public string? RemosBusinessSchemeNumber { get; set; }
    public Guid? TradeAddressId { get; set; }
    public DateTime AssuranceCommitmentsSignedDate { get; set; }
    public DateTime TermsAndConditionsSignedDate { get; set; }
    public Guid SignUpRequestSubmittedBy { get; set; }
    public TradePartyApprovalStatus ApprovalStatus { get; set; }
    public TradeAddressDto? Address { get; set; }
    public TradeContactDto? Contact { get; set; }
    public AuthorisedSignatoryDto? AuthorisedSignatory { get; set; }

}

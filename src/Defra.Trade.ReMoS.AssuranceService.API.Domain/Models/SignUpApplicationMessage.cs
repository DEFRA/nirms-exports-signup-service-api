using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Models;

[ExcludeFromCodeCoverage]
public record SignUpApplicationMessage
{
    public TradePartyData? TradeParty { get; init; }
}

[ExcludeFromCodeCoverage]
public record TradePartyData
{
    public Guid Id { get; init; }
    public Guid OrgId { get; init; }
    public string? FboNumber { get; init; }
    public string? PhrNumber { get; init; }
    public string? CountryName { get; init; }
    public string? RemosBusinessSchemeNumber { get; init; }
    public DateTime TermsAndConditionsSignedDate { get; init; }
    public Guid SignUpRequestSubmittedBy { get; init; }
    public TradeContact? TradeContact { get; init; }
    public AuthorisedSignatory? AuthorisedSignatory { get; init; }
    public ICollection<LogisticsLocationData>? LogisticsLocations { get; init; }
}

public record LogisticsLocationData
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? EmailAddress { get; init; }
    public Guid TradePartyId {  get; init; }
    public string? RemosEstablishmentSchemeNumber { get; init; }
    public AddressData? Address { get; init; }
}

public record AddressData
{
    public string? LineOne { get; init; }
    public string ? LineTwo { get; init; }
    public string? PostCode { get; init; }
    public string? CityName { get; init; }
    public string? County {  get; init; }
}

public record LogisticsLocationDataForUpdate
{
    public Guid Id { get; init; }
    public Guid TradePartyId { get; init; }
    public Guid InspectionLocationId { get; set; }
    public string? Status { get; set; }
}

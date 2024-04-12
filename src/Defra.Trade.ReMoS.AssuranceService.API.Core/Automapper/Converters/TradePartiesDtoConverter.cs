using AutoMapper;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Defra.Trade.ReMoS.AssuranceService.Shared.DTO;
using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Converters;

[ExcludeFromCodeCoverage]
public class TradePartiesDtoConverter : ITypeConverter<TradeParty, TradePartyDto>
{
    public TradePartyDto Convert(TradeParty source, TradePartyDto dest, ResolutionContext context)
    {
        TradePartyDto tradePartyDto = new()
        {
            Id = source.Id,
            PartyName = source.Name ?? null,
            OrgId = source.OrgId,
            PracticeName = source.PracticeName ?? null,
            NatureOfBusiness = source.NatureOfBusiness ?? null,
            FboNumber = source.FboNumber ?? null,
            PhrNumber = source.PhrNumber ?? null,
            FboPhrOption = source.FboPhrOption ?? null,
            RegulationsConfirmed = source.RegulationsConfirmed,
            RemosBusinessSchemeNumber = source.RemosBusinessSchemeNumber ?? null,
            AssuranceCommitmentsSignedDate = source.AssuranceCommitmentSignedDate,
            TermsAndConditionsSignedDate = source.TermsAndConditionsSignedDate,
            SignUpRequestSubmittedBy = source.SignUpRequestSubmittedBy,
            ApprovalStatus = source.ApprovalStatus,
        };

        if (source.TradeAddress != null)
        {
            tradePartyDto.Address ??= new TradeAddressDto();

            if (source.TradeAddress.Id != Guid.Empty)
            {
                tradePartyDto.Address.Id = source.TradeAddress.Id;
            }

            tradePartyDto.TradeAddressId = source.TradeAddressId ?? null;
            tradePartyDto.Address.LineOne = source.TradeAddress.LineOne ?? null;
            tradePartyDto.Address.LineTwo = source.TradeAddress.LineTwo ?? null;
            tradePartyDto.Address.LineThree = source.TradeAddress.LineThree ?? null;
            tradePartyDto.Address.LineFour = source.TradeAddress.LineFour ?? null;
            tradePartyDto.Address.LineFive = source.TradeAddress.LineFive ?? null;
            tradePartyDto.Address.PostCode = source.TradeAddress.PostCode ?? null;
            tradePartyDto.Address.CityName = source.TradeAddress.CityName ?? null;
            tradePartyDto.Address.TradeCountry = source.TradeAddress.TradeCountry ?? null;
        }

        if (source.TradeContact != null)
        {
            tradePartyDto.Contact ??= new TradeContactDto();

            if (source.TradeContact.Id != Guid.Empty)
            {
                tradePartyDto.Contact.Id = source.TradeContact.Id;
            }

            tradePartyDto.Contact.PersonName = source.TradeContact.PersonName ?? null;
            tradePartyDto.Contact.TelephoneNumber = source.TradeContact.TelephoneNumber ?? null;
            tradePartyDto.Contact.Email = source.TradeContact.Email ?? null;
            tradePartyDto.Contact.Position = source.TradeContact.Position ?? null;
            tradePartyDto.Contact.IsAuthorisedSignatory = source.TradeContact.IsAuthorisedSignatory ?? null;
            tradePartyDto.Contact.SubmittedDate = source.TradeContact.SubmittedDate;
            tradePartyDto.Contact.LastModifiedDate = source.TradeContact.LastModifiedDate;
            tradePartyDto.Contact.ModifiedBy = source.TradeContact.ModifiedBy;
        }

        if (source.AuthorisedSignatory != null) 
        {
            tradePartyDto.AuthorisedSignatory ??= new AuthorisedSignatoryDto();

            if (source.AuthorisedSignatory.Id != Guid.Empty) 
            {
                tradePartyDto.AuthorisedSignatory.Id = source.AuthorisedSignatory.Id;
            }

            tradePartyDto.AuthorisedSignatory.Name = source.AuthorisedSignatory.Name ?? null;
            tradePartyDto.AuthorisedSignatory.EmailAddress = source.AuthorisedSignatory.EmailAddress ?? null;            
            tradePartyDto.AuthorisedSignatory.Position = source.AuthorisedSignatory.Position ?? null;
            tradePartyDto.AuthorisedSignatory.SubmittedDate = source.AuthorisedSignatory.SubmittedDate;
            tradePartyDto.AuthorisedSignatory.LastModifiedDate = source.AuthorisedSignatory.LastModifiedDate;
            tradePartyDto.AuthorisedSignatory.ModifiedBy = source.AuthorisedSignatory.ModifiedBy;
        }
        return tradePartyDto;
    }
}

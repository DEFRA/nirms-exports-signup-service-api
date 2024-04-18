using AutoMapper;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Converters;

[ExcludeFromCodeCoverage]
public class TradePartiesConverter : ITypeConverter<TradePartyDto, TradeParty>
{
    public TradeParty Convert(TradePartyDto source, TradeParty dest, ResolutionContext context)
    {
        TradeParty tradeParty = new();
        if (dest != null)
        {
            tradeParty = dest;
        }

        tradeParty.Id = AssignValues(tradeParty.Id, source.Id);
        tradeParty.Name = AssignValues(tradeParty.Name, source.PartyName);
        tradeParty.OrgId = AssignValues(tradeParty.OrgId, source.OrgId);
        tradeParty.PracticeName = AssignValues(tradeParty.PracticeName, source.PracticeName);
        tradeParty.NatureOfBusiness = AssignValues(tradeParty.NatureOfBusiness, source.NatureOfBusiness);
        tradeParty.FboNumber = AssignValues(tradeParty.FboNumber, source.FboNumber);
        tradeParty.PhrNumber = AssignValues(tradeParty.PhrNumber, source.PhrNumber);
        tradeParty.FboPhrOption = AssignValues(tradeParty.FboPhrOption, source.FboPhrOption);
        tradeParty.RegulationsConfirmed = AssignValues(tradeParty.RegulationsConfirmed, source.RegulationsConfirmed);
        tradeParty.RemosBusinessSchemeNumber = AssignValues(tradeParty.RemosBusinessSchemeNumber, source.RemosBusinessSchemeNumber);
        tradeParty.AssuranceCommitmentSignedDate = AssignValues(tradeParty.AssuranceCommitmentSignedDate, source.AssuranceCommitmentsSignedDate);
        tradeParty.TermsAndConditionsSignedDate = AssignValues(tradeParty.TermsAndConditionsSignedDate, source.TermsAndConditionsSignedDate);
        tradeParty.SignUpRequestSubmittedBy = tradeParty.SignUpRequestSubmittedBy != Guid.Empty ? tradeParty.SignUpRequestSubmittedBy : source.SignUpRequestSubmittedBy;
        tradeParty.ApprovalStatus = AssignValues(tradeParty.ApprovalStatus, source.ApprovalStatus);

        if (source.Address != null)
        {
            tradeParty.TradeAddress ??= new TradeAddress();
            tradeParty.TradeAddress.Id = AssignValues(tradeParty.TradeAddress.Id, source.Address.Id);
            tradeParty.TradeAddressId = tradeParty.TradeAddress.Id;
            tradeParty.TradeAddress.LineOne = AssignValues(tradeParty.TradeAddress.LineOne, source.Address.LineOne);
            tradeParty.TradeAddress.LineTwo = AssignValues(tradeParty.TradeAddress.LineTwo, source.Address.LineTwo);
            tradeParty.TradeAddress.LineThree = AssignValues(tradeParty.TradeAddress.LineThree, source.Address.LineThree);
            tradeParty.TradeAddress.LineFour = AssignValues(tradeParty.TradeAddress.LineFour, source.Address.LineFour);
            tradeParty.TradeAddress.LineFive = AssignValues(tradeParty.TradeAddress.LineFive, source.Address.LineFive);
            tradeParty.TradeAddress.PostCode = AssignValues(tradeParty.TradeAddress.PostCode, source.Address.PostCode);
            tradeParty.TradeAddress.CityName = AssignValues(tradeParty.TradeAddress.CityName, source.Address.CityName);
            tradeParty.TradeAddress.TradeCountry = AssignValues(tradeParty.TradeAddress.TradeCountry, source.Address.TradeCountry);
        }

        if (source.Contact != null)
        {
            tradeParty.TradeContact ??= new TradeContact();
            tradeParty.TradeContact.Id = AssignValues(tradeParty.TradeContact.Id, source.Contact.Id);
            tradeParty.TradeContact.TradePartyId = source.Id;
            tradeParty.TradeContact.PersonName = AssignValues(tradeParty.TradeContact.PersonName, source.Contact.PersonName);
            tradeParty.TradeContact.TelephoneNumber = AssignValues(tradeParty.TradeContact.TelephoneNumber, source.Contact.TelephoneNumber);
            tradeParty.TradeContact.Email = AssignValues(tradeParty.TradeContact.Email, source.Contact.Email);
            tradeParty.TradeContact.Position = AssignValues(tradeParty.TradeContact.Position, source.Contact.Position);
            tradeParty.TradeContact.IsAuthorisedSignatory =  source.Contact.IsAuthorisedSignatory;
            tradeParty.TradeContact.SubmittedDate = AssignValues(tradeParty.TradeContact.SubmittedDate, source.Contact.SubmittedDate);
            tradeParty.TradeContact.LastModifiedDate = AssignValues(tradeParty.TradeContact.LastModifiedDate, source.Contact.LastModifiedDate);
            tradeParty.TradeContact.ModifiedBy = AssignValues(tradeParty.TradeContact.ModifiedBy, source.Contact.ModifiedBy);
        }

        if (source.AuthorisedSignatory != null)
        {
            tradeParty.AuthorisedSignatory ??= new AuthorisedSignatory();
            if (source.AuthorisedSignatory.Id != Guid.Empty)
            {
                tradeParty.AuthorisedSignatory.Id = source.AuthorisedSignatory.Id;
            }

            if (source.AuthorisedSignatory.Id != Guid.Empty && source.AuthorisedSignatory.Name == null)
            {
                tradeParty.AuthorisedSignatory.Name = null;
                tradeParty.AuthorisedSignatory.EmailAddress = null;
                tradeParty.AuthorisedSignatory.Position = null;
            }

            else
            {
                tradeParty.AuthorisedSignatory.TradePartyId = source.Id;
                tradeParty.AuthorisedSignatory.Name = AssignValues(tradeParty.AuthorisedSignatory.Name, source.AuthorisedSignatory.Name);
                tradeParty.AuthorisedSignatory.EmailAddress = AssignValues(tradeParty.AuthorisedSignatory.EmailAddress, source.AuthorisedSignatory.EmailAddress);
                tradeParty.AuthorisedSignatory.Position = AssignValues(tradeParty.AuthorisedSignatory.Position, source.AuthorisedSignatory.Position);
                tradeParty.AuthorisedSignatory.SubmittedDate = AssignValues(tradeParty.AuthorisedSignatory.SubmittedDate, source.AuthorisedSignatory.SubmittedDate);
                tradeParty.AuthorisedSignatory.LastModifiedDate = AssignValues(tradeParty.AuthorisedSignatory.LastModifiedDate, source.AuthorisedSignatory.LastModifiedDate);
                tradeParty.AuthorisedSignatory.ModifiedBy = AssignValues(tradeParty.AuthorisedSignatory.ModifiedBy, source.AuthorisedSignatory.ModifiedBy);
            }

        }
        return tradeParty;
    }

    public static string? AssignValues (string? oldString, string? newString)
    {
        if (newString != null)
        {
            return newString;
        }
        if (oldString != null)
        {
            return oldString;
        }        
        return null;
    }

    public static bool AssignValues(bool oldVal, bool newVal)
    {
        if (newVal)
        {
            return newVal;
        }
        return oldVal;
    }

    public static DateTime AssignValues(DateTime oldDate, DateTime newDate)
    {
        if (newDate != DateTime.MinValue)
        {
            return newDate;
        }
        if (oldDate != DateTime.MinValue)
        {
            return oldDate;
        }
        return DateTime.MinValue;
    }

    public static Guid AssignValues(Guid oldGuid, Guid newGuid)
    {
        if (oldGuid != Guid.Empty && newGuid != Guid.Empty)
            return newGuid;
        if (oldGuid != Guid.Empty && newGuid == Guid.Empty)
            return oldGuid;
        if (oldGuid == Guid.Empty && newGuid != Guid.Empty)
            return newGuid;
        return Guid.NewGuid();
    }

    public static T? AssignValues<T>(T oldVal, T newVal)
    {
        if (newVal != null)
        {
            return newVal;
        }
        if (oldVal != null)
        {
            return oldVal;
        }
        return default;
    }
}
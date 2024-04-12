using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Models;
using Defra.Trade.ReMoS.AssuranceService.Shared.Enums;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.UnitTests.Models;

[TestFixture]
public class ModelTests
{
    [Test]
    public void SetSelfServeUpdateEstablishmentMessage_GivenValidValues_FieldsSetToGivenValues()
    {
        //Arrange
        var tradePartyId = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d429");
        var orgId = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d531");
        var locationId = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d693");
        var inspectionLocationId = Guid.NewGuid();

        //Act
        var message = new SelfServeUpdateEstablishmentMessage
        {
            TradeParty = new TradePartyWithLogicsLocationUpdateData()
            {
                Id = tradePartyId,
                OrgId = orgId,
                LogisticsLocation = new LogisticsLocationDataForUpdate()
                {
                    Id = locationId,
                    TradePartyId = tradePartyId,
                    Status = LogisticsLocationApprovalStatus.Removed.ToString(),
                    InspectionLocationId = inspectionLocationId,
                }
            }
        };


        //Assert
        message.TradeParty.Id.Should().Be(tradePartyId);
        message.TradeParty.OrgId.Should().Be(orgId);
        message.TradeParty.LogisticsLocation.Id.Should().Be(locationId);
        message.TradeParty.LogisticsLocation.TradePartyId.Should().Be(tradePartyId);
        message.TradeParty.LogisticsLocation.Status.Should().Be("Removed");
        message.TradeParty.LogisticsLocation.InspectionLocationId.Should().Be(inspectionLocationId);

    }

    [Test]
    public void SetSelfServeAddEstablishmentMessage_GivenValidValues_FieldsSetToGivenValues()
    {
        //Arrange
        var tradePartyId = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d429");
        var orgId = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d531");
        var locationId = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d693");
        var rmsNumber = "12AB";
        var name = "Test Name";
        var email = "sd@sd.com";
        var tradeAddress = new AddressData
        {
            //Act
            LineOne = "Line 1",
            LineTwo = "Line 2",
            PostCode = "NE36 0PQ",
            CityName = "London"
        };

        //Act
        var message = new SelfServeAddEstablishmentMessage
        {
            TradeParty = new TradePartyWithLogicsLocationData()
            {
                Id = tradePartyId,
                OrgId = orgId,
                LogisticsLocation = new LogisticsLocationData()
                {
                    Id = locationId,
                    Name = name,
                    EmailAddress = email,
                    RemosEstablishmentSchemeNumber = rmsNumber,
                    TradePartyId = tradePartyId,
                    Address = tradeAddress,
                }
            }
        };


        //Assert
        message.TradeParty.Id.Should().Be(tradePartyId);
        message.TradeParty.OrgId.Should().Be(orgId);
        message.TradeParty.LogisticsLocation.Id.Should().Be(locationId);
        message.TradeParty.LogisticsLocation.TradePartyId.Should().Be(tradePartyId);
        message.TradeParty.LogisticsLocation.EmailAddress.Should().Be(email);
        message.TradeParty.LogisticsLocation.RemosEstablishmentSchemeNumber.Should().Be(rmsNumber);
        message.TradeParty.LogisticsLocation.TradePartyId.Should().Be(tradePartyId);
        message.TradeParty.LogisticsLocation.Address.Should().Be(tradeAddress);

    }

    [Test]
    public void SignUpApplicationMessage_SetToValidValues()
    {
        // arrange
        var tradePartId = Guid.NewGuid();
        var orgId = Guid.NewGuid();
        var fboNumber = "TestFBO1234";
        var phrNumber = "Testphr1234";
        var countryName = "Test country";
        var rmsNumber = "12AB";
        var tcSIgnDate = DateTime.UtcNow;
        var susSUbmittedBy = Guid.NewGuid();
        
        var tradeContact = new TradeContact 
        {
            Id = Guid.NewGuid(),
            TradePartyId = tradePartId,
            Email = "sd~sd.com",
            IsAuthorisedSignatory = false,
            LastModifiedDate = DateTime.UtcNow,
            ModifiedBy = susSUbmittedBy,
            PersonName = "Test person",
            Position = "Sales rep",
            SubmittedDate = DateTime.UtcNow,
            TelephoneNumber = "0118 123 1234",
        };

        var authSignatory = new AuthorisedSignatory 
        {
            Id = Guid.NewGuid(),
            TradePartyId = tradePartId,
            EmailAddress = "sd~sd.com",
            LastModifiedDate = DateTime.UtcNow,
            ModifiedBy = susSUbmittedBy,
            Name = "Test person",
            Position = "Sales rep",
            SubmittedDate = DateTime.UtcNow,
        };

        ICollection<LogisticsLocationData>? loctions = new List<LogisticsLocationData> 
        {
            new LogisticsLocationData {Id = Guid.NewGuid(), TradePartyId = tradePartId, EmailAddress = "loc@loc.com", Name = "Test loc", RemosEstablishmentSchemeNumber = rmsNumber},
        };

        // act
        var message = new SignUpApplicationMessage
        {
            TradeParty = new TradePartyData
            {
                Id = tradePartId,
                OrgId = orgId,
                FboNumber = fboNumber,
                CountryName = countryName,
                PhrNumber = phrNumber,
                RemosBusinessSchemeNumber = rmsNumber,
                TermsAndConditionsSignedDate = tcSIgnDate,
                SignUpRequestSubmittedBy = susSUbmittedBy,
                TradeContact = tradeContact,
                AuthorisedSignatory = authSignatory,
                LogisticsLocations = loctions,

            }
        };

        // assert
        message.TradeParty.Id.Should().Be(tradePartId);
        message.TradeParty.OrgId.Should().Be(orgId);
        message.TradeParty.FboNumber.Should().Be(fboNumber);
        message.TradeParty.PhrNumber.Should().Be(phrNumber);
        message.TradeParty.CountryName.Should().Be(countryName);
        message.TradeParty.RemosBusinessSchemeNumber.Should().Be(rmsNumber);
        message.TradeParty.TermsAndConditionsSignedDate.Should().Be(tcSIgnDate);
        message.TradeParty.SignUpRequestSubmittedBy.Should().Be(susSUbmittedBy);

    }
}

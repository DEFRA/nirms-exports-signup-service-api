using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.UnitTests.Entities;

[TestFixture]
public class TradePartyTests
{
    [Test]
    public void SetTradeParty_GivenValidValues_FieldsSetToGivenValues()
    {
        //Arrange
        var testDate = new DateTime(2023, 1, 1, 0, 0, 0);
        var tradeParty = new TradeParty
        {
            Id = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"),
            Name = "Trade party Ltd",
            OrgId = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"),
            PracticeName = "test practice",
            NatureOfBusiness = "Wholesale Hamster Supplies",
            FboNumber = "fbonum-123456-fbonum",
            PhrNumber = "phr123",
            FboPhrOption = "none",
            RegulationsConfirmed = false,
            TradeAddressId = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d532"),
            CreatedDate = testDate,
            LastUpdateDate = testDate,
            AssuranceCommitmentSignedDate = testDate,
            TermsAndConditionsSignedDate = testDate
        };

        var tradeAddress = new TradeAddress
        {
            Id = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"),
            TradeCountry = "United Kingdom",
            LineOne = "Line 1",
            LineTwo = "Line 2",
            LineThree = "Line 3",
            LineFour = "Line 4",
            LineFive = "Line 5",
            PostCode = "NE36 0PQ",
            CityName = "London"
        };

        var tradeContact = new TradeContact
        {
            Id = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"),
            PersonName = "Joe Blogs",
            TelephoneNumber = "01414 523 333",
            Email = "a@a.com",
            Position = "a",
        };

        var locations = new List<LogisticsLocation>
        {
            new LogisticsLocation
            {
                Id = Guid.NewGuid(),
                TradePartyId = tradeParty.Id,
            }
        };

        //Act
        tradeParty.Id = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188");
        tradeParty.Name = "Trade party Ltd";
        tradeParty.OrgId = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188");
        tradeParty.PracticeName = "test practice";
        tradeParty.NatureOfBusiness = "Wholesale Hamster Supplies";
        tradeParty.FboNumber = "fbonum-123456-fbonum";
        tradeParty.RegulationsConfirmed = true;
        tradeParty.TradeAddressId = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d532");
        tradeParty.RemosBusinessSchemeNumber = "RMS-GB-000000";


        tradeParty.TradeAddress = tradeAddress;
        tradeParty.TradeContact = tradeContact;
        tradeParty.LogisticsLocations = locations;

        //Assert
        tradeParty.Id.Should().Be(Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"));
        tradeParty.Name.Should().Be("Trade party Ltd");
        tradeParty.OrgId.Should().Be(Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"));
        tradeParty.PracticeName.Should().Be("test practice");
        tradeParty.NatureOfBusiness.Should().Be("Wholesale Hamster Supplies");
        tradeParty.FboNumber.Should().Be("fbonum-123456-fbonum");
        tradeParty.PhrNumber.Should().Be("phr123");
        tradeParty.FboPhrOption.Should().Be("none");
        tradeParty.RegulationsConfirmed.Should().Be(true);
        tradeParty.TradeAddressId.Should().Be("c16eb7a7-2949-4880-b5d7-0405f4f7d532");
        tradeParty.RemosBusinessSchemeNumber.Should().Be("RMS-GB-000000");
        tradeParty.CreatedDate.Should().Be(testDate);
        tradeParty.LastUpdateDate.Should().Be(testDate);
        tradeParty.AssuranceCommitmentSignedDate.Should().Be(testDate);
        tradeParty.TermsAndConditionsSignedDate.Should().Be(testDate);
        tradeParty.TradeContact.Should().Be(tradeContact);
        tradeParty.TradeAddress.Should().Be(tradeAddress);
        tradeParty.LogisticsLocations.Should().Equal(locations);
    }
}

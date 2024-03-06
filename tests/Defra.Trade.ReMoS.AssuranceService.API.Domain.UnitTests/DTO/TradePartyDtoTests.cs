using Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.UnitTests.DTO;

[TestFixture]
public class TradePartyDtoTests
{
    [Test]
    public void SetTradeParty_GivenValidValues_FieldsSetToGivenValues()
    {
        //Arrange
        var tradeParty = new TradePartyDto();

        var tradeAddress = new TradeAddressDto
        {
            //Act
            Id = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"),
            LineOne = "Line 1",
            LineTwo = "Line 2",
            LineThree = "Line 3",
            LineFour = "Line 4",
            LineFive = "Line 5",
            PostCode = "NE36 0PQ",
            CityName = "London"
        };

        var tradeContact = new TradeContactDto
        {
            Id = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"),
            PersonName = "Joe Blogs",
            TelephoneNumber = "01414 523 333",
            Email = "a@a.com",
            Position = "a",
        };

        DateTime testdate = new(2023, 1, 1, 0, 0, 0);

        //Act
        tradeParty.Id = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188");
        tradeParty.PartyName = "Trade party Ltd";
        tradeParty.OrgId = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188");
        tradeParty.PracticeName = "test practice";
        tradeParty.CountryName = "United Kingdom";
        tradeParty.NatureOfBusiness = "Wholesale Hamster Supplies";
        tradeParty.FboNumber = "fbonum-123456-fbonum";
        tradeParty.PhrNumber = "phr123";
        tradeParty.FboPhrOption = "fbo";
        tradeParty.RegulationsConfirmed = true;
        tradeParty.AssuranceCommitmentsSignedDate = testdate;
        tradeParty.TermsAndConditionsSignedDate = testdate;
        tradeParty.Address = tradeAddress;
        tradeParty.TradeAddressId = tradeAddress.Id;
        tradeParty.Contact = tradeContact;
        tradeParty.ApprovalStatus = Enums.TradePartyApprovalStatus.NotSignedUp;

        //Assert
        tradeParty.Id.Should().Be(Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"));
        tradeParty.PartyName.Should().Be("Trade party Ltd");
        tradeParty.OrgId.Should().Be(Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"));
        tradeParty.PracticeName.Should().Be("test practice");
        tradeParty.CountryName.Should().Be("United Kingdom");
        tradeParty.NatureOfBusiness.Should().Be("Wholesale Hamster Supplies");
        tradeParty.FboNumber.Should().Be("fbonum-123456-fbonum");
        tradeParty.PhrNumber.Should().Be("phr123");
        tradeParty.FboPhrOption.Should().Be("fbo");
        tradeParty.RegulationsConfirmed.Should().BeTrue();
        tradeParty.AssuranceCommitmentsSignedDate.Should().Be(testdate);
        tradeParty.TermsAndConditionsSignedDate.Should().Be(testdate);
        tradeParty.Contact.Should().Be(tradeContact);
        tradeParty.Address.Should().Be(tradeAddress);
        tradeParty.TradeAddressId.Should().Be(tradeAddress.Id);
        tradeParty.ApprovalStatus.Should().Be(Enums.TradePartyApprovalStatus.NotSignedUp);
    }
}

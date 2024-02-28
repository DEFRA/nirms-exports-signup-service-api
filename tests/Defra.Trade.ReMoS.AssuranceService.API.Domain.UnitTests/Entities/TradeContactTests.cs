using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.UnitTests.Entities;

[TestFixture]
public class TradeContactTests
{
    [Test]
    public void SetContact_GivenValidValues_FieldsSetToGivenValues()
    {
        //Arrange
        var tradeContact = new TradeContact();
        DateTime testdate = new(2023, 1, 1, 0, 0, 0);

        //Act
        tradeContact.Id = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188");
        tradeContact.ModifiedBy = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188");
        tradeContact.TradePartyId = Guid.Parse("c16eb7a7-2949-3495-b5d7-0405f4f7d904");
        tradeContact.PersonName = "Joe Blogs";
        tradeContact.TelephoneNumber = "01414 523 333";
        tradeContact.Email = "a@a.com";
        tradeContact.Position = "a";
        tradeContact.LastModifiedDate = testdate;
        tradeContact.SubmittedDate = testdate;

        //Assert
        tradeContact.Id.Should().Be(Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"));
        tradeContact.ModifiedBy.Should().Be(Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"));
        tradeContact.TradePartyId.Should().Be(Guid.Parse("c16eb7a7-2949-3495-b5d7-0405f4f7d904"));
        tradeContact.PersonName.Should().Be("Joe Blogs");
        tradeContact.TelephoneNumber.Should().Be("01414 523 333");
        tradeContact.Email.Should().Be("a@a.com");
        tradeContact.Position.Should().Be("a");
        tradeContact.LastModifiedDate.Should().Be(testdate);
        tradeContact.SubmittedDate.Should().Be(testdate);
    }
}

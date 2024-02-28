using Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.UnitTests.DTO;

[TestFixture]
public class TradeContactDtoTests
{
    [Test]
    public void SetContact_GivenValidValues_FieldsSetToGivenValues()
    {
        //Arrange & Act
        DateTime testdate = new(2023, 1, 1, 0, 0, 0);
        var tradeContact = new TradeContactDto
        {
            Id = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"),
            PersonName = "Joe Blogs",
            TelephoneNumber = "01414 523 333",
            Email = "a@a.com",
            Position = "a",
            LastModifiedDate = testdate,
            SubmittedDate = testdate,
            ModifiedBy = Guid.Parse("b5094c53-2478-4263-b262-af6e052a94d2")
        };

        //Assert
        tradeContact.Id.Should().Be(Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"));
        tradeContact.PersonName.Should().Be("Joe Blogs");
        tradeContact.TelephoneNumber.Should().Be("01414 523 333");
        tradeContact.Email.Should().Be("a@a.com");
        tradeContact.Position.Should().Be("a");
        tradeContact.LastModifiedDate.Should().Be(testdate);
        tradeContact.SubmittedDate.Should().Be(testdate);
        tradeContact.ModifiedBy.Should().Be(Guid.Parse("b5094c53-2478-4263-b262-af6e052a94d2"));
    }
}

using Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.UnitTests.DTO;

[TestFixture]
public class TradeAddressDtoTests
{
    [Test]
    public void SetAddress_GivenValidValues_FieldsSetToGivenValues()
    {
        //Arrange
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
            CityName = "London",
            County = "Surrey",
            TradeCountry = "England"
        };

        //Assert
        tradeAddress.Id.Should().Be(Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"));
        tradeAddress.LineOne.Should().Be("Line 1");
        tradeAddress.LineTwo.Should().Be("Line 2");
        tradeAddress.LineThree.Should().Be("Line 3");
        tradeAddress.LineFour.Should().Be("Line 4");
        tradeAddress.LineFive.Should().Be("Line 5");
        tradeAddress.PostCode.Should().Be("NE36 0PQ");
        tradeAddress.CityName.Should().Be("London");
        tradeAddress.County.Should().Be("Surrey");
        tradeAddress.TradeCountry.Should().Be("England");
    }
}

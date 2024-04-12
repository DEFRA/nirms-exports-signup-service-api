using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.UnitTests.Entities;

[TestFixture]
public class LogisticsLocationTests
{
    [Test]
    public void SetLogisticsLocation_GivenValidValues_FieldsSetToGivenValues()
    {
        //Arrange
        var logisticsLocation = new LogisticsLocation();
        var date = DateTime.Now;

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

        var tradeParty = new TradeParty
        {
            Id = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"),
            FboNumber = "1234",
            Name = "abc",
        };

        //Act
        logisticsLocation.Id = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188");
        logisticsLocation.Name = "Trade party Ltd";
        logisticsLocation.Email = "aa@aa.com";
        logisticsLocation.CreatedDate = date;
        logisticsLocation.LastModifiedDate = date;
        logisticsLocation.NI_GBFlag = "Northern Ireland";
        logisticsLocation.TradeAddressId = tradeAddress.Id;
        logisticsLocation.Address = tradeAddress;
        logisticsLocation.Party = tradeParty;

        //Assert
        logisticsLocation.Id.Should().Be(Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d188"));
        logisticsLocation.Name.Should().Be("Trade party Ltd");
        logisticsLocation.Email.Should().Be("aa@aa.com");
        logisticsLocation.CreatedDate.Should().Be(date);
        logisticsLocation.LastModifiedDate.Should().Be(date);
        logisticsLocation.NI_GBFlag.Should().Be("Northern Ireland");
        logisticsLocation.TradeAddressId.Should().Be(tradeAddress.Id);
        logisticsLocation.Address.Should().Be(tradeAddress);
        logisticsLocation.Party.Should().Be(tradeParty);
    }
}

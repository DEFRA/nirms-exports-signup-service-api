using Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.UnitTests.DTO;

[TestFixture]
public class LogisticsLocationDtoTests
{
    [Test]
    public void SetLogisticsLocationDTO_GivenValidValues_FieldsSetToGivenValues()
    {
        //Arrange
        var logisticsLocationDto = new LogisticsLocationDto();
        var tradePartyId = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d429");
        var locationId = Guid.Parse("c16eb7a7-2949-4880-b5d7-0405f4f7d693");
        var date = DateTime.Now;

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



        //Act
        logisticsLocationDto.TradeAddressId = tradeAddress.Id;
        logisticsLocationDto.TradePartyId = tradePartyId;
        logisticsLocationDto.Id = locationId;
        logisticsLocationDto.Name = "Lorem Ipsum Lane";
        logisticsLocationDto.CreatedDate = date;
        logisticsLocationDto.LastModifiedDate = date;
        logisticsLocationDto.Email = "contact@test.com";
        logisticsLocationDto.Address = tradeAddress;
        logisticsLocationDto.NI_GBFlag = "Northern Ireland";


        //Assert
        logisticsLocationDto.TradeAddressId.Should().Be(tradeAddress.Id);
        logisticsLocationDto.TradePartyId.Should().Be(tradePartyId);
        logisticsLocationDto.Id.Should().Be(locationId);
        logisticsLocationDto.Name.Should().Be("Lorem Ipsum Lane");
        logisticsLocationDto.CreatedDate.Should().Be(date);
        logisticsLocationDto.LastModifiedDate.Should().Be(date);
        logisticsLocationDto.Email.Should().Be("contact@test.com");
        logisticsLocationDto.Address.Should().Be(tradeAddress);
        logisticsLocationDto.NI_GBFlag.Should().Be("Northern Ireland");

    }
}

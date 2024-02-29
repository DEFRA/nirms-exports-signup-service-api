using Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Enums;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Models;

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
}

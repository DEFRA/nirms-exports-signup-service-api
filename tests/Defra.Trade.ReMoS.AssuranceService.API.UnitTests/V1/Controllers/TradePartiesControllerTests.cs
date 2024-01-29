using Defra.Trade.ReMoS.AssuranceService.API.Core.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Models;
using Defra.Trade.ReMoS.AssuranceService.API.V1.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.UnitTests.V1.Controllers;

public class TradePartiesControllerTests
{

    private TradePartiesController? systemUnderTest;
    readonly Mock<ITradePartiesService> _mockTradePartiesService = new();

    [SetUp]
    public void Setup()
    {
        systemUnderTest = new TradePartiesController(_mockTradePartiesService.Object);
    }

    [Test]
    public async Task GetTradePartiesAsync_ShouldReturnParties()
    {
        // arrange
        var tradeParties = GenerateTradeParties();
        var expected = systemUnderTest?.Ok(tradeParties);
        _mockTradePartiesService.Setup(m => m.GetTradePartiesAsync().Result).Returns(tradeParties);

        // act
        var result = await systemUnderTest!.GetTradePartiesAsync();

        // assert
        result.Should().BeEquivalentTo(expected);
    }


    [Test]
    public async Task GetTradePartiesAsync_ShouldReturnBadRequest()
    {
        // arrange        
        _mockTradePartiesService.Setup(m => m.GetTradePartiesAsync())
        .ThrowsAsync(new Exception());


        // act
        var result = await systemUnderTest!.GetTradePartiesAsync();

        // assert
        result.Should().BeEquivalentTo(systemUnderTest.BadRequest());
    }

    [Test]
    public async Task GetTradeParty_ShouldReturnTradePartyDto_IfTradePartyExists()
    {
        //Arrange
        var tradePartyId = Guid.NewGuid();
        var tradePartyDTO = new TradePartyDto { Id = tradePartyId, PartyName = "partyname" };
        _mockTradePartiesService
            .Setup(action => action.GetTradePartyAsync(tradePartyId))
            .ReturnsAsync(tradePartyDTO);

        //Act
        var result = await systemUnderTest!.GetTradePartyAsync(tradePartyId);

        //Assert
        result.Should().BeOfType<ActionResult<TradePartyDto>>();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result.Result!).Value.Should().BeEquivalentTo(tradePartyDTO);
    }

    [Test]
    public async Task GetTradeParty_ShouldReturnBadRequest()
    {
        //Arrange
        var tradePartyId = Guid.NewGuid();
        _mockTradePartiesService
            .Setup(action => action.GetTradePartyAsync(tradePartyId))
            .ThrowsAsync(new Exception());

        //Act
        var result = await systemUnderTest!.GetTradePartyAsync(tradePartyId);

        //Assert
        result.Should().BeOfType<ActionResult<TradePartyDto>>();
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task GetTradePartyByDefraOrgId_ShouldReturnTradePartyDto_IfTradePartyWithDefraOrgIdExists()
    {
        //Arrange
        var orgId = Guid.NewGuid();
        var tradePartyDTO = new TradePartyDto { OrgId = orgId, PartyName = "partyname" };
        _mockTradePartiesService
            .Setup(action => action.GetTradePartyByDefraOrgIdAsync(orgId))
            .ReturnsAsync(tradePartyDTO);

        //Act
        var result = await systemUnderTest!.GetTradePartyByDefraOrgId(orgId);

        //Assert
        result.Should().BeOfType<ActionResult<TradePartyDto>>();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result.Result!).Value.Should().BeEquivalentTo(tradePartyDTO);
    }

    [Test]
    public async Task GetTradePartyByDefraOrgId_ShouldReturnNotFound_IfTradePartyWithDefraOrgIdDoesNotExist()
    {
        //Arrange
        var orgId = Guid.NewGuid();
        _mockTradePartiesService
            .Setup(action => action.GetTradePartyByDefraOrgIdAsync(orgId))
            .ReturnsAsync((TradePartyDto)null!);

        //Act
        var result = await systemUnderTest!.GetTradePartyByDefraOrgId(orgId);

        //Assert
        result.Should().BeOfType<ActionResult<TradePartyDto>>();
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Test]
    public async Task GetTradePartyByDefraOrgId_ShouldReturnBadRequest()
    {
        //Arrange
        var orgId = Guid.NewGuid();
        _mockTradePartiesService
            .Setup(action => action.GetTradePartyByDefraOrgIdAsync(orgId))
            .ThrowsAsync(new Exception());

        //Act
        var result = await systemUnderTest!.GetTradePartyByDefraOrgId(orgId);

        //Assert
        result.Should().BeOfType<ActionResult<TradePartyDto>>();
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task UpdateTradeParty_ShouldReturnOk_IfTradePartyUpdated()
    {
        //Arrange
        var tradePartyDTO = new TradePartyDto { Id = Guid.NewGuid(), PartyName = "partyname" };
        _mockTradePartiesService
            .Setup(action => action.UpdateTradePartyAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ReturnsAsync(tradePartyDTO);

        //Act
        var result = await systemUnderTest!.UpdateTradeParty(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task UpdateTradeParty_ShouldReturnNotFound_IfTradePartyNull()
    {
        //Arrange        
        _mockTradePartiesService
            .Setup(action => action.UpdateTradePartyAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ReturnsAsync(value: null!);

        //Act
        var result = await systemUnderTest!.UpdateTradeParty(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Test]
    public async Task UpdateTradeParty_ShouldReturnBadRequest()
    {
        //Arrange        
        _mockTradePartiesService
            .Setup(action => action.UpdateTradePartyAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ThrowsAsync(new Exception());

        //Act
        var result = await systemUnderTest!.UpdateTradeParty(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task UpdateTradePartyAddress_ShouldReturnOk_IfTradePartyAddressUpdated()
    {
        //Arrange
        var tradePartyDTO = new TradePartyDto { Id = Guid.NewGuid(), PartyName = "partyname" };
        _mockTradePartiesService
            .Setup(action => action.UpdateTradePartyAddressAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ReturnsAsync(tradePartyDTO);

        //Act
        var result = await systemUnderTest!.UpdateTradePartyAddress(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task UpdateTradePartyAddress_ShouldReturnNotFound_IfTradePartyAddressNull()
    {
        //Arrange        
        _mockTradePartiesService
            .Setup(action => action.UpdateTradePartyAddressAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ReturnsAsync(value: null!);

        //Act
        var result = await systemUnderTest!.UpdateTradePartyAddress(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Test]
    public async Task UpdateTradePartyAddress_ShouldReturnBadRequest()
    {
        //Arrange        
        _mockTradePartiesService
            .Setup(action => action.UpdateTradePartyAddressAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ThrowsAsync(new Exception());

        //Act
        var result = await systemUnderTest!.UpdateTradePartyAddress(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task AddTradePartyAddress_ShouldReturnNotFound_IfPartyDoesNotExist()
    {
        // Arrange
        Guid partyId = Guid.Empty;
        TradeAddressDto addressRequest = GenerateTradeAddressDto();
        _mockTradePartiesService
            .Setup(action => action.AddTradePartyAddressAsync(It.IsAny<Guid>(), It.IsAny<TradeAddressDto>()))
            .ReturnsAsync(value: null!);

        // Act
        var result = await systemUnderTest!.AddTradePartyAddress(partyId, addressRequest);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Test]
    public async Task AddTradePartyAddress_ShouldReturnOk_IfPartyExists()
    {
        //Arrange
        var tradePartyDTO = new TradePartyDto { Id = Guid.NewGuid(), PartyName = "partyname" };
        _mockTradePartiesService
            .Setup(action => action.AddTradePartyAddressAsync(It.IsAny<Guid>(), It.IsAny<TradeAddressDto>()))
            .ReturnsAsync(tradePartyDTO);

        //Act
        var result = await systemUnderTest!.AddTradePartyAddress(It.IsAny<Guid>(), It.IsAny<TradeAddressDto>());

        //Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task AddTradePartyAddress_ShouldReturnBadRequest()
    {
        //Arrange        
        _mockTradePartiesService
            .Setup(action => action.AddTradePartyAddressAsync(It.IsAny<Guid>(), It.IsAny<TradeAddressDto>()))
            .ThrowsAsync(new Exception());

        //Act
        var result = await systemUnderTest!.AddTradePartyAddress(It.IsAny<Guid>(), It.IsAny<TradeAddressDto>());

        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    private static TradeAddressDto GenerateTradeAddressDto()
    {
        return new TradeAddressDto
        {
            Id = Guid.NewGuid(),
            LineOne = "line 1",
            LineTwo = "line 2",
            CityName = "city",
            County = "county",
            TradeCountry = "GB",
            PostCode = "WE12 2SA",
        };
    }

    [Test]
    public async Task UpdateTradePartyContact_ShouldReturnOk_IfTradePartyContactUpdated()
    {
        //Arrange
        var tradePartyDTO = new TradePartyDto { Id = Guid.NewGuid(), PartyName = "partyname" };
        _mockTradePartiesService
            .Setup(action => action.UpdateTradePartyContactAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ReturnsAsync(tradePartyDTO);

        //Act
        var result = await systemUnderTest!.UpdateTradePartyContact(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task UpdateTradePartyContact_ShouldReturnNotFound_IfTradePartyContactNull()
    {
        //Arrange        
        _mockTradePartiesService
            .Setup(action => action.UpdateTradePartyContactAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ReturnsAsync(value: null!);

        //Act
        var result = await systemUnderTest!.UpdateTradePartyContact(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Test]
    public async Task UpdateTradePartyContact_ShouldReturnBadRequest()
    {
        //Arrange        
        _mockTradePartiesService
            .Setup(action => action.UpdateTradePartyContactAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ThrowsAsync(new Exception());

        //Act
        var result = await systemUnderTest!.UpdateTradePartyContact(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task UpdateContactSelfServe_ShouldReturnBadRequest()
    {
        //Arrange        
        _mockTradePartiesService
            .Setup(action => action.UpdateContactSelfServeAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ThrowsAsync(new Exception());

        //Act
        var result = await systemUnderTest!.UpdateTradePartyContactSelfServe(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task UpdateTradePartyContactSelfServe_ShouldReturnOk_IfTradePartyContactUpdated()
    {
        //Arrange
        var tradePartyDTO = new TradePartyDto { Id = Guid.NewGuid(), PartyName = "partyname" };
        _mockTradePartiesService
            .Setup(action => action.UpdateContactSelfServeAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ReturnsAsync(tradePartyDTO);

        //Act
        var result = await systemUnderTest!.UpdateTradePartyContactSelfServe(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task AddTradeParty_ReturnsCreatedAtRouteResult_WhenTradePartyAdded()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var expected = systemUnderTest?.CreatedAtRoute("GetTradePartyAsync", new { id = guid }, guid);
        var tradePartyDTO = new TradePartyDto { Id = guid, PartyName = "partyname" };
        _mockTradePartiesService
            .Setup(action => action.AddTradePartyAsync(It.IsAny<TradePartyDto>()))
            .ReturnsAsync(tradePartyDTO);

        //Act
        var result = await systemUnderTest!.AddTradeParty(It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task AddTradeParty_ReturnsBadRequestResult_WhenTradePartyIsNull()
    {
        //Arrange                        
        _mockTradePartiesService
            .Setup(action => action.AddTradePartyAsync(It.IsAny<TradePartyDto>()))
            .ReturnsAsync(value: null!);

        //Act
        var result = await systemUnderTest!.AddTradeParty(It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task AddTradeParty_ReturnsBadRequestResult_WhenExceptionThrown()
    {
        //Arrange        
        _mockTradePartiesService
            .Setup(action => action.AddTradePartyAsync(It.IsAny<TradePartyDto>()))
            .ThrowsAsync(new Exception());

        //Act
        var result = await systemUnderTest!.AddTradeParty(It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task GetTradePartyAsync_ReturnsNotFound()
    {
        //Arrange        
        _mockTradePartiesService.Setup(action => action.GetTradePartyAsync(It.IsAny<Guid>())).ReturnsAsync(value: null!);

        //Act
        var result = await systemUnderTest!.GetTradePartyAsync(Guid.Empty);

        //Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Test]
    public async Task UpdateAuthorisedSignatory_ShouldReturnOk_IfAuthorisedSignatory()
    {
        //Arrange
        var tradePartyDTO = new TradePartyDto { Id = Guid.NewGuid(), PartyName = "partyname" };
        _mockTradePartiesService
            .Setup(action => action.UpdateAuthorisedSignatoryAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ReturnsAsync(tradePartyDTO);

        //Act
        var result = await systemUnderTest!.UpdateAuthorisedSignatory(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task UpdateAuthorisedSignatory_ShouldReturnBadRequest()
    {
        //Arrange        
        _mockTradePartiesService
            .Setup(action => action.UpdateAuthorisedSignatoryAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ThrowsAsync(new Exception());

        //Act
        var result = await systemUnderTest!.UpdateAuthorisedSignatory(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task UpdateAuthorisedSignatorySelfServe_ShouldReturnBadRequest()
    {
        //Arrange        
        _mockTradePartiesService
            .Setup(action => action.UpdateAuthRepSelfServeAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ThrowsAsync(new Exception());

        //Act
        var result = await systemUnderTest!.UpdateAuthorisedSignatorySelfServe(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public async Task UpdateAuthorisedSignatorySelfServe_ShouldReturnOk_IfAuthorisedSignatory()
    {
        //Arrange
        var tradePartyDTO = new TradePartyDto { Id = Guid.NewGuid(), PartyName = "partyname" };
        _mockTradePartiesService
            .Setup(action => action.UpdateAuthRepSelfServeAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ReturnsAsync(tradePartyDTO);

        //Act
        var result = await systemUnderTest!.UpdateAuthorisedSignatorySelfServe(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task UpdateAuthorisedSignatory_ShouldReturnNotFound_IfAuthorisedSignatoryNull()
    {
        //Arrange        
        _mockTradePartiesService
            .Setup(action => action.UpdateAuthorisedSignatoryAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ReturnsAsync(value: null!);

        //Act
        var result = await systemUnderTest!.UpdateAuthorisedSignatory(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Test]
    public async Task UpdateAuthorisedSignatorySelfServe_ShouldReturnNotFound_IfAuthorisedSignatoryNull()
    {
        //Arrange        
        _mockTradePartiesService
            .Setup(action => action.UpdateAuthRepSelfServeAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>()))
            .ReturnsAsync(value: null!);

        //Act
        var result = await systemUnderTest!.UpdateAuthorisedSignatorySelfServe(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    private static List<TradePartyDto> GenerateTradeParties()
    {
        return new List<TradePartyDto>
        {
            new TradePartyDto
            {
                Id = Guid.NewGuid(),
                PartyName = "COmpany A Ltd"
            },
            new TradePartyDto
            {
                Id = Guid.NewGuid(),
                PartyName = "Company B Ltd"
            }
        };
    }
}

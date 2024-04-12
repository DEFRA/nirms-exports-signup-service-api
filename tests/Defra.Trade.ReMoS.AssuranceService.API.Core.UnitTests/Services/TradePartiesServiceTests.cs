using AutoMapper;
using Azure.Messaging.ServiceBus;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Profiles;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Services;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Moq;
using NUnit.Framework.Internal;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.UnitTests.Services;

public class TradePartiesServiceTests
{
    private readonly Mock<ITradePartyRepository> _mockTradePartyRepository = new();
    private readonly Mock<IEstablishmentRepository> _mockEstablishmentRepository = new();
    private readonly Mock<IOptions<TradePlatform>> _mockTradePlatformConfig = new();
    private readonly Mock<IFeatureManager> _mockFeatureManager = new();
    private readonly Mock<ServiceBusClient> _mockServiceBusClient = new();
    private TradePartiesService? _sut;

    [SetUp]
    public void Setup()
    {
        var profiles = new List<Profile>()
        {
            new TradePartyProfiler(),
        };

        var mapperConfiguration = new MapperConfiguration(
            cfg => cfg.AddProfiles(new List<Profile>(profiles)));
        var mapper = new Mapper(mapperConfiguration);

        _sut = new TradePartiesService(
            _mockTradePartyRepository.Object,
            mapper,
            _mockEstablishmentRepository.Object,
            _mockTradePlatformConfig.Object,
            _mockFeatureManager.Object,
            _mockServiceBusClient.Object);
    }

    [Test]
    public async Task AddTradePartyAsync_ReturnsTradePartyDto()
    {
        //Arrange
        var tradePartyRequest = new TradePartyDto { PartyName = "party name" };
        _mockTradePartyRepository
            .Setup(action => action.AddTradePartyAsync(It.IsAny<TradeParty>(), It.IsAny<CancellationToken>()));

        //Act
        var result = await _sut!.AddTradePartyAsync(tradePartyRequest);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TradePartyDto?>();
    }

    [Test]
    public async Task GetTradePartiesAsync_ReturnsTradePartyDto()
    {
        //Arrange
        var tradePartyRequests = new List<TradeParty>().AsEnumerable();
        _mockTradePartyRepository
            .Setup(action => action.GetAllTradeParties()).Returns(Task.FromResult(tradePartyRequests));

        //Act
        var result = await _sut!.GetTradePartiesAsync();

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<TradePartyDto>>();
    }

    [Test]
    public async Task GetTradePartyAsync_ReturnsTradePartyDto()
    {
        //Arrange
        var guid = new Guid();

        var tradePartyRequest = new TradeParty
        {
            Id = guid
        };

        _mockTradePartyRepository
            .Setup(action => action.GetTradePartyAsync(It.IsAny<Guid>())).Returns(Task.FromResult(tradePartyRequest)!);

        //Act
        var result = await _sut!.GetTradePartyAsync(guid);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TradePartyDto>();
    }

    [Test]
    public async Task GetTradePartyByDefraOrgIdAsync_ReturnsTradePartyDto()
    {
        //Arrange
        var orgId = new Guid();

        var tradePartyRequest = new TradeParty
        {
            OrgId = orgId
        };

        _mockTradePartyRepository
            .Setup(action => action.GetTradePartyByDefraOrgIdAsync(It.IsAny<Guid>()))
            .Returns(Task.FromResult(tradePartyRequest)!);

        //Act
        var result = await _sut!.GetTradePartyByDefraOrgIdAsync(orgId);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TradePartyDto>();
    }

    [Test]
    public async Task GetTradePartyByDefraOrgIdAsync_ReturnsNull_IfPartyWithGivenOrgIdDoesNotExist()
    {
        //Arrange
        var orgId = new Guid();

        var tradePartyRequest = new TradeParty
        {
            OrgId = orgId
        };

        _mockTradePartyRepository
            .Setup(action => action.GetTradePartyByDefraOrgIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((TradeParty)null!);

        //Act
        var result = await _sut!.GetTradePartyByDefraOrgIdAsync(orgId);

        //Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task UpdateTradePartyAsync_ReturnsTradePartyDto()
    {
        //Arrange
        var partyId = Guid.NewGuid();
        var tradePartyRequest = new TradePartyDto { Id = partyId, PartyName = "party name" };
        var tradePartyEntity = new TradeParty { Id = partyId, Name = "party name" };
        _mockTradePartyRepository
            .Setup(action => action.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tradePartyEntity);
        _mockTradePartyRepository
            .Setup(action => action.UpdateTradeParty(It.IsAny<TradeParty>(), It.IsAny<CancellationToken>()))
            .Returns(tradePartyEntity);
        _mockFeatureManager.Setup(action => action.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(false);

        //Act
        var result = await _sut!.UpdateTradePartyAsync(partyId, tradePartyRequest);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<TradePartyDto?>();
    }

    [Test]
    public async Task UpdateTradePartyAsync_GivenNotFoundId_ReturnsNull()
    {
        //Arrange
        var guid = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689");
        _mockTradePartyRepository
            .Setup(action => action.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null!);

        //Act
        var result = await _sut!.UpdateTradePartyAsync(guid, new TradePartyDto());

        //Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetTradePartiesAsync_ReturnsTradeParties()
    {
        //Arrange
        var tradeParties = new List<TradeParty> { new TradeParty(), new TradeParty() };
        _mockTradePartyRepository.Setup(x => x.GetAllTradeParties()).Returns(Task.FromResult(tradeParties.AsEnumerable()));

        //Act
        var result = await _sut!.GetTradePartiesAsync();

        //Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(tradeParties.Count);
        result.FirstOrDefault().Should().BeOfType<TradePartyDto>();
    }

    [Test]
    public async Task UpdateTradePartyAddressAsync_ReturnsTradeParties()
    {
        //Arrange
        var tradeParty = new TradeParty
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689")
        };

        var tradePartyDto = new TradePartyDto
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689")
        };

        _mockTradePartyRepository.Setup(x => x.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(tradeParty)!);

        //Act
        var result = await _sut!.UpdateTradePartyAddressAsync(It.IsAny<Guid>(), tradePartyDto);

        //Assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task UpdateTradePartyAddressAsync_GivenNotFoundId_ReturnsNull()
    {
        //Arrange
        _mockTradePartyRepository
            .Setup(action => action.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(value: null!);

        //Act
        var result = await _sut!.UpdateTradePartyAddressAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task AddTradePartyAddressAsync_ReturnsTradeParty()
    {
        //Arrange
        var tradeParty = new TradeParty
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689")
        };

        var tradeAddressDto = new TradeAddressDto
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689")
        };

        _mockTradePartyRepository.Setup(x => x.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(tradeParty)!);

        //Act
        var result = await _sut!.AddTradePartyAddressAsync(It.IsAny<Guid>(), tradeAddressDto);

        //Assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task AddTradePartyAddressAsync_GivenNotFoundId_ReturnsNull()
    {
        //Arrange
        _mockTradePartyRepository
            .Setup(action => action.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(value: null!);

        //Act
        var result = await _sut!.AddTradePartyAddressAsync(It.IsAny<Guid>(), It.IsAny<TradeAddressDto>());

        //Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task UpdateTradePartyContactAsync_ReturnsTradeParties()
    {
        //Arrange
        var tradeParty = new TradeParty
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689")
        };

        var tradePartyDto = new TradePartyDto
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689")
        };

        _mockTradePartyRepository.Setup(x => x.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(tradeParty)!);

        //Act
        var result = await _sut!.UpdateTradePartyContactAsync(new Guid(), tradePartyDto);

        //Assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task UpdateTradePartyContactSelfServeAsync_ReturnsTradeParties()
    {
        //Arrange
        var partyId = Guid.NewGuid();
        var tradeContact = new TradeContact()
        {
            Id = Guid.NewGuid(),
            TradePartyId = partyId,
            PersonName = "test",
            Position = "test",
            Email = "test",
            TelephoneNumber = "test"
        };
        var authSig = new AuthorisedSignatory()
        {
            Id = Guid.NewGuid(),
            TradePartyId = partyId,
            Position = "test",
            EmailAddress = "test",
            Name = "test"
        };
        var address = new TradeAddress()
        {
            Id = Guid.NewGuid(),
            LineOne = "test",
            PostCode = "test",
            CityName = "test",
            County = "test"
        };
        var logisticsLocation = new LogisticsLocation()
        {
            Id = Guid.NewGuid(),
            TradePartyId = partyId,
            TradeAddressId = address.Id,
            Name = "test",
            RemosEstablishmentSchemeNumber = "RMS-GB-000001-001",
            NI_GBFlag = "GB",
            CreatedDate = DateTime.UtcNow
        };
        var tp = new TradePlatform()
        {
            ServiceBusName = "test",
            ServiceBusConnectionString = "test"
        };
        List<LogisticsLocation> locations = new()
        {
            logisticsLocation
        };
        var tradePartyRequest = new TradePartyDto { Id = partyId, PartyName = "party name", SignUpRequestSubmittedBy = Guid.NewGuid() };
        var tradePartyEntity = new TradeParty { Id = partyId, Name = "party name", TradeContact = tradeContact, AuthorisedSignatory = authSig, LogisticsLocations = locations };

        _mockTradePartyRepository.Setup(x => x.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(tradePartyEntity)!);
        _mockTradePlatformConfig.Setup(action => action.Value).Returns(tp);
        var sbMock = new Mock<ServiceBusSender>();
        _mockServiceBusClient.Setup(action => action.CreateSender(It.IsAny<string>())).Returns(sbMock.Object);

        //Act
        var result = await _sut!.UpdateContactSelfServeAsync(new Guid(), tradePartyRequest);

        //Assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task UpdateTradePartyContactAsync_GivenNotFoundId_ReturnsNull()
    {
        //Arrange
        _mockTradePartyRepository
            .Setup(action => action.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null!);

        //Act
        var result = await _sut!.UpdateTradePartyContactAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task UpdateTradePartyContactSelfServeAsync_GivenNotFoundId_ReturnsNull()
    {
        //Arrange
        _mockTradePartyRepository
            .Setup(action => action.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null!);

        //Act
        var result = await _sut!.UpdateContactSelfServeAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task UpdateTradePartyContactAsync_GivenTradeContactExists_ReturnsTradeParties()
    {
        //Arrange
        var tradeContact = new TradeContact();

        var tradeContactDto = new TradeContactDto();

        var tradeParty = new TradeParty
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689"),
            TradeContact = new TradeContact()
        };

        var tradePartyDto = new TradePartyDto
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689"),
            Contact = new TradeContactDto()
        };

        _mockTradePartyRepository.Setup(x => x.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(tradeParty)!);

        //Act
        var result = await _sut!.UpdateTradePartyContactAsync(new Guid(), tradePartyDto);

        //Assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task UpdateTradePartyContactSelfServeAsync_GivenTradeContactExists_ReturnsTradeParties()
    {
        //Arrange
        var tradeContact = new TradeContact();

        var tradeContactDto = new TradeContactDto();

        var tradeParty = new TradeParty
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689"),
            TradeContact = new TradeContact()
        };

        var tradePartyDto = new TradePartyDto
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689"),
            Contact = new TradeContactDto()
        };

        _mockTradePartyRepository.Setup(x => x.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(tradeParty)!);

        //Act
        var result = await _sut!.UpdateContactSelfServeAsync(new Guid(), tradePartyDto);

        //Assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task UpdateTradePartyContactAsync_GivenTradeContactIdExists_ReturnsTradeParties()
    {
        //Arrange
        var tradeContact = new TradeContact
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689")
        };

        var tradeContactDto = new TradeContactDto
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689")
        };

        var tradeParty = new TradeParty
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689"),
            TradeContact = tradeContact
        };

        var tradePartyDto = new TradePartyDto
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689"),
            Contact = tradeContactDto
        };

        _mockTradePartyRepository.Setup(x => x.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(tradeParty)!);

        //Act
        var result = await _sut!.UpdateTradePartyContactAsync(new Guid(), tradePartyDto);

        //Assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task UpdateAuthorisedSignatoryAsync_ReturnsTradeParties()
    {
        //Arrange
        var tradeParty = new TradeParty
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689")
        };

        var tradePartyDto = new TradePartyDto
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689")
        };

        _mockTradePartyRepository.Setup(x => x.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(tradeParty)!);
        _mockTradePartyRepository.Setup(x => x.UpsertAuthorisedSignatory(It.IsAny<TradeParty>(), It.IsAny<CancellationToken>())).Returns(tradeParty!);

        //Act
        var result = await _sut!.UpdateAuthorisedSignatoryAsync(It.IsAny<Guid>(), tradePartyDto);

        //Assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task UpdateAuthorisedSignatoryAsync_UpsertTradePartyContact_ReturnsTradeParties()
    {
        //Arrange
        var tradeParty = new TradeParty
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689"),
            TradeContact = new TradeContact { IsAuthorisedSignatory = true }

        };

        var tradePartyDto = new TradePartyDto
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689")
        };

        _mockTradePartyRepository.Setup(x => x.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(tradeParty)!);
        _mockTradePartyRepository.Setup(x => x.UpsertAuthorisedSignatory(It.IsAny<TradeParty>(), It.IsAny<CancellationToken>())).Returns(tradeParty!);
        _mockTradePartyRepository.Setup(x => x.UpsertTradePartyContact(It.IsAny<TradeParty>(), It.IsAny<CancellationToken>())).Returns(tradeParty!);

        //Act
        var result = await _sut!.UpdateAuthorisedSignatoryAsync(It.IsAny<Guid>(), tradePartyDto);

        //Assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task UpdateAuthorisedSignatoryAsync_GivenNotFoundId_ReturnsNull()
    {
        //Arrange
        _mockTradePartyRepository
            .Setup(action => action.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(value: null!);

        //Act
        var result = await _sut!.UpdateAuthorisedSignatoryAsync(It.IsAny<Guid>(), It.IsAny<TradePartyDto>());

        //Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task UpdateAuthorisedSignatoryAsync_IfContactDetailsAlreadyComplete_ReturnTradeParty()
    {
        //Arrange
        var tradeParty = new TradeParty
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689"),
            AuthorisedSignatory = new AuthorisedSignatory()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Position = "Developer",
                EmailAddress = "test@email.com"
            }
        };

        var tradePartyRequest = new TradePartyDto
        {
            Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689"),
            AuthorisedSignatory = null,
            Contact = new TradeContactDto()
            {
                IsAuthorisedSignatory = false,
            }
        };

        _mockTradePartyRepository.Setup(x => x.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(tradeParty)!);
        _mockTradePartyRepository.Setup(x => x.UpsertAuthorisedSignatory(It.IsAny<TradeParty>(), It.IsAny<CancellationToken>())).Returns(tradeParty!);

        //Act
        var result = await _sut!.UpdateAuthorisedSignatoryAsync(It.IsAny<Guid>(), tradePartyRequest);

        //Assert        
        _mockTradePartyRepository.Verify(m => m.UpsertAuthorisedSignatory(It.IsAny<TradeParty>(), It.IsAny<CancellationToken>()), Times.Never);

        result?.AuthorisedSignatory?.Name.Should().Be("Test");
        result?.AuthorisedSignatory?.Position.Should().Be("Developer");
        result?.AuthorisedSignatory?.EmailAddress.Should().Be("test@email.com");
    }

    [Test]
    public async Task UpdateTradePartyAsync_CallServiceBus()
    {
        //Arrange
        var partyId = Guid.NewGuid();
        var tradeContact = new TradeContact()
        {
            Id = Guid.NewGuid(),
            TradePartyId = partyId,
            PersonName = "test",
            Position = "test",
            Email = "test",
            TelephoneNumber = "test"
        };
        var authSig = new AuthorisedSignatory()
        {
            Id = Guid.NewGuid(),
            TradePartyId = partyId,
            Position = "test",
            EmailAddress = "test",
            Name = "test"
        };
        var address = new TradeAddress()
        {
            Id = Guid.NewGuid(),
            LineOne = "test",
            PostCode = "test",
            CityName = "test",
            County = "test"
        };
        var logisticsLocation = new LogisticsLocation()
        {
            Id = Guid.NewGuid(),
            TradePartyId = partyId,
            TradeAddressId = address.Id,
            Name = "test",
            RemosEstablishmentSchemeNumber = "RMS-GB-000001-001",
            NI_GBFlag = "GB",
            CreatedDate = DateTime.UtcNow
        };
        var tp = new TradePlatform()
        {
            ServiceBusName = "test",
            ServiceBusConnectionString = "test"
        };
        List<LogisticsLocation> locations = new()
        {
            logisticsLocation
        };
        var tradePartyRequest = new TradePartyDto { Id = partyId, PartyName = "party name", SignUpRequestSubmittedBy = Guid.NewGuid() };
        var tradePartyEntity = new TradeParty { Id = partyId, Name = "party name", TradeContact = tradeContact, AuthorisedSignatory = authSig, LogisticsLocations = locations };
        _mockTradePartyRepository
            .Setup(action => action.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tradePartyEntity);
        _mockTradePartyRepository
            .Setup(action => action.UpdateTradeParty(It.IsAny<TradeParty>(), It.IsAny<CancellationToken>()))
            .Returns(tradePartyEntity);
        _mockFeatureManager.Setup(action => action.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(true);
        _mockTradePlatformConfig.Setup(action => action.Value).Returns(tp);
        var sbMock = new Mock<ServiceBusSender>();
        _mockServiceBusClient.Setup(action => action.CreateSender(It.IsAny<string>())).Returns(sbMock.Object);

        //Act
        var result = await _sut!.UpdateTradePartyAsync(partyId, tradePartyRequest);


        //Assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task UpdateAuthRepAsync_CallServiceBus()
    {
        //Arrange
        var partyId = Guid.NewGuid();
        var tradeContact = new TradeContact()
        {
            Id = Guid.NewGuid(),
            TradePartyId = partyId,
            PersonName = "test",
            Position = "test",
            Email = "test",
            TelephoneNumber = "test"
        };
        var authSig = new AuthorisedSignatory()
        {
            Id = Guid.NewGuid(),
            TradePartyId = partyId,
            Position = "test",
            EmailAddress = "test",
            Name = "test"
        };
        var address = new TradeAddress()
        {
            Id = Guid.NewGuid(),
            LineOne = "test",
            PostCode = "test",
            CityName = "test",
            County = "test"
        };
        var logisticsLocation = new LogisticsLocation()
        {
            Id = Guid.NewGuid(),
            TradePartyId = partyId,
            TradeAddressId = address.Id,
            Name = "test",
            RemosEstablishmentSchemeNumber = "RMS-GB-000001-001",
            NI_GBFlag = "GB",
            CreatedDate = DateTime.UtcNow
        };
        var tp = new TradePlatform()
        {
            ServiceBusName = "test",
            ServiceBusConnectionString = "test"
        };
        List<LogisticsLocation> locations = new()
        {
            logisticsLocation
        };
        var tradePartyRequest = new TradePartyDto { Id = partyId, PartyName = "party name", SignUpRequestSubmittedBy = Guid.NewGuid() };
        var tradePartyEntity = new TradeParty { Id = partyId, Name = "party name", TradeContact = tradeContact, AuthorisedSignatory = authSig, LogisticsLocations = locations };
        _mockTradePartyRepository
            .Setup(action => action.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tradePartyEntity);
        _mockTradePartyRepository
            .Setup(action => action.UpdateTradeParty(It.IsAny<TradeParty>(), It.IsAny<CancellationToken>()))
            .Returns(tradePartyEntity);
        _mockFeatureManager.Setup(action => action.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(true);
        _mockTradePlatformConfig.Setup(action => action.Value).Returns(tp);
        var sbMock = new Mock<ServiceBusSender>();
        _mockServiceBusClient.Setup(action => action.CreateSender(It.IsAny<string>())).Returns(sbMock.Object);

        //Act
        var result = await _sut!.UpdateAuthRepSelfServeAsync(partyId, tradePartyRequest);


        //Assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task UpdateTradePartyAsync_FboPhrOption_Is_None()
    {
        //Arrange
        var partyId = Guid.NewGuid();
        var tradeContact = new TradeContact()
        {
            Id = Guid.NewGuid(),
            TradePartyId = partyId,
            PersonName = "test",
            Position = "test",
            Email = "test",
            TelephoneNumber = "test"
        };
        var authSig = new AuthorisedSignatory()
        {
            Id = Guid.NewGuid(),
            TradePartyId = partyId,
            Position = "test",
            EmailAddress = "test",
            Name = "test"
        };
        var tradePartyRequest = new TradePartyDto { Id = partyId, PartyName = "party name", FboPhrOption = "none", SignUpRequestSubmittedBy = Guid.NewGuid()};
        var tradePartyEntity = new TradeParty { Id = partyId, Name = "party name" };
        _mockTradePartyRepository
            .Setup(action => action.FindTradePartyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(tradePartyEntity);
        _mockTradePartyRepository
            .Setup(action => action.UpdateTradeParty(It.IsAny<TradeParty>(), It.IsAny<CancellationToken>()))
            .Returns(tradePartyEntity);
        _mockFeatureManager.Setup(action => action.IsEnabledAsync(It.IsAny<string>())).ReturnsAsync(false);

        //Act
        var result = await _sut!.UpdateTradePartyAsync(partyId, tradePartyRequest);


        //Assert
        result.Should().NotBeNull();
    }
}

using AutoMapper;
using Defra.Trade.Address.V1.ApiClient.Api;
using Defra.Trade.Address.V1.ApiClient.Client;
using Defra.Trade.Address.V1.ApiClient.Model;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Profiles;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Models;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Services;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Repositories;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.UnitTests.Services
{
    public class EstablishmentsServiceTests
    {
        private readonly Mock<IEstablishmentRepository> _mockEstablishmentRepository = new();
        private readonly Mock<IAddressRepository> _mockAddressRepository = new();
        private readonly Mock<ITradePartyRepository> _mockTradePartyRepository = new();
        private readonly Mock<IOsPlacesRepository> _mockOsPlacesRepository = new();
        private readonly Mock<IPlacesApi> _mockPlacesApi = new();

        private EstablishmentsService? _sut;

        [SetUp]
        public void Setup()
        {

            var profiles = new List<Profile>()
            {
                new TradePartyProfiler(),
                new LogisticsLocationProfiler(),
                new DpaProfiler(),
                new TradeAddressApiProfiler()
            };

            var mapperConfiguration = new MapperConfiguration(
                cfg => cfg.AddProfiles(new List<Profile>(profiles)));
            var mapper = new Mapper(mapperConfiguration);

            _sut = new EstablishmentsService(
                _mockEstablishmentRepository.Object,
                _mockAddressRepository.Object,
                _mockTradePartyRepository.Object,
                _mockOsPlacesRepository.Object,
                mapper,
                _mockPlacesApi.Object);
        }

        [Test]
        public async Task GetLogisticsLocationByIdAsync_ReturnsLogisticsLocation()
        {
            //Arrange
            _mockEstablishmentRepository
                .Setup(action => action.GetLogisticsLocationByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new LogisticsLocation())!);

            //Act
            var result = await _sut!.GetLogisticsLocationByIdAsync(It.IsAny<Guid>());

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<LogisticsLocationDto?>();
        }

        [Test]
        public async Task GetAllLogisticsLocationsAsync_ReturnsLogisticsLocations()
        {
            //Arrange
            _mockEstablishmentRepository
                .Setup(action => action.GetAllLogisticsLocationsAsync()).Returns(Task.FromResult(new List<LogisticsLocation>().AsEnumerable())!);

            //Act
            var result = await _sut!.GetAllLogisticsLocationsAsync();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<LogisticsLocationDto>>();
        }

        [Test]
        public async Task GetLogisticsLocationsByPostcodeAsync_ReturnsLogisticsLocations()
        {
            //Arrange
            _mockEstablishmentRepository
                .Setup(action => action.GetLogisticsLocationByPostcodeAsync(It.IsAny<string>())).Returns(Task.FromResult(new List<LogisticsLocation>().AsEnumerable())!);

            //Act
            var result = await _sut!.GetLogisticsLocationsByPostcodeAsync(It.IsAny<string>());

            //Assert
            result.Should().NotBeNull();
            result!.ToList().Should().BeOfType<List<LogisticsLocationDto>>();
        }

        [TestCase("GB", "CB11 3SN")]
        [TestCase("NI", "BT11 3SN")]
        public void GetLogisticsLocationByUprnAsync_ReturnsLogisticLocation(string NI_GB, string postcode)
        {
            // arrange
            var uprn = "1234";
            var addressDto = new AddressDto(uprn, null, null, null, null, null, postcode)
            {
                Address = "Business name, Some, other, string, Street name, Town, CB11 3SN",
                PostTown = "Town",
                ThroughfareName = "Street name"
            };
            var addressesDto = new List<AddressDto> { addressDto };
            _mockPlacesApi.Setup(x => x.UprnLookupAsync(uprn, null, 0)).Returns(addressesDto);

            // act
            var result = _sut!.GetLogisticsLocationByUprnAsync(uprn);

            // assert
            result.Should().BeOfType<LogisticsLocationDto>();
            result?.NI_GBFlag.Should().Be(NI_GB);
        }

        [Test]
        public void GetLogisticsLocationByUprnAsync_ReturnsLogisticLocation_AdjustedAddressLine()
        {
            // arrange
            var NI_GB = "NI";
            var uprn = "1234";
            var postcode = "BT11 3SN";
            var streetName = "Very Long Line Value, which is over 50 characters, so should be split";

            var addressDto = new AddressDto(uprn, null, null, null, null, null, postcode)
            {
                Address = $"{streetName}, Some, other, string, Test, Town, CB11 3SN",
                PostTown = "Town",
                ThroughfareName = streetName
            };

            var addressesDto = new List<AddressDto> { addressDto };
            _mockPlacesApi.Setup(x => x.UprnLookupAsync(uprn, null, 0)).Returns(addressesDto);

            // act
            var result = _sut!.GetLogisticsLocationByUprnAsync(uprn);

            // assert
            result.Should().BeOfType<LogisticsLocationDto>();
            result?.NI_GBFlag.Should().Be(NI_GB);

            result?.Address?.LineOne.Should().Be("Very Long Line Value, which is over 50 characters,");
            result?.Address?.LineTwo.Should().Be("so should be split");
        }

        [Test]
        public void GetLogisticsLocationByUprnAsync_ReturnsLogisticLocation_UntouchedAddressLine()
        {
            // arrange
            var NI_GB = "NI";
            var uprn = "1234";
            var postcode = "BT11 3SN";
            var streetName = "Very short Line Value";

            var addressDto = new AddressDto(uprn, null, null, null, null, null, postcode)
            {
                Address = $"{streetName}, Some, other, string, Test, Town, CB11 3SN",
                PostTown = "Town",
                ThroughfareName = streetName
            };

            var addressesDto = new List<AddressDto> { addressDto };
            _mockPlacesApi.Setup(x => x.UprnLookupAsync(uprn, null, 0)).Returns(addressesDto);

            // act
            var result = _sut!.GetLogisticsLocationByUprnAsync(uprn);

            // assert
            result.Should().BeOfType<LogisticsLocationDto>();
            result?.NI_GBFlag.Should().Be(NI_GB);

            result?.Address?.LineOne.Should().Be("Very short Line Value");
        }

        [Test]
        public void GetLogisticsLocationByUprnAsync_ReturnsNull()
        {
            // arrange
            var uprn = "1234";
            List<AddressDto>? addressDto = null;
            _mockPlacesApi.Setup(x => x.UprnLookupAsync(uprn, null, 0)).Returns(addressDto!);

            // act
            var result = _sut!.GetLogisticsLocationByUprnAsync(uprn);

            // assert
            result.Should().BeNull();
        }

        [Test]
        public void GetTradeAddressApiByPostcode_ReturnsAddress()
        {
            // arrange
            var postcode = "CB11 3SN";
            var addressDto = new AddressDto("1234", null, null, null, null, null, postcode);
            var addressesDto = new List<AddressDto>() { addressDto };
            _mockPlacesApi.Setup(x => x.PostCodeLookupAsync(postcode, null, 0)).Returns(addressesDto);

            // act
            var result = _sut!.GetTradeAddressApiByPostcode(postcode);

            // assert
            result.Should().BeOfType<List<AddressDto>>();
            result.FirstOrDefault().Should().BeEquivalentTo(addressDto);
        }

        [Test]
        public void GetTradeAddressApiByPostcode_ReturnsEmptyAddress()
        {
            // arrange
            var postcode = "CB11 3SN";
            var addressesDto = new List<AddressDto>();
            _mockPlacesApi.Setup(x => x.PostCodeLookupAsync(postcode, null, 0)).Throws(new Exception());

            // act
            var result = _sut!.GetTradeAddressApiByPostcode(postcode);

            // assert
            result.Should().BeOfType<List<AddressDto>>();
            result.Should().BeEquivalentTo(addressesDto);
        }

        [Test]
        public async Task AddLogisticsLocationAsync_ReturnsLogisticsLocations()
        {
            //Arrange
            var tradePartyId = Guid.NewGuid();

            var tradeParty = new TradeParty
            {
                Id = tradePartyId
            };

            var logisticsLocation = new LogisticsLocation
            {
                Id = Guid.NewGuid(),
                Name = "test ltd",
                TradePartyId = tradePartyId,
                TradeAddressId = Guid.NewGuid(),
                Address = new TradeAddress(),
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                NI_GBFlag = "GBFlag",
            };

            var logisticsLocationAddDto = new LogisticsLocationDto
            {
                Name = "Test Name",
                Id = Guid.NewGuid(),
                Address = new TradeAddressDto { Id = new Guid() }
            };

            _mockEstablishmentRepository
                .Setup(action => action.AddLogisticsLocationAsync(It.IsAny<LogisticsLocation>()))
                .Returns(Task.FromResult(logisticsLocation));

            _mockTradePartyRepository
                .Setup(action => action.GetTradePartyAsync(It.IsAny<Guid>()))
                .ReturnsAsync(tradeParty);
            _mockTradePartyRepository
                .Setup(action => action.TradePartyExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            //Act
            var result = await _sut!.AddLogisticsLocationAsync(tradePartyId, logisticsLocationAddDto);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<LogisticsLocationDto>();
        }


        [Test]
        public async Task AddLogisticsLocationAsync_AddressDoesNotExist_ReturnsNull()
        {
            //Arrange
            var tradepartyId = Guid.NewGuid();
            var logisticsLocationDto = new LogisticsLocationDto
            {
                Name = "Test Name",
                Id = Guid.NewGuid(),
                Address = new TradeAddressDto { Id = Guid.Empty },   
                TradeAddressId = Guid.NewGuid()
            };

            _mockTradePartyRepository.Setup(x => x.TradePartyExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);
            _mockAddressRepository.Setup(x => x.AddressExistsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);
            //Act
            var result = await _sut!.AddLogisticsLocationAsync(tradepartyId, logisticsLocationDto);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task AddLogisticsLocationAsync_GivenTradeAddressIsNull_ReturnsNull()
        {
            //Arrange
            var tradePartyId = Guid.NewGuid();
            var logisticsLocation = new LogisticsLocation
            {
                Id = Guid.NewGuid(),
                Name = "test ltd",
                TradePartyId = tradePartyId,
                TradeAddressId = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689"),
                Address = new TradeAddress(),
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                NI_GBFlag = "GBFlag",
            };

            var logisticsLocationAddDto = new LogisticsLocationDto
            {
                Name = "Test Name",
                Id = Guid.NewGuid(),
                TradeAddressId = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689"),
                Address = new TradeAddressDto { Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689") }
            };

            _mockEstablishmentRepository
                .Setup(action => action.AddLogisticsLocationAsync(It.IsAny<LogisticsLocation>())).Returns(Task.FromResult(logisticsLocation));

            _mockAddressRepository
                .Setup(action => action.AddressExistsAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

            //Act
            var result = await _sut!.AddLogisticsLocationAsync(tradePartyId, logisticsLocationAddDto);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task GetLogisticsLocationsForTradePartyAsync_ReturnsLogisticsLocationDTOs()
        {
            //Arrange
            var guid = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689");

            var logisticLocation = new LogisticsLocation
            {
                Id = guid,
                Name = "abc",
                TradePartyId = guid,
                Email = "contact@test.com"
            };

            var list = new List<LogisticsLocation> { logisticLocation };
            _mockTradePartyRepository.Setup(action => action.TradePartyExistsAsync(guid)).Returns(Task.FromResult(true)!);
            _mockEstablishmentRepository.Setup(action => action.GetActiveLogisticsLocationsForTradePartyAsync(guid)).Returns(Task.FromResult(list.AsEnumerable())!);

            //Act
            var result = await _sut!.GetLogisticsLocationsForTradePartyAsync(guid);

            //Assert
            result.Should().NotBeNull();
        }

        [Test]
        public async Task GetLogisticsLocationsForTradePartyAsync_GivenTradePartyDoesNotExist_ReturnsNull()
        {
            //Arrange
            var guid = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689");

            var logisticLocation = new LogisticsLocation
            {
                Id = guid,
                Name = "abc",
                TradePartyId = guid,
                Email = "contact@test.com"
            };

            var list = new List<LogisticsLocation> { logisticLocation };
            _mockTradePartyRepository.Setup(action => action.TradePartyExistsAsync(guid)).Returns(Task.FromResult(false)!);
            _mockEstablishmentRepository.Setup(action => action.GetActiveLogisticsLocationsForTradePartyAsync(guid)).Returns(Task.FromResult(list.AsEnumerable())!);

            //Act
            var result = await _sut!.GetLogisticsLocationsForTradePartyAsync(guid);

            //Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task RemoveLogisticsLocationAsync_ReturnsTrue()
        {
            //Arrange
            var guid = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689");
            _mockEstablishmentRepository.Setup(action => action.GetLogisticsLocationByIdAsync(guid)).ReturnsAsync(new LogisticsLocation());
            _mockEstablishmentRepository.Setup(action => action.SaveChangesAsync(new CancellationToken())).ReturnsAsync(true);

            //Act
            var result = await _sut!.RemoveLogisticsLocationAsync(guid);

            //Assert
            result.Should().Be(true);
        }


        [Test]
        public async Task RemoveLogisticsLocationAsync_ReturnsFalse()
        {
            //Arrange
            var guid = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689");
            _mockEstablishmentRepository.Setup(action => action.GetLogisticsLocationByIdAsync(guid)).Returns(Task.FromResult<LogisticsLocation?>(null));
            _mockEstablishmentRepository.Setup(action => action.SaveChangesAsync(new CancellationToken())).ReturnsAsync(true);

            //Act
            var result = await _sut!.RemoveLogisticsLocationAsync(guid);

            //Assert
            result.Should().Be(false);
        }

        [Test]
        public async Task GetOSPlacesLocations_ReturnsListOfLocations()
        {
            //Arrange
            var osPlace = new OsPlaces
            {
                Results = new List<OsPlacesResult> {
                    new OsPlacesResult {
                        Dpa = new Dpa
                        {
                            ADDRESS = "10, Downing Street, London, SW1A 2AA",
                            POSTCODE = "SW1A 2AA",
                            POST_TOWN = "London"
                        }
                    }
                }
            };

            //TODO: Replace with mocked interface - being used to test development code currently
            //Arrange
            _mockOsPlacesRepository.Setup(action => action.GetOSPlacesLocationsFromPostCodeAsync("SW1A 2AA")).Returns(Task.FromResult(osPlace)!);

            //Act
            //Government Building
            var result = await _sut!.GetOSPlacesTradeLocations("SW1A 2AA");
            //Assert   
            result.Should().NotBeNull();
        }

        [Test]
        public async Task UpdateLogisticsLocationAsync_GivenDTO_ReturnsLogisticsLocationDTO()
        {
            //Arrange
            var guid = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689");
            var logisticsLocationDto = new LogisticsLocationDto();
            var locationSavedInDb = new LogisticsLocation();

            //Arrange
            _mockEstablishmentRepository.Setup(action => action.GetLogisticsLocationByIdAsync(guid)).Returns(Task.FromResult(locationSavedInDb)!);
            _mockEstablishmentRepository.Setup(action => action.UpdateLogisticsLocation(It.IsAny<LogisticsLocation>()));

            //Act
            var result = await _sut!.UpdateLogisticsLocationAsync(guid, logisticsLocationDto);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<LogisticsLocationDto>();
        }

        [Test]
        public async Task UpdateLogisticsLocationAsync_NotFound_ReturnNull()
        {
            //Arrange
            var logisticsLocationDto = new LogisticsLocationDto();
            LogisticsLocation? nullLocation = null;
            _mockEstablishmentRepository.Setup(x => x.GetLogisticsLocationByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(nullLocation);

            //Act
            var result = await _sut!.UpdateLogisticsLocationAsync(Guid.NewGuid(), logisticsLocationDto);

            //Assert
            Assert.IsNull(result);
                
        }

        [Test]
        public async Task GenerateEstablishmentRemosSchemeNumber_ReturnsRemosNumber()
        {
            var tradePartyId = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689");

            var tradeParty = new TradeParty
            {
                RemosBusinessSchemeNumber = "RMS-GB-123456"
            };

            var locations = new List<LogisticsLocation>()
            {
                new LogisticsLocation
                {
                    Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689"),
                },
                   new LogisticsLocation
                {
                    Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689"),
                },
                      new LogisticsLocation
                {
                    Id = Guid.Parse("21afbc5e-51b0-4538-b844-911460c05689"),
                }
            };

            _mockEstablishmentRepository.Setup(action => action.GetAllLogisticsLocationsForTradePartyAsync(tradePartyId)).Returns(Task.FromResult(locations.AsEnumerable()));
            _mockTradePartyRepository.Setup(action => action.GetTradePartyAsync(tradePartyId)).ReturnsAsync(tradeParty);

            var remosNo = await _sut!.GenerateEstablishmentRemosSchemeNumber(tradePartyId);

            remosNo.Should().Be("RMS-GB-123456-004");
        }

        [Test]
        public async Task EstablishmentAlreadyExists_ReturnsFalse_WhenItDoesNotExist()
        {
            //Arrange
            var logisticsLocationAddDto = new LogisticsLocationDto
            {
                Name = "Test Name",
                Id = Guid.NewGuid(),
                Address = new TradeAddressDto { Id = new Guid() }
            };
            _mockEstablishmentRepository.Setup(
                action => action.LogisticsLocationAlreadyExists(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Guid>()))
                .Returns(Task.FromResult(false));

            //Act
            var result = await _sut!.EstablishmentAlreadyExists(logisticsLocationAddDto);

            //Assert
            result.Should().Be(false);
        }

        [Test]
        public async Task EstablishmentAlreadyExists_ReturnsTrue_WhenItExists()
        {
            //Arrange
            var logisticsLocationAddDto = new LogisticsLocationDto
            {
                Name = "Test Name",
                Id = Guid.NewGuid(),
                Address = new TradeAddressDto { Id = new Guid() }
            };
            _mockEstablishmentRepository.Setup(
                action => action.LogisticsLocationAlreadyExists(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Guid>()))
                .Returns(Task.FromResult(true));

            //Act
            var result = await _sut!.EstablishmentAlreadyExists(logisticsLocationAddDto);

            //Assert
            result.Should().Be(true);
        }

        [Test]
        public async Task EstablishmentAlreadyExists_ReturnsFalse_WhenRemoved()
        {
            //Arrange
            var logisticsLocationAddDto = new LogisticsLocationDto
            {
                Name = "Test Name",
                Id = Guid.NewGuid(),
                Address = new TradeAddressDto { Id = Guid.Empty },
                IsRemoved = true
            };
            _mockEstablishmentRepository.Setup(
                action => action.LogisticsLocationAlreadyExists(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Guid>()))
                .Returns(Task.FromResult(false));

            //Act
            var result = await _sut!.EstablishmentAlreadyExists(logisticsLocationAddDto);

            //Assert
            result.Should().Be(false);
        }
    }
}

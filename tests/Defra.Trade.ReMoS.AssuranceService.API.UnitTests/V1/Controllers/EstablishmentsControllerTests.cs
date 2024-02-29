using Defra.Trade.Address.V1.ApiClient.Model;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Defra.Trade.ReMoS.AssuranceService.API.V1.Controllers;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using static Microsoft.Azure.Amqp.Serialization.SerializableType;
#pragma warning disable CS8602
#pragma warning disable CS8629

namespace Defra.Trade.ReMoS.AssuranceService.API.UnitTests.V1.Controllers
{
    public class EstablishmentsControllerTests
    {
        private EstablishmentsController? systemUnderTest;
        readonly Mock<IEstablishmentsService> _mockEstablishmentsService = new();


        [SetUp]
        public void Setup()
        {
            systemUnderTest = new EstablishmentsController(_mockEstablishmentsService.Object);
        }

        [Test]
        public async Task AddEstablishmentReturnsSuccess()
        {
            //arrange
            var logisticsLocation = GetLogisticsLocation();
            var partyId = logisticsLocation.TradePartyId;
            var dto = GenerateLLDTO();
            var expected = systemUnderTest.CreatedAtRoute("GetLogisticsLocationByIdAsync", new { id = logisticsLocation.Id }, logisticsLocation.Id);
            _mockEstablishmentsService
                .Setup(x => x.EstablishmentAlreadyExists(It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(false);
            _mockEstablishmentsService
                .Setup(x => x.AddLogisticsLocationAsync(It.IsAny<Guid>(), It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(logisticsLocation);

            //act
            var results = await systemUnderTest.AddLogisticsLocationToTradePartyAsync(partyId.Value, dto);

            //assert
            results.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task AddEstablishmentReturnsFailure()
        {
            //arrange
            var dto = GenerateLLDTO();
            LogisticsLocationDto? nullLocation = null;

            _mockEstablishmentsService.Setup(x => x.EstablishmentAlreadyExists(It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(false);
            _mockEstablishmentsService.Setup(x => x.AddLogisticsLocationAsync(It.IsAny<Guid>() ,It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(nullLocation);

            //act
            var results = await systemUnderTest.AddLogisticsLocationToTradePartyAsync(dto.TradePartyId.Value, dto);

            //assert
            results.Should().BeEquivalentTo(systemUnderTest.BadRequest("No establishment added"));
        }

        [Test]
        public async Task AddEstablishmentReturnsBadRequestFromException()
        {
            //arrange
            var dto = GenerateLLDTO();
            _mockEstablishmentsService
            .Setup(x => x.EstablishmentAlreadyExists(It.IsAny<LogisticsLocationDto>()))
            .ThrowsAsync(new Exception("Internal error"));

            //act
            var results = await systemUnderTest.AddLogisticsLocationToTradePartyAsync(dto.TradePartyId.Value, dto);

            //assert
            results.Should().BeEquivalentTo(systemUnderTest.BadRequest("Internal error"));
        }

        [Test]
        public async Task AddEstablishment_ReturnsBadRequest_If_Establishment_Exists()
        {
            //arrange
            var logisticsLocation = GetLogisticsLocation();
            var partyId = logisticsLocation.TradePartyId;
            var dto = GenerateLLDTO();
            var expected = systemUnderTest.BadRequest("Establishment already exists");
            _mockEstablishmentsService
                .Setup(x => x.EstablishmentAlreadyExists(It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(true);

            //act
            var results = await systemUnderTest.AddLogisticsLocationToTradePartyAsync(partyId.Value, dto);

            //assert
            results.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAllEstablishmentsReturnsSuccess()
        {
            //arrange
            IEnumerable<LogisticsLocationDto> logisticsList = new List<LogisticsLocationDto>() { GetLogisticsLocation() };
            var expected = systemUnderTest.Ok(logisticsList);
            _mockEstablishmentsService.Setup(x => x.GetAllLogisticsLocationsAsync())
                .ReturnsAsync(logisticsList);

            //act
            var result = await systemUnderTest.GetAllLogisticsLocationsAsync();

            //assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAllEstablishmentsReturnsNotFound()
        {
            //act
            _mockEstablishmentsService.Setup(x => x.GetAllLogisticsLocationsAsync())
            .ReturnsAsync(new List<LogisticsLocationDto>());

            var result = await systemUnderTest.GetAllLogisticsLocationsAsync();

            //assert
            result.Should().BeEquivalentTo(systemUnderTest.NotFound());
        }

        [Test]
        public async Task GetAllEstablishmentsReturnsBadRequest()
        {
            //arrange
            _mockEstablishmentsService.Setup(x => x.GetAllLogisticsLocationsAsync())
                .ThrowsAsync(new Exception());

            //act
            var result = await systemUnderTest.GetAllLogisticsLocationsAsync();

            //assert
            result.Should().BeEquivalentTo(systemUnderTest.BadRequest());
        }

        [Test]
        public async Task GetEstablishmentByIdReturnsSuccess()
        {
            //arrange
            var logisticsLocation = GetLogisticsLocation();
            var expected = systemUnderTest.Ok(logisticsLocation);
            _mockEstablishmentsService.Setup(x => x.GetLogisticsLocationByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(logisticsLocation);

            //act
            var result = await systemUnderTest.GetLogisticsLocationByIdAsync(logisticsLocation.Id);

            //assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetEstablishmentByIdReturnsFailure()
        {
            //arrange
            var id = Guid.NewGuid();
            LogisticsLocationDto? logisticsLocation = null;
            _mockEstablishmentsService.Setup(x => x.GetLogisticsLocationByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(logisticsLocation);

            //act
            var results = await systemUnderTest.GetLogisticsLocationByIdAsync(id);

            //assert
            results.Should().BeEquivalentTo(systemUnderTest.NotFound());
        }

        [Test]

        public async Task GetEstablishmentByIdReturnsBadRequest()
        {
            //arrange
            var id = Guid.NewGuid();
            _mockEstablishmentsService.Setup(x => x.GetLogisticsLocationByIdAsync(It.IsAny<Guid>()))
              .ThrowsAsync(new Exception());

            //act
            var results = await systemUnderTest.GetLogisticsLocationByIdAsync(id);

            //assert
            results.Should().BeEquivalentTo(systemUnderTest.BadRequest());
        }

        [Test]
        public async Task GetEstablishmentByPostcodeReturnsSuccess()
        {
            //arrange
            var logisticsLocation = GetLogisticsLocation();
            var list = new List<LogisticsLocationDto> { logisticsLocation };
            var expected = systemUnderTest.Ok(list);
            _mockEstablishmentsService.Setup(x => x.GetLogisticsLocationsByPostcodeAsync(It.IsAny<string>()))
                .ReturnsAsync(list);

            //act
            var result = await systemUnderTest.GetLogisticsLocationsByPostcodeAsync("aa11 1aa");

            //assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetEstablishmentByPostcodeReturnsNotFound()
        {
            //arrange            
            _mockEstablishmentsService.Setup(x => x.GetLogisticsLocationsByPostcodeAsync(It.IsAny<string>()))
             .ReturnsAsync(new List<LogisticsLocationDto>());

            //act
            var result = await systemUnderTest.GetLogisticsLocationsByPostcodeAsync("aa11 1aa");

            //assert
            result.Should().BeEquivalentTo(systemUnderTest.NotFound("No establishments found"));
        }


        [Test]
        public async Task GetEstablishmentByPostcodeReturnsBadRequest()
        {
            //arrange                                 
            _mockEstablishmentsService.Setup(x => x.GetLogisticsLocationsByPostcodeAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            //act
            var result = await systemUnderTest.GetLogisticsLocationsByPostcodeAsync("aa11 1aa");

            //assert
            result.Should().BeEquivalentTo(systemUnderTest.BadRequest());
        }

        [Test]
        public async Task GetLogisticsLocationsForTradePartyAsyncReturnsSuccess()
        {
            //arrange
            var logisticsLocation = GenerateLLDTO();
            var list = new List<LogisticsLocationDto> { logisticsLocation };
            var expected = systemUnderTest.Ok(list);
            _mockEstablishmentsService.Setup(x => x.GetLogisticsLocationsForTradePartyAsync(It.IsAny<Guid>())).Returns(Task.FromResult(list.AsEnumerable())!);

            //act
            var result = await systemUnderTest.GetLogisticsLocationsForTradePartyAsync(Guid.Empty, false);

            //assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetLogisticsLocationsForTradePartyAsync_ReturnsOk_ForEmptyListOfEstablishments()
        {
            //arrange
            _mockEstablishmentsService.Setup(action => action.GetLogisticsLocationsForTradePartyAsync(It.IsAny<Guid>())).ReturnsAsync(new List<LogisticsLocationDto>());

            //act
            var result = await systemUnderTest.GetLogisticsLocationsForTradePartyAsync(Guid.NewGuid(), false);

            //assert
            result.Should().BeEquivalentTo(systemUnderTest.Ok());
        }

        [Test]
        public async Task GetAllLogisticsLocationsForTradePartyAsync_ReturnsOk_ForEmptyListOfEstablishments()
        {
            //arrange
            _mockEstablishmentsService.Setup(action => action.GetLogisticsLocationsForTradePartyAsync(It.IsAny<Guid>())).ReturnsAsync(new List<LogisticsLocationDto>());

            //act
            var result = await systemUnderTest.GetLogisticsLocationsForTradePartyAsync(Guid.NewGuid(), true);

            //assert
            result.Should().BeEquivalentTo(systemUnderTest.Ok());
        }


        [Test]
        public async Task GetLogisticsLocationsForTradePartyAsyncReturnsBadRequest()
        {
            //arrange
            _mockEstablishmentsService.Setup(x => x.GetLogisticsLocationsForTradePartyAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            //act
            var result = await systemUnderTest.GetLogisticsLocationsForTradePartyAsync(Guid.Empty, false);

            //assert
            result.Should().BeEquivalentTo(systemUnderTest.BadRequest());
        }

        [Test]
        public async Task UpdateLogisticsLocationAsync_Returns_Success()
        {
            //arrange
            var location = GetLogisticsLocation();
            var expected = systemUnderTest.NoContent();
            _mockEstablishmentsService
                .Setup(x => x.EstablishmentAlreadyExists(It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(false);
            _mockEstablishmentsService
                .Setup(x => x.UpdateLogisticsLocationAsync(It.IsAny<Guid>(), It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(location);

            //act
            var result = await systemUnderTest.UpdateLogisticsLocationAsync(Guid.NewGuid(), GetLogisticsLocation());

            //assert
            result.Should().BeEquivalentTo(expected);
        }


        [Test]
        public async Task UpdateLogisticsLocationSelfServeAsync_Returns_Success()
        {
            //arrange
            var location = GetLogisticsLocation();
            var expected = systemUnderTest.NoContent();
            _mockEstablishmentsService
                .Setup(x => x.EstablishmentAlreadyExists(It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(false);
            _mockEstablishmentsService
                .Setup(x => x.UpdateLogisticsLocationSelfServeAsync(It.IsAny<Guid>(), It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(location);

            //act
            var result = await systemUnderTest.UpdateLogisticsLocationSelfServeAsync(Guid.NewGuid(), GetLogisticsLocation());

            //assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task UpdateLogisticsLocationSelfServeAsync_Returns_Success_For_Update()
        {
            //arrange
            var location = GetLogisticsLocation();
            location.ApprovalStatus = Domain.Enums.LogisticsLocationApprovalStatus.Removed;
            var expected = systemUnderTest.NoContent();
            _mockEstablishmentsService
                .Setup(x => x.EstablishmentAlreadyExists(It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(false);
            _mockEstablishmentsService
                .Setup(x => x.UpdateLogisticsLocationSelfServeAsync(It.IsAny<Guid>(), It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(location);

            //act
            var result = await systemUnderTest.UpdateLogisticsLocationSelfServeAsync(Guid.NewGuid(), GetLogisticsLocation());

            //assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task UpdateEstablishment_ReturnsBadRequest_If_Establishment_Exists()
        {
            //arrange
            var logisticsLocation = GetLogisticsLocation();
            var partyId = logisticsLocation.TradePartyId;
            var dto = GenerateLLDTO();
            var expected = systemUnderTest.BadRequest("Establishment already exists");
            _mockEstablishmentsService
                .Setup(x => x.EstablishmentAlreadyExists(It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(true);

            //act
            var results = await systemUnderTest.UpdateLogisticsLocationAsync(partyId.Value, dto);

            //assert
            results.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task UpdateEstablishmentSelfServe_ReturnsBadRequest_If_Establishment_Exists()
        {
            //arrange
            var logisticsLocation = GetLogisticsLocation();
            var partyId = logisticsLocation.TradePartyId;
            var dto = GenerateLLDTO();
            var expected = systemUnderTest.BadRequest("Establishment already exists");
            _mockEstablishmentsService
                .Setup(x => x.EstablishmentAlreadyExists(It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(true);

            //act
            var results = await systemUnderTest.UpdateLogisticsLocationSelfServeAsync(partyId.Value, dto);

            //assert
            results.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task UpdateLogisticsLocationAsync_Returns_NotFound()
        {
            //arrange
            LogisticsLocationDto? location = null;
            _mockEstablishmentsService
                .Setup(x => x.EstablishmentAlreadyExists(It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(false);
            _mockEstablishmentsService
                .Setup(x => x.UpdateLogisticsLocationAsync(It.IsAny<Guid>(), It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(location);

            //act
            var result = await systemUnderTest.UpdateLogisticsLocationAsync(Guid.NewGuid(), GetLogisticsLocation());

            //assert
            result.Should().BeEquivalentTo(systemUnderTest.NotFound());
        }

        [Test]
        public async Task UpdateLogisticsLocationSelfServeAsync_Returns_NotFound()
        {
            //arrange
            LogisticsLocationDto? location = null;
            _mockEstablishmentsService
                .Setup(x => x.EstablishmentAlreadyExists(It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(false);
            _mockEstablishmentsService
                .Setup(x => x.UpdateLogisticsLocationSelfServeAsync(It.IsAny<Guid>(), It.IsAny<LogisticsLocationDto>()))
                .ReturnsAsync(location);

            //act
            var result = await systemUnderTest.UpdateLogisticsLocationSelfServeAsync(Guid.NewGuid(), GetLogisticsLocation());

            //assert
            result.Should().BeEquivalentTo(systemUnderTest.NotFound());
        }

        [Test]
        public async Task UpdateLogisticsLocationAsync_Returns_BadRequest()
        {
            //arrange                        
            _mockEstablishmentsService
                .Setup(x => x.EstablishmentAlreadyExists(It.IsAny<LogisticsLocationDto>()))
                .ThrowsAsync(new Exception());

            //act
            var result = await systemUnderTest.UpdateLogisticsLocationAsync(Guid.NewGuid(), GetLogisticsLocation());

            //assert
            result.Should().BeEquivalentTo(systemUnderTest.BadRequest());
        }

        [Test]
        public async Task UpdateLogisticsLocationSelfServeAsync_Returns_BadRequest()
        {
            //arrange                        
            _mockEstablishmentsService
                .Setup(x => x.EstablishmentAlreadyExists(It.IsAny<LogisticsLocationDto>()))
                .ThrowsAsync(new Exception());

            //act
            var result = await systemUnderTest.UpdateLogisticsLocationSelfServeAsync(Guid.NewGuid(), GetLogisticsLocation());

            //assert
            result.Should().BeEquivalentTo(systemUnderTest.BadRequest());
        }

        [Test]
        public async Task RemoveEstablishmentReturnsNotFound()
        {
            //arrange
            var logisticsLocation = GetLogisticsLocation();
            var partyId = logisticsLocation.TradePartyId;
            var expected = systemUnderTest.NotFound();
            _mockEstablishmentsService.Setup(x => x.RemoveLogisticsLocationAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

            //act
            var results = await systemUnderTest.RemoveEstablishmentAsync(partyId.Value);

            //assert
            results.Should().BeEquivalentTo(expected);
        }


        [Test]
        public async Task RemoveEstablishmentReturnsNoContent()
        {
            //arrange
            var logisticsLocation = GetLogisticsLocation();
            var partyId = logisticsLocation.TradePartyId;
            _mockEstablishmentsService.Setup(x => x.RemoveLogisticsLocationAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            //act
            var results = await systemUnderTest.RemoveEstablishmentAsync(partyId.Value);

            //assert
            results.Should().BeEquivalentTo(systemUnderTest.NoContent());
        }

        [Test]
        public async Task RemoveEstablishmentReturnsBadRequest()
        {
            //arrange
            var logisticsLocation = GetLogisticsLocation();
            var partyId = logisticsLocation.TradePartyId;
            _mockEstablishmentsService.Setup(x => x.RemoveLogisticsLocationAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            //act
            var results = await systemUnderTest.RemoveEstablishmentAsync(partyId.Value);

            //assert
            results.Should().BeEquivalentTo(systemUnderTest.BadRequest());
        }

        public static LogisticsLocationDto GenerateLLDTO()
        {
            return new LogisticsLocationDto()
            {
                Name = "test ltd",
                NI_GBFlag = "GB",
                TradePartyId = Guid.NewGuid(),
                TradeAddressId = GetAddress().Id,
                Party = new TradePartyDto()
            };
        }

        public static LogisticsLocationDto GetLogisticsLocation()
        {
            return new LogisticsLocationDto()
            {
                Id = Guid.NewGuid(),
                Name = "test ltd",
                TradePartyId = Guid.NewGuid(),
                TradeAddressId = GetAddress().Id,
                Address = GetAddress(),
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                NI_GBFlag = "GBFlag"
            };
        }

        public static TradeAddressDto GetAddress()
        {
            return new TradeAddressDto()
            {
                Id = Guid.NewGuid(),
                LineOne = "test lane ltd",
                LineTwo = "test town",
                PostCode = "AA11 1AA",
                TradeCountry = "GB"
            };
        }

        [Test]
        public void GetTradeAddressApiLocations_ReturnsSuccess()
        {
            // arrange
            var postCode = "G31 4EB";
            var addressesDto = new List<AddressDto>()
            {
                new AddressDto("1234", null, null, null, null, null, postCode)
            };
            var expected = systemUnderTest.Ok(addressesDto);
            _mockEstablishmentsService.Setup(x => x.GetTradeAddressApiByPostcode(postCode)).Returns(addressesDto);

            // act
            var result = systemUnderTest.GetTradeAddressApiLocations(postCode);

            // assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetTradeAddressApiLocations_ReturnsNotFound()
        {
            // arrange
            var postCode = "G31 4EB";
            List<AddressDto>? addressesDto = null;
            var expected = systemUnderTest.NotFound();
            _mockEstablishmentsService.Setup(x => x.GetTradeAddressApiByPostcode(postCode)).Returns(addressesDto!);

            // act
            var result = systemUnderTest.GetTradeAddressApiLocations(postCode);

            // assert
            result.Should().BeEquivalentTo(expected);
        }


        [Test]
        public void GetTradeAddressApiLocations_ReturnsBadRequest()
        {
            // arrange
            var postCode = "G31 4EB";            
            _mockEstablishmentsService.Setup(x => x.GetTradeAddressApiByPostcode(postCode))
                .Throws(new Exception());

            // act
            var result = systemUnderTest.GetTradeAddressApiLocations(postCode);

            // assert
            result.Should().BeEquivalentTo(systemUnderTest.BadRequest());
        }

        [Test]
        public void GetLogisticsLocationByUprnReturnsSuccess()
        {
            // arrange
            var uprn = "1234";
            var logisticsLocationDto = GetLogisticsLocation();
            var expected = systemUnderTest.Ok(logisticsLocationDto);
            _mockEstablishmentsService.Setup(x => x.GetLogisticsLocationByUprnAsync(uprn)).Returns(logisticsLocationDto);

            // act
            var result = systemUnderTest.GetLogisticsLocationByUprn(uprn);

            // assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetLogisticsLocationByUprn_ReturnsNotFound()
        {
            // arrange
            var uprn = "1234";
            LogisticsLocationDto? logisticsLocationDto = null;
            var expected = systemUnderTest.NotFound();
            _mockEstablishmentsService.Setup(x => x.GetLogisticsLocationByUprnAsync(uprn)).Returns(logisticsLocationDto!);

            // act
            var result = systemUnderTest.GetLogisticsLocationByUprn(uprn);

            // assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetLogisticsLocationByUprn_ReturnsBadRequest()
        {
            // arrange
            var uprn = "1234";                        
            _mockEstablishmentsService.Setup(x => x.GetLogisticsLocationByUprnAsync(uprn))
                .Throws(new Exception());

            // act
            var result = systemUnderTest.GetLogisticsLocationByUprn(uprn);

            // assert
            result.Should().BeEquivalentTo(systemUnderTest.BadRequest());
        }

    }
}

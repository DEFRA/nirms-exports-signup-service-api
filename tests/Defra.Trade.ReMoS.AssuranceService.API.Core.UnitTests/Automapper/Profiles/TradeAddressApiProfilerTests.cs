using AutoMapper;
using Defra.Trade.Address.V1.ApiClient.Model;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Profiles;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.UnitTests.Automapper.Profiles;

public class TradeAddressApiProfilerTests
{
    private IMapper? _mapper;

    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<TradeAddressApiProfiler>());
        _mapper = config.CreateMapper();
    }

    [Test]
    public void TradeAddressApiConverter_ConvertsSimple()
    {
        // arrange
        var source = new AddressDto("1234", null, null, null, null, null, "postcode")
        {
            Address = "business name, some, other, string, street, town, postcode",
            ThroughfareName = "street",
            PostTown = "town",
            Postcode = "postcode",
            DependantLocality = null
        };
        var destination = new TradeAddressAndBusinessNameDto()
        {
            BusinessName = "business name",
            TradeAddress = new TradeAddressDto()
            {
                LineOne = "some other string",
                LineTwo = "street",
                CityName = "town",
                PostCode = "postcode"
            }
        };

        // act
        var result = _mapper?.Map<TradeAddressAndBusinessNameDto>(source);

        // assert
        result.Should().BeEquivalentTo(destination);
    }

    [Test]
    public void TradeAddressApiConverter_ConvertsSimpleLocality()
    {
        // arrange
        var source = new AddressDto("1234", null, null, null, null, null, "postcode")
        {
            Address = "business name, some, other, string, street, locality, town, postcode",
            ThroughfareName = "street",
            PostTown = "town",
            Postcode = "postcode",
            DependantLocality = "locality"
        };
        var destination = new TradeAddressAndBusinessNameDto()
        {
            BusinessName = "business name",
            TradeAddress = new TradeAddressDto()
            {
                LineOne = "some other string",
                LineTwo = "street locality",
                CityName = "town",
                PostCode = "postcode"
            }
        };

        // act
        var result = _mapper?.Map<TradeAddressAndBusinessNameDto>(source);

        // assert
        result.Should().BeEquivalentTo(destination);
    }

    [Test]
    public void TradeAddressApiConverter_ConvertsLocality()
    {
        // arrange
        var source = new AddressDto("1234", null, null, null, null, null, "postcode")
        {
            Address = "12, street, locality, town, postcode",
            ThroughfareName = "street",
            PostTown = "town",
            Postcode = "postcode",
            DependantLocality = "locality"
        };
        var destination = new TradeAddressAndBusinessNameDto()
        {
            BusinessName = null,
            TradeAddress = new TradeAddressDto()
            {
                LineOne = "12 street",
                LineTwo = "locality",
                CityName = "town",
                PostCode = "postcode"
            }
        };

        // act
        var result = _mapper?.Map<TradeAddressAndBusinessNameDto>(source);

        // assert
        result.Should().BeEquivalentTo(destination);
    }

    [Test]
    public void TradeAddressApiConverter_ConvertsHouse()
    {
        // arrange
        var source = new AddressDto("1234", null, null, null, null, null, "postcode")
        {
            Address = "12, street, town, postcode",
            ThroughfareName = "street",
            PostTown = "town",
            Postcode = "postcode",
            DependantLocality = null
        };
        var destination = new TradeAddressAndBusinessNameDto()
        {
            BusinessName = null,
            TradeAddress = new TradeAddressDto()
            {
                LineOne = "12 street",
                LineTwo = null,
                CityName = "town",
                PostCode = "postcode"
            }
        };

        // act
        var result = _mapper?.Map<TradeAddressAndBusinessNameDto>(source);

        // assert
        result.Should().BeEquivalentTo(destination);
    }

    [Test]
    public void TradeAddressApiConverter_ConvertsFlat()
    {
        // arrange
        var source = new AddressDto("1234", null, null, null, null, null, "postcode")
        {
            Address = "flat 1, 12, street, town, postcode",
            ThroughfareName = "street",
            PostTown = "town",
            Postcode = "postcode",
            DependantLocality = null
        };
        var destination = new TradeAddressAndBusinessNameDto()
        {
            BusinessName = "flat 1",
            TradeAddress = new TradeAddressDto()
            {
                LineOne = "12 street",
                LineTwo = null,
                CityName = "town",
                PostCode = "postcode"
            }
        };

        // act
        var result = _mapper?.Map<TradeAddressAndBusinessNameDto>(source);

        // assert
        result.Should().BeEquivalentTo(destination);
    }

    [Test]
    public void TradeAddressApiConverter_ConvertsBusiness()
    {
        // arrange
        var source = new AddressDto("1234", null, null, null, null, null, "postcode")
        {
            Address = "business name, flat 1, 12, street, town, postcode",
            ThroughfareName = "street",
            PostTown = "town",
            Postcode = "postcode",
            DependantLocality = null
        };
        var destination = new TradeAddressAndBusinessNameDto()
        {
            BusinessName = "business name",
            TradeAddress = new TradeAddressDto()
            {
                LineOne = "flat 1",
                LineTwo = "12 street",
                CityName = "town",
                PostCode = "postcode"
            }
        };

        // act
        var result = _mapper?.Map<TradeAddressAndBusinessNameDto>(source);

        // assert
        result.Should().BeEquivalentTo(destination);
    }

    [Test]
    public void TradeAddressApiConverter_ConvertsBuilding()
    {
        // arrange
        var source = new AddressDto("1234", null, null, null, null, null, "postcode")
        {
            Address = "business name, building name, street, town, postcode",
            ThroughfareName = "street",
            PostTown = "town",
            Postcode = "postcode",
            DependantLocality = null
        };
        var destination = new TradeAddressAndBusinessNameDto()
        {
            BusinessName = "business name",
            TradeAddress = new TradeAddressDto()
            {
                LineOne = "building name",
                LineTwo = "street",
                CityName = "town",
                PostCode = "postcode"
            }
        };

        // act
        var result = _mapper?.Map<TradeAddressAndBusinessNameDto>(source);

        // assert
        result.Should().BeEquivalentTo(destination);
    }

    [Test]
    public void TradeAddressApiConverter_ConvertsBusinessNameAndBuildingNo()
    {
        // arrange
        var source = new AddressDto("1234", null, null, null, null, null, "postcode")
        {
            Address = "business name, 1A, street, town, postcode",
            BuildingName = "1A",
            ThroughfareName = "street",
            PostTown = "town",
            Postcode = "postcode",
            DependantLocality = null
            //Address = "Oven Door, 90, Castle Street, Belfast, BT1 1HE",
            //BuildingName = "90",
            //ThroughfareName = "Castle Street",
            //PostTown = "Belfast",
            //Postcode = "BT1 1HE",
            //DependantLocality = null
        };
        var destination = new TradeAddressAndBusinessNameDto()
        {
            BusinessName = "business name",
            TradeAddress = new TradeAddressDto()
            {
                LineOne = "1A street",
                CityName = "town",
                PostCode = "postcode"
            }
        };

        // act
        var result = _mapper?.Map<TradeAddressAndBusinessNameDto>(source);

        // assert
        result.Should().BeEquivalentTo(destination);
    }
}
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.UnitTests.Entities
{
    public class OsPlacesTests
    {

        [Test]
        public void SetOsPlace_GivenValidValues_FieldsSetToGivenValues()
        {
            //Arrange
            var date = DateTime.UtcNow;

            var osPlaces = new OsPlaces
            {
                //Act
                 Header = new OsPlacesHeader
                 {
                    dataset = "test header",
                    epoch = "test epoch",
                    filter = "test filter",
                    format = "test format",
                    lr = "test lr",
                    maxresults = 2, 
                    offset = 6, 
                    output_srs = "test output srs",
                    query = "test query",
                    totalresults = 2, 
                    uri = "test uri"
                 },
                 Results = new List<OsPlacesResult>
                 {
                     new OsPlacesResult {
                         Dpa = new Dpa{
                              ADDRESS = "test address",
                              BLPU_STATE_CODE = "test blpu state code",
                              BLPU_STATE_CODE_DESCRIPTION = "test blpu code description",
                              BLPU_STATE_DATE = "test blpu state date",
                              BUILDING_NAME = "test building name",
                              CLASSIFICATION_CODE = "test classification code", 
                              CLASSIFICATION_CODE_DESCRIPTION = "classification code description",
                              DEPENDENT_LOCALITY = "test dependent locality", 
                              Easting = .2M,
                              ENTRY_DATE = "test entry date",
                              GridReference = "test grid reference",
                              LANGUAGE = "test language",
                              LAST_UPDATE_DATE = "test last update date",
                              LOCAL_CUSTODIAN_CODE = 4,
                              LOCAL_CUSTODIAN_CODE_DESCRIPTION = "test local custodian code description",
                              LOGICAL_STATUS_CODE = "test local status code",
                              MATCH = 6,
                              MATCH_DESCRIPTION = "test match description",
                              Northing = .5M,
                              PARENT_UPRN = "test parent uprn",
                              POSTAL_ADDRESS_CODE = "test postal address code",
                              POSTAL_ADDRESS_CODE_DESCRIPTION = "test postal address code description",
                              POSTCODE = "test postcode",
                              POST_TOWN = "test post town",
                              RPC = "test rpc",
                              STATUS = "test status",
                              THOROUGHFARE_NAME = "test throughfare name",
                              TOPOGRAPHY_LAYER_TOID = "test topograpgt layer to id",
                              UPRN = "test uprn",
                              X_COORDINATE = .1, 
                              Y_COORDINATE = .3
                         }
                     }
                 }
            };

            osPlaces.Header.dataset.Should().Be("test header");
            osPlaces.Header.epoch.Should().Be("test epoch");
            osPlaces.Header.filter.Should().Be("test filter");
            osPlaces.Header.format.Should().Be("test format");
            osPlaces.Header.lr.Should().Be("test lr");
            osPlaces.Header.maxresults.Should().Be(2);
            osPlaces.Header.offset.Should().Be(6);
            osPlaces.Header.output_srs.Should().Be("test output srs");
            osPlaces.Header.query.Should().Be("test query");
            osPlaces.Header.totalresults.Should().Be(2);
            osPlaces.Header.uri.Should().Be("test uri");

            osPlaces.Results.First().Dpa!.ADDRESS.Should().Be("test address");
            osPlaces.Results.First().Dpa!.BLPU_STATE_CODE.Should().Be("test blpu state code");
            osPlaces.Results.First().Dpa!.BLPU_STATE_CODE_DESCRIPTION.Should().Be("test blpu code description");
            osPlaces.Results.First().Dpa!.BLPU_STATE_DATE.Should().Be("test blpu state date");
            osPlaces.Results.First().Dpa!.BUILDING_NAME.Should().Be("test building name");
            osPlaces.Results.First().Dpa!.CLASSIFICATION_CODE.Should().Be("test classification code");
            osPlaces.Results.First().Dpa!.CLASSIFICATION_CODE_DESCRIPTION.Should().Be("classification code description");
            osPlaces.Results.First().Dpa!.DEPENDENT_LOCALITY.Should().Be("test dependent locality");
            osPlaces.Results.First().Dpa!.Easting.Should().Be(.2M);
            osPlaces.Results.First().Dpa!.ENTRY_DATE.Should().Be("test entry date");
            osPlaces.Results.First().Dpa!.GridReference.Should().Be("test grid reference");
            osPlaces.Results.First().Dpa!.LANGUAGE.Should().Be("test language");
            osPlaces.Results.First().Dpa!.LAST_UPDATE_DATE.Should().Be("test last update date");
            osPlaces.Results.First().Dpa!.LOCAL_CUSTODIAN_CODE.Should().Be(4);
            osPlaces.Results.First().Dpa!.LOCAL_CUSTODIAN_CODE_DESCRIPTION.Should().Be("test local custodian code description");
            osPlaces.Results.First().Dpa!.LOGICAL_STATUS_CODE.Should().Be("test local status code");
            osPlaces.Results.First().Dpa!.MATCH.Should().Be(6);
            osPlaces.Results.First().Dpa!.MATCH_DESCRIPTION.Should().Be("test match description");
            osPlaces.Results.First().Dpa!.Northing.Should().Be(.5M);
            osPlaces.Results.First().Dpa!.PARENT_UPRN.Should().Be("test parent uprn");
            osPlaces.Results.First().Dpa!.POSTAL_ADDRESS_CODE.Should().Be("test postal address code");
            osPlaces.Results.First().Dpa!.POSTAL_ADDRESS_CODE_DESCRIPTION.Should().Be("test postal address code description");
            osPlaces.Results.First().Dpa!.POSTCODE.Should().Be("test postcode");
            osPlaces.Results.First().Dpa!.POST_TOWN.Should().Be("test post town");
            osPlaces.Results.First().Dpa!.RPC.Should().Be("test rpc");
            osPlaces.Results.First().Dpa!.STATUS.Should().Be("test status");
            osPlaces.Results.First().Dpa!.THOROUGHFARE_NAME.Should().Be("test throughfare name");
            osPlaces.Results.First().Dpa!.TOPOGRAPHY_LAYER_TOID.Should().Be("test topograpgt layer to id");
            osPlaces.Results.First().Dpa!.UPRN.Should().Be("test uprn");
            osPlaces.Results.First().Dpa!.X_COORDINATE.Should().Be(.1);
            osPlaces.Results.First().Dpa!.Y_COORDINATE.Should().Be(.3);
        }
    }
}

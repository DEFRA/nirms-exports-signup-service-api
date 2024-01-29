using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities
{
    public class Dpa
    {
        public string? UPRN { get; set; }
        public string? ADDRESS { get; set; }
        public string? BUILDING_NAME { get; set; }
        public string? THOROUGHFARE_NAME { get; set; }
        public string? DEPENDENT_LOCALITY { get; set; }
        public string? POST_TOWN { get; set; }
        public string? POSTCODE { get; set; }
        public string? RPC { get; set; }
        public double X_COORDINATE { get; set; }
        public double Y_COORDINATE { get; set; }
        public string? STATUS { get; set; }
        public string? LOGICAL_STATUS_CODE { get; set; }
        public string? CLASSIFICATION_CODE { get; set; }
        public string? CLASSIFICATION_CODE_DESCRIPTION { get; set; }
        public int LOCAL_CUSTODIAN_CODE { get; set; }
        public string? LOCAL_CUSTODIAN_CODE_DESCRIPTION { get; set; }
        public string? POSTAL_ADDRESS_CODE { get; set; }
        public string? POSTAL_ADDRESS_CODE_DESCRIPTION { get; set; }
        public string? BLPU_STATE_CODE { get; set; }
        public string? BLPU_STATE_CODE_DESCRIPTION { get; set; }
        public string? TOPOGRAPHY_LAYER_TOID { get; set; }
        public string? PARENT_UPRN { get; set; }
        public string? LAST_UPDATE_DATE { get; set; }
        public string? ENTRY_DATE { get; set; }
        public string? BLPU_STATE_DATE { get; set; }
        public string? LANGUAGE { get; set; }
        public double MATCH { get; set; }
        public string? MATCH_DESCRIPTION { get; set; }
        public string? GridReference { get; set; }
        public decimal Northing { get; set; }
        public decimal Easting { get; set; }
    }
}

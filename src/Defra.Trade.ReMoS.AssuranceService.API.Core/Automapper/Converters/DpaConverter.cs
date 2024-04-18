using AutoMapper;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Defra.Trade.ReMoS.AssuranceService.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Converters
{
    [ExcludeFromCodeCoverage]
    public class DpaConverter : ITypeConverter<Dpa, TradeAddressDto>
    {
        private readonly List<string> _englandPostcodes = new()
        {
            "AL", "CB", "CM", "CO", "EN", "IG", "IP", "LU", "MK", "NR", "PE", "RM", "SG", "SS", "WD", "B", "CV", 
            "DE", "DY", "LE", "NG", "NN", "ST", "WS", "WV", "BD", "DH", "DL", "DN", "HD", "HG", "HU", "HX", "LN", "LS", "NE", "S", "SR", "TS", "WF", "YO", "BB", "BL", 
            "CA", "CW", "FY", "L", "LA", "M", "OL", "PR", "SK", "SY", "TF", "WA", "WN", "CH", "E", "EC", "N", "NW", "SE", "SW", "W", "WC", "GU", "HA", "HP", "OX", 
            "PO", "RG", "SL", "SN", "SO", "SP", "UB", "BN", "BR", "CR", "CT", "DA", "KT", "ME", "RH", "SM", "TN", "TW", "BA", "BH", "BS", "DT", "EX", "GL", "HR", 
            "PL", "TA", "TQ", "TR", "WR"};
        private readonly List<string> _scotlandPostCodes = new() { "AB", "DD", "DG", "EH", "FG", "G", "HS", "IV", "KA", "KW", "KY", "ML", "PA", "PH", "TD", "ZE"};
        private readonly List<string> _walesPostCodes = new() { "CF", "LD", "LL", "NP", "SA"};
        private readonly List<string> _northernIrelandPostCodes = new() { "BT" };
        private readonly List<string> _guernseyPostCodes = new() { "GY" };
        private readonly List<string> _jerseyPostCodes = new() { "JE" };
        private readonly List<string> _isleOfManPostCodes = new() { "IM" };

        public TradeAddressDto Convert(Dpa source, TradeAddressDto destination, ResolutionContext context)
        {
            var addressString = source.ADDRESS!.Split(',').ToList();

            TradeAddressDto tradeAddress = new()
            {
                PostCode = source.POSTCODE,
                CityName = source.POST_TOWN,
                TradeCountry = FindCountryByPostCode(source.POSTCODE!)
            };

            //These are cases like Government Buildings
            if (addressString.Count > 4 && source.POSTCODE == addressString[4].Trim() && !int.TryParse(addressString[0], out _) && int.TryParse(addressString[1], out _))
            {
                tradeAddress.LineOne = addressString[0].Trim();
                tradeAddress.LineTwo = addressString[1].Trim() + " " + addressString[2].Trim();
            }
            //these are cases like Apartments
            else if(int.TryParse(addressString[0], out _) && int.TryParse(addressString[2], out _) && source.POSTCODE != addressString[3].Trim())
            {
                tradeAddress.LineOne = addressString[0].Trim() + " " + addressString[1].Trim();
                tradeAddress.LineTwo = addressString[2].Trim() + " " + addressString[3].Trim();
            }
            //These are regular street addreses
            else
            {
                tradeAddress.LineOne = addressString[0].Trim() + " " + addressString[1].Trim();
                tradeAddress.LineTwo = addressString[2].Trim();
            }

            return tradeAddress;
        }

        private string FindCountryByPostCode(string postCode)
        {
            var postcodeRegion = Regex.Match(postCode, @"^[^0-9]*", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(60)).Value;

            return postCode switch
            {
                "England" when _englandPostcodes.Contains(postcodeRegion) => "England",
                "Scotland" when _scotlandPostCodes.Contains(postcodeRegion) => "Scotland",
                "Wales" when _walesPostCodes.Contains(postcodeRegion) => "Wales",
                "Northern Ireland" when _northernIrelandPostCodes.Contains(postcodeRegion) => "Northern Ireland",
                "Guernsey" when _guernseyPostCodes.Contains(postcodeRegion) => "Guernsey",
                "Jersey" when _jerseyPostCodes.Contains(postcodeRegion) => "Jersey",
                "Isle of Man" when _isleOfManPostCodes.Contains(postcodeRegion) => "Isle of Man",
                _ => "",
            };
        }
    }
}

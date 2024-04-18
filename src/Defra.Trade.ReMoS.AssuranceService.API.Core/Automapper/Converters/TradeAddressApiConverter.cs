using AutoMapper;
using Defra.Trade.Address.V1.ApiClient.Model;
using Defra.Trade.ReMoS.AssuranceService.Shared.DTO;
using System.Text;
using System.Text.RegularExpressions;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Converters
{
    public class TradeAddressApiConverter : ITypeConverter<AddressDto, TradeAddressAndBusinessNameDto>
    {
        public TradeAddressAndBusinessNameDto Convert(AddressDto source, TradeAddressAndBusinessNameDto destinantion, ResolutionContext context)
        {
            var addressString = source.Address!.Split(',').ToList();

            TradeAddressAndBusinessNameDto tradeAddress = new()
            {
                BusinessName = addressString[0],
                TradeAddress = new()
                {
                    PostCode = source.Postcode,
                    CityName = source.PostTown,
                    LineTwo = source.ThroughfareName
                }
            };

            var pattern = @"(?<=" + addressString[0].Trim() + @",\s).*(?=,\s*" + source.ThroughfareName + ")";
            Match m = Regex.Match(source.Address, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(60));

            var pattern2 = @".*(?=,\s" + source.ThroughfareName + ")";
            Match m2 = Regex.Match(source.Address, pattern2, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(60));
            var ad = m2.Value.Split(", ");

            if (int.TryParse(ad[^1], out int houseNumber)) // house number
            {
                for (int i = 1; i < ad.Length - 1; i++) // lineOne is everything after buiness name to house number
                {
                    if (i != (ad.Length - 2)) tradeAddress.TradeAddress.LineOne = tradeAddress.TradeAddress.LineOne + ad[i] + " ";
                    else tradeAddress.TradeAddress.LineOne += ad[i];
                }
                tradeAddress.TradeAddress.LineTwo = houseNumber.ToString() + " " + source.ThroughfareName; // lineTwo is house number and street name

                if (houseNumber.ToString() == tradeAddress.BusinessName) // set business name to null for simple street address
                {
                    tradeAddress.BusinessName = null;
                }
            }
            else tradeAddress.TradeAddress.LineOne = m.Value.Replace(",", ""); // lineOne is everything after business name to street name

            if (tradeAddress.TradeAddress.LineOne == m2.Value.Split()[^1]) tradeAddress.TradeAddress.LineOne = null; // if lineOne is just house number remove it

            if (tradeAddress.TradeAddress.LineOne == null || tradeAddress.TradeAddress.LineOne == string.Empty) // move lineTwo to lineOne if its empty
            {
                tradeAddress.TradeAddress.LineOne = tradeAddress.TradeAddress.LineTwo;
                tradeAddress.TradeAddress.LineTwo = string.Empty;
            }
            if (source.BuildingName != null)
            {
                tradeAddress.TradeAddress.LineOne = source.BuildingName + " " + tradeAddress.TradeAddress.LineOne;
            }
            if (source.DependantLocality != null)
            {
                if (tradeAddress.TradeAddress.LineTwo == null || tradeAddress.TradeAddress.LineTwo == "") tradeAddress.TradeAddress.LineTwo = source.DependantLocality;
                else tradeAddress.TradeAddress.LineTwo += $" {source.DependantLocality}";
            }

            CheckAndAdjustAddressLength(tradeAddress.TradeAddress);

            if (tradeAddress.TradeAddress.LineTwo == string.Empty) tradeAddress.TradeAddress.LineTwo = null;

            return tradeAddress;
        }

        //There is a requirement to have Adress Line lengths to be 50 characters or less. This is handled for user entry on the UI, but values from the
        //Api should be managed here. Line 2 over 50 chars will need to be manually edited
        private static void CheckAndAdjustAddressLength(TradeAddressDto tradeAddressDto)
        {
            var maxLineLength = 50;
            tradeAddressDto.LineOne ??= "";
            tradeAddressDto.LineTwo ??= "";

            if (tradeAddressDto.LineOne!.Length > maxLineLength || tradeAddressDto.LineTwo!.Length > maxLineLength)
            {
                var totalAddressLines = tradeAddressDto.LineOne + " " + tradeAddressDto.LineTwo;
                var words = totalAddressLines.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                StringBuilder bld1 = new();
                StringBuilder bld2 = new();

                var charCount = 0;
                foreach (var word in words)
                {
                    if (charCount < maxLineLength)
                    {
                        if (bld1.Length != 0)
                            bld1.Append(' ');

                        bld1.Append(word);
                    }
                    else
                    {
                        if (bld2.Length != 0)
                            bld2.Append(' ');

                        bld2.Append(word);
                    }
                    charCount += word.Length + 1;
                }

                tradeAddressDto.LineOne = bld1.ToString();
                tradeAddressDto.LineTwo = bld2.ToString();
            }
        }
    }
}
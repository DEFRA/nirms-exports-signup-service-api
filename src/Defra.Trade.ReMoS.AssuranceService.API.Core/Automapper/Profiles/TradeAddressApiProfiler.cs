using AutoMapper;
using Defra.Trade.Address.V1.ApiClient.Model;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Converters;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Profiles;

public class TradeAddressApiProfiler : Profile
{
    public TradeAddressApiProfiler()
    {
        CreateMap<AddressDto, TradeAddressAndBusinessNameDto>()
            .ConvertUsing<TradeAddressApiConverter>();
        CreateMap<TradeAddressAndBusinessNameDto, AddressDto>();
    }
}

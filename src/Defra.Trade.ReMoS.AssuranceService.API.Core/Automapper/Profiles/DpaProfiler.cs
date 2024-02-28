using AutoMapper;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Converters;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Profiles;

public class DpaProfiler : Profile
{
    public DpaProfiler()
    {
        CreateMap<Dpa, TradeAddressDto>()
            .ConvertUsing<DpaConverter>();
        CreateMap<TradeAddressDto, Dpa>();
    }
}

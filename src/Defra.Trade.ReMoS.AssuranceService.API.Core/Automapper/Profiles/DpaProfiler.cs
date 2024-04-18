using AutoMapper;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Converters;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Defra.Trade.ReMoS.AssuranceService.Shared.DTO;

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

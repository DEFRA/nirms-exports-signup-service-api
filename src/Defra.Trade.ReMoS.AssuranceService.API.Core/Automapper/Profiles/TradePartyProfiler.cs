using AutoMapper;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Converters;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Profiles;

public class TradePartyProfiler : Profile
{
	public TradePartyProfiler()
	{
		CreateMap<TradePartyDto, TradeParty>()
			.ConvertUsing<TradePartiesConverter>();
		CreateMap<TradeParty, TradePartyDto>()
            .ConvertUsing<TradePartiesDtoConverter>();
        CreateMap<TradeAddressDto, TradeAddress>();
        CreateMap<TradeAddress, TradeAddressDto>();		
    }
}

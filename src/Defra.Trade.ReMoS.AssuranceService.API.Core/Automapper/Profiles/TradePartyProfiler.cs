using AutoMapper;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Converters;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

using AutoMapper;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Defra.Trade.ReMoS.AssuranceService.Shared.DTO;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Profiles;

public class LogisticsLocationProfiler : Profile
{
    public LogisticsLocationProfiler()
    {
        CreateMap<LogisticsLocationDto, LogisticsLocation>()
            .ForMember(x => x.Party, opt => opt.Ignore());
        CreateMap<LogisticsLocation, LogisticsLocationDto>()
            .ForMember(x => x.Party, opt => opt.Ignore());
    }
}

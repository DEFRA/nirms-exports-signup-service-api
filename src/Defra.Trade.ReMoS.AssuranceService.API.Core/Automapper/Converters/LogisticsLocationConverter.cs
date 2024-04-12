using AutoMapper;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Automapper.Converters;

[ExcludeFromCodeCoverage]
public class LogisticsLocationConverter : ITypeConverter<LogisticsLocationDto, LogisticsLocation>
{
    public LogisticsLocation Convert(LogisticsLocationDto source, LogisticsLocation dest, ResolutionContext context)
    {
        LogisticsLocation logisticsLocation = new();
        if (dest != null)
        {
            logisticsLocation = dest;
        }

        logisticsLocation.Id = (source.Id != Guid.Empty) ? source.Id : logisticsLocation.Id;
        logisticsLocation.Name ??= source.Name;
        logisticsLocation.NI_GBFlag ??= source.NI_GBFlag;
        logisticsLocation.ApprovalStatus = AssignValues(logisticsLocation.ApprovalStatus, source.ApprovalStatus);
        logisticsLocation.InspectionLocationId = source.InspectionLocationId;

        return logisticsLocation;
    }

    public static T? AssignValues<T>(T oldVal, T newVal)
    {
        if (newVal != null)
        {
            return newVal;
        }
        if (oldVal != null)
        {
            return oldVal;
        }
        return default;
    }
}

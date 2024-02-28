using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Configurations;

[ExcludeFromCodeCoverage]
public class TradeAddressConfiguration : IEntityTypeConfiguration<TradeAddress>
{
    public void Configure(EntityTypeBuilder<TradeAddress> builder)
    {
        builder
           .HasKey(p => p.Id);
        builder.Property(p => p.LineOne)
            .IsRequired(false);
        builder.Property(p => p.LineTwo)
            .IsRequired(false);
        builder.Property(p => p.LineThree)
            .IsRequired(false);
        builder.Property(p => p.LineFour)
            .IsRequired(false);
        builder.Property(p => p.LineFive)
            .IsRequired(false);
        builder.Property(p => p.CityName)
            .IsRequired(false);
        builder.Property(p => p.PostCode)
            .IsRequired(false);
        builder.Property(s => s.TradeCountry)
            .IsRequired(false);
            
    }
}

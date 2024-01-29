using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Configurations;

[ExcludeFromCodeCoverage]
public class TradeContactConfiguration : IEntityTypeConfiguration<TradeContact>
{
    public void Configure(EntityTypeBuilder<TradeContact> builder)
    {
        builder
           .HasKey(p => p.Id);
        builder.Property(p => p.TradePartyId)
            .IsRequired();
        builder.Property(p => p.PersonName).IsRequired(false);
        builder.Property(p => p.TelephoneNumber).IsRequired(false);
        builder.Property(p => p.Email).IsRequired(false);
        builder.Property(p => p.Position).IsRequired(false);
        builder.Property(p => p.IsAuthorisedSignatory).IsRequired(false);
        builder.Property(p => p.SubmittedDate).IsRequired();
        builder.Property(p => p.LastModifiedDate).IsRequired();
        builder.Property(p => p.ModifiedBy).IsRequired();
    }
}

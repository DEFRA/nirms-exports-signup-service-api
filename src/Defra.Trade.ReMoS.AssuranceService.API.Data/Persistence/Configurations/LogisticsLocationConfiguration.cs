using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Configurations;

[ExcludeFromCodeCoverage]
public class LogisticsLocationConfiguration : IEntityTypeConfiguration<LogisticsLocation>
{
    public void Configure(EntityTypeBuilder<LogisticsLocation> builder)
    {
        builder
           .HasKey(p => p.Id);
        builder
            .Property(p => p.Name)
            .IsRequired(false);
        builder
            .Property(p => p.TradeAddressId)
            .IsRequired(false);
        builder
            .Property(p => p.CreatedDate)
            .IsRequired(true);
        builder
            .Property(p => p.LastModifiedDate)
            .IsRequired(true);
        builder
            .Property(p => p.LastModifiedDate)
            .IsRequired(true);
        builder.HasOne(p => p.Address)
            .WithOne()
            .HasForeignKey<LogisticsLocation>(t => t.TradeAddressId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(l => l.Party)
            .WithMany(p => p.LogisticsLocations)
            .HasForeignKey(l => l.TradePartyId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Property(t => t.RemosEstablishmentSchemeNumber)
    .IsRequired(false).HasColumnType("nvarchar(17)")
    .HasMaxLength(17).IsUnicode(false);
    }
}


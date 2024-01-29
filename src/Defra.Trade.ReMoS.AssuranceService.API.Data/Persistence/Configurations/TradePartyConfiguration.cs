using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Configurations;

[ExcludeFromCodeCoverage]
public class TradePartyConfiguration : IEntityTypeConfiguration<TradeParty>
{
    public void Configure(EntityTypeBuilder<TradeParty> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(t => t.Name).IsRequired(false);
        builder.Property(t => t.NatureOfBusiness).IsRequired(false);
        builder.Property(t => t.FboNumber).IsRequired(false).HasColumnType("nvarchar(25)");
        builder.Property(t => t.PhrNumber).IsRequired(false).HasColumnType("nvarchar(25)");
        builder.Property(t => t.FboPhrOption).IsRequired(false).HasColumnType("varchar(5)");
        builder.Property(t => t.AssuranceCommitmentSignedDate);
        builder.Property(t => t.TermsAndConditionsSignedDate);
        builder.Property(t => t.CreatedDate);
        builder.Property(t => t.LastUpdateDate);
        builder.Property(t => t.RemosBusinessSchemeNumber)
            .IsRequired(false).HasColumnType("nvarchar(13)")
            .HasMaxLength(13).IsUnicode(false);
        builder.HasOne(s => s.TradeAddress)
            .WithOne()
            .HasForeignKey<TradeParty>(t => t.TradeAddressId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(s => s.TradeContact)
            .WithOne()
            .HasForeignKey<TradeContact>(tc => tc.TradePartyId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(a => a.AuthorisedSignatory)
            .WithOne()
            .HasForeignKey<AuthorisedSignatory>(a => a.TradePartyId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

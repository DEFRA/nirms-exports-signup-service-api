using Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Configurations
{
    [ExcludeFromCodeCoverage]
    public class AuthorisedSignatoryConfiguration : IEntityTypeConfiguration<AuthorisedSignatory>
    {
        public void Configure(EntityTypeBuilder<AuthorisedSignatory> builder)
        {
            builder
               .HasKey(p => p.Id);
            builder.Property(p => p.TradePartyId)
                .IsRequired();
            builder.Property(p => p.Name).IsRequired(false);
            builder.Property(p => p.EmailAddress).IsRequired(false);
            builder.Property(p => p.Position).IsRequired(false);
            builder.Property(p => p.SubmittedDate).IsRequired();
            builder.Property(p => p.LastModifiedDate).IsRequired();
            builder.Property(p => p.ModifiedBy).IsRequired();
        }
    }
}

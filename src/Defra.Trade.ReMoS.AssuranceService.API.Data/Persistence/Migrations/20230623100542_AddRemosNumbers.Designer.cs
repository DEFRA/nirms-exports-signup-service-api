﻿// <auto-generated />
using System;
using Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230623100542_AddRemosNumbers")]
    partial class AddRemosNumbers
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.LogisticLocationBusinessRelationship", b =>
                {
                    b.Property<Guid>("RelationshipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContactEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LogisticsLocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TradePartyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RelationshipId");

                    b.HasIndex("LogisticsLocationId");

                    b.HasIndex("TradePartyId");

                    b.ToTable("LogisticsLocationBusinessRelationships");
                });

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.LogisticsLocation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("NI_GBFlag")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TradeAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TradeAddressId")
                        .IsUnique()
                        .HasFilter("[TradeAddressId] IS NOT NULL");

                    b.ToTable("LogisticsLocation");
                });

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CityName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LineFive")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LineFour")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LineOne")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LineThree")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LineTwo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TradeCountry")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TradeAddresses");
                });

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeContact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelephoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TradePartyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TradePartyId")
                        .IsUnique();

                    b.ToTable("TradeContacts");
                });

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeParty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FboNumber")
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("RemosBusinessSchemeNumber")
                        .HasColumnType("nvarchar(13)");


                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NatureOfBusiness")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TradeAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TradeAddressId")
                        .IsUnique()
                        .HasFilter("[TradeAddressId] IS NOT NULL");

                    b.ToTable("TradeParties");
                });

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.LogisticLocationBusinessRelationship", b =>
                {
                    b.HasOne("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.LogisticsLocation", "LogisticsLocation")
                        .WithMany("EstablishmentBusinessRelationships")
                        .HasForeignKey("LogisticsLocationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeParty", "TradeParty")
                        .WithMany("EstablishmentBusinessRelationships")
                        .HasForeignKey("TradePartyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("LogisticsLocation");

                    b.Navigation("TradeParty");
                });

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.LogisticsLocation", b =>
                {
                    b.HasOne("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeAddress", "Address")
                        .WithOne()
                        .HasForeignKey("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.LogisticsLocation", "TradeAddressId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeContact", b =>
                {
                    b.HasOne("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeParty", null)
                        .WithOne("TradeContact")
                        .HasForeignKey("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeContact", "TradePartyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeParty", b =>
                {
                    b.HasOne("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeAddress", "TradeAddress")
                        .WithOne()
                        .HasForeignKey("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeParty", "TradeAddressId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("TradeAddress");
                });

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.LogisticsLocation", b =>
                {
                    b.Navigation("EstablishmentBusinessRelationships");
                });

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeParty", b =>
                {
                    b.Navigation("EstablishmentBusinessRelationships");

                    b.Navigation("TradeContact");
                });
#pragma warning restore 612, 618
        }
    }
}

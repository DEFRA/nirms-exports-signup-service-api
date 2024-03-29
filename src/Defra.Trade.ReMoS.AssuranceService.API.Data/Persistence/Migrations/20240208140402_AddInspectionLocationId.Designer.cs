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
    [Migration("20240208140402_AddInspectionLocationId")]
    partial class AddInspectionLocationId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.HasSequence<int>("RemosGbIdHelper", "dbo");

            modelBuilder.HasSequence<int>("RemosNiIdHelper", "dbo");

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.AuthorisedSignatory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubmittedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TradePartyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TradePartyId")
                        .IsUnique();

                    b.ToTable("AuthorisedSignatory");
                });

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.LogisticsLocation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ApprovalStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("InspectionLocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("NI_GBFlag")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RemosEstablishmentSchemeNumber")
                        .HasMaxLength(17)
                        .IsUnicode(false)
                        .HasColumnType("nvarchar(17)");

                    b.Property<Guid?>("TradeAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TradePartyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TradeAddressId")
                        .IsUnique()
                        .HasFilter("[TradeAddressId] IS NOT NULL");

                    b.HasIndex("TradePartyId");

                    b.ToTable("LogisticsLocation");
                });

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CityName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("County")
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

                    b.Property<bool?>("IsAuthorisedSignatory")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PersonName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubmittedDate")
                        .HasColumnType("datetime2");

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

                    b.Property<int>("ApprovalStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("AssuranceCommitmentSignedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FboNumber")
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("FboPhrOption")
                        .HasColumnType("varchar(5)");

                    b.Property<DateTime>("LastUpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NatureOfBusiness")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OrgId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PhrNumber")
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("PracticeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("RegulationsConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RemosBusinessSchemeNumber")
                        .HasMaxLength(13)
                        .IsUnicode(false)
                        .HasColumnType("nvarchar(13)");

                    b.Property<Guid>("SignUpRequestSubmittedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("TermsAndConditionsSignedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("TradeAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TradeAddressId")
                        .IsUnique()
                        .HasFilter("[TradeAddressId] IS NOT NULL");

                    b.ToTable("TradeParties");
                });

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.AuthorisedSignatory", b =>
                {
                    b.HasOne("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeParty", null)
                        .WithOne("AuthorisedSignatory")
                        .HasForeignKey("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.AuthorisedSignatory", "TradePartyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.LogisticsLocation", b =>
                {
                    b.HasOne("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeAddress", "Address")
                        .WithOne()
                        .HasForeignKey("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.LogisticsLocation", "TradeAddressId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeParty", "Party")
                        .WithMany("LogisticsLocations")
                        .HasForeignKey("TradePartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Party");
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

            modelBuilder.Entity("Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities.TradeParty", b =>
                {
                    b.Navigation("AuthorisedSignatory");

                    b.Navigation("LogisticsLocations");

                    b.Navigation("TradeContact");
                });
#pragma warning restore 612, 618
        }
    }
}

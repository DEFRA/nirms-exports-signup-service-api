using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradeAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineOne = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineThree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineFour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineFive = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradeCountry = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogisticsLocation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradeAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NI_GBFlag = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogisticsLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogisticsLocation_TradeAddresses_TradeAddressId",
                        column: x => x.TradeAddressId,
                        principalTable: "TradeAddresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TradeParties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfBusiness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradeAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeParties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeParties_TradeAddresses_TradeAddressId",
                        column: x => x.TradeAddressId,
                        principalTable: "TradeAddresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LogisticsLocationBusinessRelationships",
                columns: table => new
                {
                    RelationshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TradePartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogisticsLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogisticsLocationBusinessRelationships", x => x.RelationshipId);
                    table.ForeignKey(
                        name: "FK_LogisticsLocationBusinessRelationships_LogisticsLocation_LogisticsLocationId",
                        column: x => x.LogisticsLocationId,
                        principalTable: "LogisticsLocation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LogisticsLocationBusinessRelationships_TradeParties_TradePartyId",
                        column: x => x.TradePartyId,
                        principalTable: "TradeParties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TradeContacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TradePartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeContacts_TradeParties_TradePartyId",
                        column: x => x.TradePartyId,
                        principalTable: "TradeParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogisticsLocation_TradeAddressId",
                table: "LogisticsLocation",
                column: "TradeAddressId",
                unique: true,
                filter: "[TradeAddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LogisticsLocationBusinessRelationships_LogisticsLocationId",
                table: "LogisticsLocationBusinessRelationships",
                column: "LogisticsLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LogisticsLocationBusinessRelationships_TradePartyId",
                table: "LogisticsLocationBusinessRelationships",
                column: "TradePartyId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeContacts_TradePartyId",
                table: "TradeContacts",
                column: "TradePartyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TradeParties_TradeAddressId",
                table: "TradeParties",
                column: "TradeAddressId",
                unique: true,
                filter: "[TradeAddressId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogisticsLocationBusinessRelationships");

            migrationBuilder.DropTable(
                name: "TradeContacts");

            migrationBuilder.DropTable(
                name: "LogisticsLocation");

            migrationBuilder.DropTable(
                name: "TradeParties");

            migrationBuilder.DropTable(
                name: "TradeAddresses");
        }
    }
}

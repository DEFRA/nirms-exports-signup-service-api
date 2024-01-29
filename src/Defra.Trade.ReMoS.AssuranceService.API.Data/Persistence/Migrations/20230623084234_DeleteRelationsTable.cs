using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class DeleteRelationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogisticsLocationBusinessRelationships");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "LogisticsLocation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TradePartyId",
                table: "LogisticsLocation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_LogisticsLocation_TradePartyId",
                table: "LogisticsLocation",
                column: "TradePartyId");
            
            //Manual - clear data before adding FK to avoid conflicts
            migrationBuilder.Sql($"DELETE FROM LogisticsLocation");

            migrationBuilder.AddForeignKey(
                name: "FK_LogisticsLocation_TradeParties_TradePartyId",
                table: "LogisticsLocation",
                column: "TradePartyId",
                principalTable: "TradeParties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogisticsLocation_TradeParties_TradePartyId",
                table: "LogisticsLocation");

            migrationBuilder.DropIndex(
                name: "IX_LogisticsLocation_TradePartyId",
                table: "LogisticsLocation");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "LogisticsLocation");

            migrationBuilder.DropColumn(
                name: "TradePartyId",
                table: "LogisticsLocation");

            migrationBuilder.CreateTable(
                name: "LogisticsLocationBusinessRelationships",
                columns: table => new
                {
                    RelationshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogisticsLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TradePartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_LogisticsLocationBusinessRelationships_LogisticsLocationId",
                table: "LogisticsLocationBusinessRelationships",
                column: "LogisticsLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LogisticsLocationBusinessRelationships_TradePartyId",
                table: "LogisticsLocationBusinessRelationships",
                column: "TradePartyId");
        }
    }
}

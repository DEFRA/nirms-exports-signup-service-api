using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AddAuthorisedSignatoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAuthorisedSignatory",
                table: "TradeContacts",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuthorisedSignatory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TradePartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorisedSignatory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorisedSignatory_TradeParties_TradePartyId",
                        column: x => x.TradePartyId,
                        principalTable: "TradeParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorisedSignatory_TradePartyId",
                table: "AuthorisedSignatory",
                column: "TradePartyId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorisedSignatory");

            migrationBuilder.DropColumn(
                name: "IsAuthorisedSignatory",
                table: "TradeContacts");
        }
    }
}

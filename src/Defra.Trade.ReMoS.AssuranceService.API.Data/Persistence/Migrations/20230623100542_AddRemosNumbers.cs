using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AddRemosNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence("RemosGbIdHelper", "dbo", 000001, 1, 000000, 999999, false);

            migrationBuilder.CreateSequence("RemosNiIdHelper", "dbo", 000001, 1, 000000, 999999, false);

            migrationBuilder.AddColumn<string>(
                name: "RemosBusinessSchemeNumber",
                table: "TradeParties",
                type: "nvarchar(13)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence("RemosGbIdHelper");

            migrationBuilder.DropSequence("RemosNiIdHelper");

            migrationBuilder.DropColumn(
                name: "RemosBusinessSchemeNumber",
                table: "TradeParties");
        }
    }
}

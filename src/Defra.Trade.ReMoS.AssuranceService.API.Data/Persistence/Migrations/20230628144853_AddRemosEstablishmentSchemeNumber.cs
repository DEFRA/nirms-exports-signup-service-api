using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AddRemosEstablishmentSchemeNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.AddColumn<string>(
                name: "RemosEstablishmentSchemeNumber",
                table: "LogisticsLocation",
                type: "nvarchar(17)",
                unicode: false,
                maxLength: 17,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {            
            migrationBuilder.DropColumn(
                name: "RemosEstablishmentSchemeNumber",
                table: "LogisticsLocation");
        }
    }
}

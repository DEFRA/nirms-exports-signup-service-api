using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AddDefraOrgIdAndName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrgId",
                table: "TradeParties",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.AddColumn<string>(
                name: "PracticeName",
                table: "TradeParties",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "TradeParties");

            migrationBuilder.DropColumn(
                name: "PracticeName",
                table: "TradeParties");
        }
    }
}

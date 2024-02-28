using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class TradePartyDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AssuranceCommitmentSignedDate",
                table: "TradeParties",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "TradeParties",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "TradeParties",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TermsAndConditionsSignedDate",
                table: "TradeParties",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssuranceCommitmentSignedDate",
                table: "TradeParties");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "TradeParties");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "TradeParties");

            migrationBuilder.DropColumn(
                name: "TermsAndConditionsSignedDate",
                table: "TradeParties");
        }
    }
}

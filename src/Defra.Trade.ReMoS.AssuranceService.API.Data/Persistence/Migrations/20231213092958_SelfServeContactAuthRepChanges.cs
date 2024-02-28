using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class SelfServeContactAuthRepChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "TradeContacts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "TradeContacts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedDate",
                table: "TradeContacts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "AuthorisedSignatory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "AuthorisedSignatory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedDate",
                table: "AuthorisedSignatory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "TradeContacts");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "TradeContacts");

            migrationBuilder.DropColumn(
                name: "SubmittedDate",
                table: "TradeContacts");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "AuthorisedSignatory");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "AuthorisedSignatory");

            migrationBuilder.DropColumn(
                name: "SubmittedDate",
                table: "AuthorisedSignatory");
        }
    }
}

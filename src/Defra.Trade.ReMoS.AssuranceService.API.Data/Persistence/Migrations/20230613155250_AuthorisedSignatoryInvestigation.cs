using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace Defra.Trade.ReMoS.AssuranceService.API.Data.Persistence.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class AuthorisedSignatoryInvestigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorisedSignatory_TradeParties_TradePartyId",
                table: "AuthorisedSignatory");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeContacts_TradeParties_TradePartyId",
                table: "TradeContacts");

            migrationBuilder.DropIndex(
                name: "IX_TradeContacts_TradePartyId",
                table: "TradeContacts");

            migrationBuilder.DropIndex(
                name: "IX_AuthorisedSignatory_TradePartyId",
                table: "AuthorisedSignatory");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorisedSignatory_TradeParties_Id",
                table: "AuthorisedSignatory",
                column: "Id",
                principalTable: "TradeParties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeContacts_TradeParties_Id",
                table: "TradeContacts",
                column: "Id",
                principalTable: "TradeParties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorisedSignatory_TradeParties_Id",
                table: "AuthorisedSignatory");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeContacts_TradeParties_Id",
                table: "TradeContacts");

            migrationBuilder.CreateIndex(
                name: "IX_TradeContacts_TradePartyId",
                table: "TradeContacts",
                column: "TradePartyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthorisedSignatory_TradePartyId",
                table: "AuthorisedSignatory",
                column: "TradePartyId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorisedSignatory_TradeParties_TradePartyId",
                table: "AuthorisedSignatory",
                column: "TradePartyId",
                principalTable: "TradeParties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeContacts_TradeParties_TradePartyId",
                table: "TradeContacts",
                column: "TradePartyId",
                principalTable: "TradeParties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

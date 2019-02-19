using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelStay.Data.Migrations
{
    public partial class AdminUserSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isSuperAdmin",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "SalesPersonId",
                table: "Reservations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SalesPersonId",
                table: "Reservations",
                column: "SalesPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_SalesPersonId",
                table: "Reservations",
                column: "SalesPersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_SalesPersonId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_SalesPersonId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "SalesPersonId",
                table: "Reservations");

            migrationBuilder.AddColumn<bool>(
                name: "isSuperAdmin",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}

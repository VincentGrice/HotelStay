using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelStay.Data.Migrations
{
    public partial class @fixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomTags_SpecialTagsID",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "SpecialTagsID",
                table: "Rooms",
                newName: "RoomTagsID");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_SpecialTagsID",
                table: "Rooms",
                newName: "IX_Rooms_RoomTagsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomTags_RoomTagsID",
                table: "Rooms",
                column: "RoomTagsID",
                principalTable: "RoomTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomTags_RoomTagsID",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "RoomTagsID",
                table: "Rooms",
                newName: "SpecialTagsID");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_RoomTagsID",
                table: "Rooms",
                newName: "IX_Rooms_SpecialTagsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomTags_SpecialTagsID",
                table: "Rooms",
                column: "SpecialTagsID",
                principalTable: "RoomTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

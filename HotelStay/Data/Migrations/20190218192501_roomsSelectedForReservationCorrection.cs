using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HotelStay.Data.Migrations
{
    public partial class roomsSelectedForReservationCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomsSelectedForAppointments");

            migrationBuilder.CreateTable(
                name: "RoomsSelectedForReservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReservationId = table.Column<int>(nullable: false),
                    RoomId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsSelectedForReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomsSelectedForReservations_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomsSelectedForReservations_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomsSelectedForReservations_ReservationId",
                table: "RoomsSelectedForReservations",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsSelectedForReservations_RoomId",
                table: "RoomsSelectedForReservations",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomsSelectedForReservations");

            migrationBuilder.CreateTable(
                name: "RoomsSelectedForAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReservationId = table.Column<int>(nullable: false),
                    RoomId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsSelectedForAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomsSelectedForAppointments_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomsSelectedForAppointments_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomsSelectedForAppointments_ReservationId",
                table: "RoomsSelectedForAppointments",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsSelectedForAppointments_RoomId",
                table: "RoomsSelectedForAppointments",
                column: "RoomId");
        }
    }
}

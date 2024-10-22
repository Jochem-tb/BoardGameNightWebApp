using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BGN.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changed_Attendees_To_Cascade_OnDelete_GameNight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameNightAttendees_GameNights_GameNightId",
                table: "GameNightAttendees");

            migrationBuilder.DropForeignKey(
                name: "FK_GameNightAttendees_Persons_AttendeesId",
                table: "GameNightAttendees");

            migrationBuilder.AddForeignKey(
                name: "FK_GameNightAttendees_GameNights_GameNightId",
                table: "GameNightAttendees",
                column: "GameNightId",
                principalTable: "GameNights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameNightAttendees_Persons_AttendeesId",
                table: "GameNightAttendees",
                column: "AttendeesId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameNightAttendees_GameNights_GameNightId",
                table: "GameNightAttendees");

            migrationBuilder.DropForeignKey(
                name: "FK_GameNightAttendees_Persons_AttendeesId",
                table: "GameNightAttendees");

            migrationBuilder.AddForeignKey(
                name: "FK_GameNightAttendees_GameNights_GameNightId",
                table: "GameNightAttendees",
                column: "GameNightId",
                principalTable: "GameNights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameNightAttendees_Persons_AttendeesId",
                table: "GameNightAttendees",
                column: "AttendeesId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

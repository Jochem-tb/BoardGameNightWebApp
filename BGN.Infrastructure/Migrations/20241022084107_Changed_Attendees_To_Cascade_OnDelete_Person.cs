using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BGN.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changed_Attendees_To_Cascade_OnDelete_Person : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameNightAttendees_GameNights_GameNightId",
                table: "GameNightAttendees");

            migrationBuilder.AddForeignKey(
                name: "FK_GameNightAttendees_GameNights_GameNightId",
                table: "GameNightAttendees",
                column: "GameNightId",
                principalTable: "GameNights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameNightAttendees_GameNights_GameNightId",
                table: "GameNightAttendees");

            migrationBuilder.AddForeignKey(
                name: "FK_GameNightAttendees_GameNights_GameNightId",
                table: "GameNightAttendees",
                column: "GameNightId",
                principalTable: "GameNights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

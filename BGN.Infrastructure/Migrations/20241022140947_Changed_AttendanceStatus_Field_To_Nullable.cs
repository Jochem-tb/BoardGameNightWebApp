using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BGN.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changed_AttendanceStatus_Field_To_Nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "AttendanceStatus",
                table: "Attendees",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "AttendanceStatus",
                table: "Attendees",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}

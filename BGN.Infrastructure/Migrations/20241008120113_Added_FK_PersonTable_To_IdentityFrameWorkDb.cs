using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BGN.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_FK_PersonTable_To_IdentityFrameWorkDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Persons",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Persons");
        }
    }
}

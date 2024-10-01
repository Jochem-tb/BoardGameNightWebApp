using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BGN.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Genres_GenreId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Genders_GenderId",
                table: "Persons");

            migrationBuilder.AlterColumn<int>(
                name: "GenderId",
                table: "Persons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GenreId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Partyspel" },
                    { 2, "Rollenspel" },
                    { 3, "Kaartspel" },
                    { 4, "Bordspel" },
                    { 5, "Kinderspel" }
                });

            migrationBuilder.InsertData(
                table: "FoodOptions",
                columns: new[] { "Id", "GameNightId", "Name", "PersonId" },
                values: new object[,]
                {
                    { 1, null, "Lactose", null },
                    { 2, null, "Alcohol", null },
                    { 3, null, "Vegetarisch", null },
                    { 4, null, "Veganistisch", null },
                    { 5, null, "Glutenvrij", null },
                    { 6, null, "Noten", null }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "M" },
                    { 2, "V" },
                    { 3, "X" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Geen" },
                    { 2, "Coöperatief" },
                    { 3, "Strategie" },
                    { 4, "Tactiek" },
                    { 5, "Abstract" },
                    { 6, "Familie" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Genres_GenreId",
                table: "Games",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Genders_GenderId",
                table: "Persons",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Genres_GenreId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Genders_GenderId",
                table: "Persons");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FoodOptions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FoodOptions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FoodOptions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "FoodOptions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FoodOptions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FoodOptions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.AlterColumn<int>(
                name: "GenderId",
                table: "Persons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GenreId",
                table: "Games",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Genres_GenreId",
                table: "Games",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Genders_GenderId",
                table: "Persons",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "Id");
        }
    }
}

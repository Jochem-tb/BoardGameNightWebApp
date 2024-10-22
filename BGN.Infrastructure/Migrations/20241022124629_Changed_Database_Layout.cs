using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BGN.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changed_Database_Layout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    GenderId = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    HouseNr = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MinPlayers = table.Column<int>(type: "int", nullable: false),
                    MaxPlayers = table.Column<int>(type: "int", nullable: false),
                    IsAdult = table.Column<bool>(type: "bit", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    EstimatedTime = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodOptionsPerson",
                columns: table => new
                {
                    PersonsId = table.Column<int>(type: "int", nullable: false),
                    PreferencesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodOptionsPerson", x => new { x.PersonsId, x.PreferencesId });
                    table.ForeignKey(
                        name: "FK_FoodOptionsPerson_FoodOptions_PreferencesId",
                        column: x => x.PreferencesId,
                        principalTable: "FoodOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodOptionsPerson_Persons_PersonsId",
                        column: x => x.PersonsId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameNights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    OrganiserId = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    HouseNr = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaxPlayers = table.Column<int>(type: "int", nullable: false),
                    OnlyAdultWelcome = table.Column<bool>(type: "bit", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameNights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameNights_Persons_OrganiserId",
                        column: x => x.OrganiserId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendees",
                columns: table => new
                {
                    GameNightId = table.Column<int>(type: "int", nullable: false),
                    AttendeeId = table.Column<int>(type: "int", nullable: false),
                    AttendanceStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendees", x => new { x.GameNightId, x.AttendeeId });
                    table.ForeignKey(
                        name: "FK_Attendees_GameNights_GameNightId",
                        column: x => x.GameNightId,
                        principalTable: "GameNights",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attendees_Persons_AttendeeId",
                        column: x => x.AttendeeId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FoodOptionsGameNight",
                columns: table => new
                {
                    FoodOptionsId = table.Column<int>(type: "int", nullable: false),
                    GameNightsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodOptionsGameNight", x => new { x.FoodOptionsId, x.GameNightsId });
                    table.ForeignKey(
                        name: "FK_FoodOptionsGameNight_FoodOptions_FoodOptionsId",
                        column: x => x.FoodOptionsId,
                        principalTable: "FoodOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodOptionsGameNight_GameNights_GameNightsId",
                        column: x => x.GameNightsId,
                        principalTable: "GameNights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameGameNight",
                columns: table => new
                {
                    GameNightsId = table.Column<int>(type: "int", nullable: false),
                    GamesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGameNight", x => new { x.GameNightsId, x.GamesId });
                    table.ForeignKey(
                        name: "FK_GameGameNight_GameNights_GameNightsId",
                        column: x => x.GameNightsId,
                        principalTable: "GameNights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGameNight_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Lactose" },
                    { 2, "Alcohol" },
                    { 3, "Vegetarisch" },
                    { 4, "Veganistisch" },
                    { 5, "Glutenvrij" },
                    { 6, "Noten" }
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
                    { 2, "Coöperatief" },
                    { 3, "Strategie" },
                    { 4, "Tactiek" },
                    { 5, "Abstract" },
                    { 6, "Familie" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendees_AttendeeId",
                table: "Attendees",
                column: "AttendeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodOptionsGameNight_GameNightsId",
                table: "FoodOptionsGameNight",
                column: "GameNightsId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodOptionsPerson_PreferencesId",
                table: "FoodOptionsPerson",
                column: "PreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_GameGameNight_GamesId",
                table: "GameGameNight",
                column: "GamesId");

            migrationBuilder.CreateIndex(
                name: "IX_GameNights_OrganiserId",
                table: "GameNights",
                column: "OrganiserId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_CategoryId",
                table: "Games",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GenreId",
                table: "Games",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_GenderId",
                table: "Persons",
                column: "GenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendees");

            migrationBuilder.DropTable(
                name: "FoodOptionsGameNight");

            migrationBuilder.DropTable(
                name: "FoodOptionsPerson");

            migrationBuilder.DropTable(
                name: "GameGameNight");

            migrationBuilder.DropTable(
                name: "FoodOptions");

            migrationBuilder.DropTable(
                name: "GameNights");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Genders");
        }
    }
}

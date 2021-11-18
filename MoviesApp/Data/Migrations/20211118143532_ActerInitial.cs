using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoviesApp.Data.Migrations
{
    public partial class ActerInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Acters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    BirthdayDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActerMovies",
                columns: table => new
                {
                    ActerId = table.Column<int>(nullable: false),
                    MovieId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActerMovies", x => new { x.ActerId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_ActerMovies_Acters_ActerId",
                        column: x => x.ActerId,
                        principalTable: "Acters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActerMovies_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActerMovies_MovieId",
                table: "ActerMovies",
                column: "MovieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActerMovies");

            migrationBuilder.DropTable(
                name: "Acters");
        }
    }
}

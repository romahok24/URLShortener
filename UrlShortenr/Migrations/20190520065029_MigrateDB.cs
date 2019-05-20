using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UrlShortenr.Migrations
{
    public partial class MigrateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Urls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    LongUrl = table.Column<string>(maxLength: 1000, nullable: false),
                    ShortUrl = table.Column<string>(maxLength: 20, nullable: false),
                    Added = table.Column<DateTime>(nullable: false),
                    Ip = table.Column<string>(maxLength: 50, nullable: false),
                    ClicksCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    ClickDate = table.Column<DateTime>(nullable: false),
                    Ip = table.Column<string>(maxLength: 50, nullable: false),
                    Referer = table.Column<string>(maxLength: 500, nullable: false),
                    UrlId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stats_Urls_UrlId",
                        column: x => x.UrlId,
                        principalTable: "Urls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stats_UrlId",
                table: "Stats",
                column: "UrlId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stats");

            migrationBuilder.DropTable(
                name: "Urls");
        }
    }
}

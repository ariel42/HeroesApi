using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HeroesApi.Data.Migrations
{
    public partial class HeroesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Heroes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TrainerId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsAttacker = table.Column<bool>(nullable: false),
                    IsDefender = table.Column<bool>(nullable: false),
                    FirstTrainingDate = table.Column<DateTime>(nullable: true),
                    LastTrainingDate = table.Column<DateTime>(nullable: true),
                    NumTrainingAtLastDate = table.Column<int>(nullable: false),
                    SuitPart1Color = table.Column<string>(nullable: true),
                    SuitPart2Color = table.Column<string>(nullable: true),
                    SuitPart3Color = table.Column<string>(nullable: true),
                    StartingPower = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    CurrentPower = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Heroes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Heroes");
        }
    }
}

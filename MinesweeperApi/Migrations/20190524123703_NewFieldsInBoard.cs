using Microsoft.EntityFrameworkCore.Migrations;

namespace MinesweeperApi.Migrations
{
    public partial class NewFieldsInBoard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DateCreated",
                table: "GameBoards",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "GameBoards",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "GameBoards");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "GameBoards");
        }
    }
}

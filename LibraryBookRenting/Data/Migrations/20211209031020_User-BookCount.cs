using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryBookRenting.Data.Migrations
{
    public partial class UserBookCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantityKeep",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityKeep",
                table: "AspNetUsers");
        }
    }
}

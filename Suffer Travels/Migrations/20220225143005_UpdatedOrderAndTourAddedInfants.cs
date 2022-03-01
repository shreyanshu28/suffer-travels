using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Suffer_Travels.Migrations
{
    public partial class UpdatedOrderAndTourAddedInfants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceChildren",
                table: "tblTour",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceInfant",
                table: "tblTour",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "TotalChildrens",
                table: "tblOrderMaster",
                type: "Int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TotalInfants",
                table: "tblOrderMaster",
                type: "Int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceChildren",
                table: "tblTour");

            migrationBuilder.DropColumn(
                name: "PriceInfant",
                table: "tblTour");

            migrationBuilder.DropColumn(
                name: "TotalInfants",
                table: "tblOrderMaster");

            migrationBuilder.AlterColumn<int>(
                name: "TotalChildrens",
                table: "tblOrderMaster",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Int");
        }
    }
}

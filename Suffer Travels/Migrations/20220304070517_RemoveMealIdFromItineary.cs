using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Suffer_Travels.Migrations
{
    public partial class RemoveMealIdFromItineary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MealComboId",
                table: "tblTourItinerary");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "tblOrderMaster",
                type: "Decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "tblOrderMaster");

            migrationBuilder.AddColumn<long>(
                name: "MealComboId",
                table: "tblTourItinerary",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}

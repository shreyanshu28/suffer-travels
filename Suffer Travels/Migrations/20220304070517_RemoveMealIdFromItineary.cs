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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MealComboId",
                table: "tblTourItinerary",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}

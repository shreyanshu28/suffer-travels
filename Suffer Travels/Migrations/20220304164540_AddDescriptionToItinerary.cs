using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Suffer_Travels.Migrations
{
    public partial class AddDescriptionToItinerary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LandmarkId",
                table: "tblTourItinerary");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "tblTourItinerary",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "tblLandMark",
                columns: table => new
                {
                    LId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    PhotoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblLandMark", x => x.LId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblLandMark");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "tblTourItinerary");

            migrationBuilder.AddColumn<long>(
                name: "LandmarkId",
                table: "tblTourItinerary",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}

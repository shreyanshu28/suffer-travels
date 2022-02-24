using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Suffer_Travels.Migrations
{
    public partial class UpdatedOrderTotalAdultsChildrens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPeople",
                table: "tblOrderMaster",
                newName: "TotalAdults");

            migrationBuilder.AddColumn<int>(
                name: "TotalChildrens",
                table: "tblOrderMaster",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalChildrens",
                table: "tblOrderMaster");

            migrationBuilder.RenameColumn(
                name: "TotalAdults",
                table: "tblOrderMaster",
                newName: "TotalPeople");
        }
    }
}

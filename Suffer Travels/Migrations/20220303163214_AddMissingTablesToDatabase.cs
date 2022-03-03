using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Suffer_Travels.Migrations
{
    public partial class AddMissingTablesToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "tblHotelRooms");

            migrationBuilder.DropColumn(
                name: "ContactNo",
                table: "tblHotelRooms");

            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "tblHotelPhotos");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "tblHotelPhotos");

            migrationBuilder.RenameColumn(
                name: "Aid",
                table: "tblHotelPhotos",
                newName: "PID");

            migrationBuilder.RenameColumn(
                name: "HaId",
                table: "tblHotelPhotos",
                newName: "HpId");

            migrationBuilder.RenameColumn(
                name: "Hname",
                table: "tblHotelMaster",
                newName: "HName");

            migrationBuilder.CreateTable(
                name: "tblHotelAddress",
                columns: table => new
                {
                    HaId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblHotelAddress", x => x.HaId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblHotelAddress");

            migrationBuilder.RenameColumn(
                name: "PID",
                table: "tblHotelPhotos",
                newName: "Aid");

            migrationBuilder.RenameColumn(
                name: "HpId",
                table: "tblHotelPhotos",
                newName: "HaId");

            migrationBuilder.RenameColumn(
                name: "HName",
                table: "tblHotelMaster",
                newName: "Hname");

            migrationBuilder.AddColumn<long>(
                name: "AreaId",
                table: "tblHotelRooms",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ContactNo",
                table: "tblHotelRooms",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "tblHotelPhotos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "tblHotelPhotos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

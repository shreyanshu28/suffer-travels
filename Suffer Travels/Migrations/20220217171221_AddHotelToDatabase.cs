using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Suffer_Travels.Migrations
{
    public partial class AddHotelToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblHotelMaster",
                columns: table => new
                {
                    HId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaId = table.Column<long>(type: "bigint", nullable: false),
                    ContactNo = table.Column<long>(type: "bigint", nullable: false),
                    HotelType = table.Column<int>(type: "Int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblHotelMaster", x => x.HId);
                });

            migrationBuilder.CreateTable(
                name: "tblHotelPhotos",
                columns: table => new
                {
                    HaId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HId = table.Column<long>(type: "bigint", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblHotelPhotos", x => x.HaId);
                });

            migrationBuilder.CreateTable(
                name: "tblHotelRooms",
                columns: table => new
                {
                    HrId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HId = table.Column<long>(type: "bigint", nullable: false),
                    AreaId = table.Column<long>(type: "bigint", nullable: false),
                    ContactNo = table.Column<long>(type: "bigint", nullable: false),
                    TotalRooms = table.Column<int>(type: "Int", nullable: false),
                    Capacity = table.Column<int>(type: "Int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblHotelRooms", x => x.HrId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblHotelMaster");

            migrationBuilder.DropTable(
                name: "tblHotelPhotos");

            migrationBuilder.DropTable(
                name: "tblHotelRooms");
        }
    }
}

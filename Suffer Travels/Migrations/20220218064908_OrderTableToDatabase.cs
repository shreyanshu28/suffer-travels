using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Suffer_Travels.Migrations
{
    public partial class OrderTableToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblOrderHotel",
                columns: table => new
                {
                    OhId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoOfRooms = table.Column<int>(type: "Int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    HrId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOrderHotel", x => x.OhId);
                });

            migrationBuilder.CreateTable(
                name: "tblOrderMaster",
                columns: table => new
                {
                    OId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalPeople = table.Column<int>(type: "Int", nullable: false),
                    Payment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOrderMaster", x => x.OId);
                });

            migrationBuilder.CreateTable(
                name: "tblOrderPeople",
                columns: table => new
                {
                    OpId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Proof = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProofId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOrderPeople", x => x.OpId);
                });

            migrationBuilder.CreateTable(
                name: "tblOrderTour",
                columns: table => new
                {
                    OtId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    TourId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOrderTour", x => x.OtId);
                });

            migrationBuilder.CreateTable(
                name: "tblOrderVehicle",
                columns: table => new
                {
                    OvId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "Decimal(10,2)", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    VehicleInfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOrderVehicle", x => x.OvId);
                });

            migrationBuilder.CreateTable(
                name: "tblPaymentMaster",
                columns: table => new
                {
                    PId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "Decimal(10,2)", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPaymentMaster", x => x.PId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblOrderHotel");

            migrationBuilder.DropTable(
                name: "tblOrderMaster");

            migrationBuilder.DropTable(
                name: "tblOrderPeople");

            migrationBuilder.DropTable(
                name: "tblOrderTour");

            migrationBuilder.DropTable(
                name: "tblOrderVehicle");

            migrationBuilder.DropTable(
                name: "tblPaymentMaster");
        }
    }
}

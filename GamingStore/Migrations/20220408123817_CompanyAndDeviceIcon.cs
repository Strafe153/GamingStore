using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingStore.Migrations
{
    public partial class CompanyAndDeviceIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    InStock = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "PasswordSalt", "Role", "Username" },
                values: new object[] { new Guid("3b052423-4246-43a1-baa9-4017d6d02a05"), new byte[] { 226, 132, 33, 154, 209, 4, 73, 58, 41, 18, 203, 244, 215, 52, 148, 91, 213, 248, 88, 192, 76, 107, 244, 179, 21, 207, 39, 223, 130, 247, 238, 140, 0, 99, 92, 185, 118, 43, 223, 50, 231, 68, 173, 202, 253, 225, 65, 150, 153, 65, 76, 243, 158, 233, 54, 4, 60, 114, 166, 7, 220, 51, 255, 125 }, new byte[] { 21, 76, 17, 9, 204, 37, 109, 213, 56, 71, 240, 23, 74, 7, 203, 0, 139, 117, 241, 252, 11, 58, 77, 37, 232, 31, 73, 233, 222, 87, 114, 34, 53, 57, 67, 69, 19, 207, 82, 210, 174, 134, 34, 68, 154, 148, 94, 129, 220, 149, 133, 186, 235, 251, 74, 98, 179, 148, 120, 208, 75, 138, 78, 186, 117, 120, 82, 52, 58, 237, 124, 171, 127, 48, 217, 14, 154, 97, 210, 146, 181, 128, 140, 197, 43, 185, 19, 178, 109, 144, 231, 108, 166, 250, 199, 209, 78, 255, 125, 50, 162, 209, 140, 245, 64, 248, 32, 240, 218, 84, 31, 101, 116, 30, 133, 239, 192, 136, 158, 46, 167, 88, 122, 96, 150, 157, 235, 242 }, 0, "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CompanyId",
                table: "Devices",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}

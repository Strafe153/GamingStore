using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Price = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    InStock = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
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
                columns: new[] { "Id", "PasswordHash", "PasswordSalt", "Username", "Role" },
                values: new object[] { 1, new byte[] { 241, 238, 85, 247, 225, 198, 34, 195, 221, 130, 255, 82, 151, 253, 169, 41, 190, 103, 234, 107, 144, 103, 26, 225, 240, 232, 76, 66, 253, 249, 188, 173, 201, 237, 79, 244, 15, 181, 207, 156, 3, 61, 98, 188, 57, 98, 128, 53, 166, 167, 149, 236, 123, 24, 157, 100, 140, 206, 68, 32, 50, 211, 154, 240 }, new byte[] { 58, 98, 235, 243, 207, 186, 33, 43, 189, 222, 191, 164, 214, 135, 171, 72, 3, 243, 72, 54, 161, 163, 253, 70, 137, 227, 24, 22, 74, 179, 66, 36, 45, 173, 235, 148, 28, 173, 236, 11, 90, 244, 231, 89, 250, 212, 125, 228, 100, 58, 55, 151, 38, 113, 177, 226, 111, 216, 196, 248, 154, 124, 111, 110, 139, 238, 120, 106, 128, 218, 222, 47, 24, 168, 130, 237, 104, 56, 246, 221, 187, 100, 149, 0, 220, 192, 240, 34, 175, 145, 44, 159, 241, 98, 18, 120, 165, 238, 155, 93, 180, 188, 157, 99, 148, 234, 46, 127, 218, 253, 194, 69, 64, 89, 58, 234, 90, 170, 159, 29, 182, 39, 90, 182, 82, 148, 233, 78 }, "Admin", 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Name",
                table: "Companies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CompanyId",
                table: "Devices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Name",
                table: "Devices",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
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

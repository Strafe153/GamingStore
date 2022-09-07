using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class AddPictures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 44, 33, 65, 180, 28, 46, 205, 149, 70, 90, 47, 59, 93, 175, 85, 128, 134, 1, 25, 182, 44, 136, 132, 221, 64, 7, 217, 211, 146, 199, 123, 2, 167, 100, 75, 2, 215, 82, 43, 2, 96, 31, 53, 178, 167, 182, 68, 164, 230, 199, 250, 255, 191, 26, 11, 19, 225, 252, 190, 40, 195, 82, 98, 151 }, new byte[] { 19, 227, 116, 128, 66, 196, 109, 7, 157, 15, 224, 30, 176, 84, 179, 10, 105, 171, 14, 89, 9, 236, 130, 79, 245, 180, 204, 150, 110, 36, 46, 110, 114, 153, 164, 95, 179, 33, 6, 198, 32, 6, 241, 205, 185, 17, 82, 210, 67, 223, 49, 230, 160, 239, 236, 37, 120, 214, 232, 130, 127, 56, 203, 238, 77, 82, 84, 129, 15, 43, 246, 47, 107, 136, 10, 224, 107, 236, 82, 247, 112, 43, 27, 112, 115, 91, 248, 243, 139, 18, 128, 31, 147, 141, 93, 58, 197, 124, 226, 41, 247, 32, 218, 206, 249, 64, 88, 80, 195, 149, 31, 145, 124, 148, 214, 133, 167, 96, 0, 187, 212, 176, 155, 47, 120, 73, 254, 144 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Companies");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 241, 238, 85, 247, 225, 198, 34, 195, 221, 130, 255, 82, 151, 253, 169, 41, 190, 103, 234, 107, 144, 103, 26, 225, 240, 232, 76, 66, 253, 249, 188, 173, 201, 237, 79, 244, 15, 181, 207, 156, 3, 61, 98, 188, 57, 98, 128, 53, 166, 167, 149, 236, 123, 24, 157, 100, 140, 206, 68, 32, 50, 211, 154, 240 }, new byte[] { 58, 98, 235, 243, 207, 186, 33, 43, 189, 222, 191, 164, 214, 135, 171, 72, 3, 243, 72, 54, 161, 163, 253, 70, 137, 227, 24, 22, 74, 179, 66, 36, 45, 173, 235, 148, 28, 173, 236, 11, 90, 244, 231, 89, 250, 212, 125, 228, 100, 58, 55, 151, 38, 113, 177, 226, 111, 216, 196, 248, 154, 124, 111, 110, 139, 238, 120, 106, 128, 218, 222, 47, 24, 168, 130, 237, 104, 56, 246, 221, 187, 100, 149, 0, 220, 192, 240, 34, 175, 145, 44, 159, 241, 98, 18, 120, 165, 238, 155, 93, 180, 188, 157, 99, 148, 234, 46, 127, 218, 253, 194, 69, 64, 89, 58, 234, 90, 170, 159, 29, 182, 39, 90, 182, 82, 148, 233, 78 } });
        }
    }
}

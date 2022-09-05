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
                values: new object[] { new byte[] { 67, 27, 125, 79, 84, 220, 75, 60, 16, 60, 3, 173, 212, 218, 131, 253, 203, 11, 221, 141, 121, 31, 117, 41, 239, 113, 238, 97, 118, 191, 81, 134, 1, 51, 80, 227, 77, 12, 146, 95, 243, 5, 138, 165, 74, 255, 13, 20, 76, 75, 245, 92, 170, 116, 245, 47, 226, 93, 157, 15, 43, 243, 1, 79 }, new byte[] { 189, 216, 44, 29, 50, 193, 189, 78, 186, 29, 194, 30, 196, 247, 209, 202, 185, 220, 70, 108, 148, 146, 56, 208, 152, 29, 126, 0, 93, 14, 137, 98, 12, 193, 134, 133, 19, 107, 46, 46, 144, 228, 111, 182, 132, 19, 7, 148, 111, 204, 228, 30, 7, 95, 206, 185, 175, 208, 63, 120, 78, 50, 148, 123, 98, 199, 120, 251, 49, 233, 28, 144, 126, 249, 63, 243, 72, 196, 194, 70, 207, 185, 66, 250, 47, 197, 242, 249, 176, 226, 164, 67, 145, 181, 154, 48, 160, 203, 203, 199, 128, 192, 147, 208, 18, 185, 205, 109, 111, 51, 110, 57, 128, 197, 132, 232, 191, 39, 33, 17, 221, 128, 131, 60, 51, 77, 153, 234 } });
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

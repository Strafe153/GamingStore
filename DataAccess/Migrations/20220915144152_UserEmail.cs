using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class UserEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 247, 108, 106, 60, 85, 212, 223, 4, 89, 42, 126, 153, 75, 107, 249, 198, 227, 91, 27, 116, 217, 92, 54, 92, 149, 37, 162, 51, 26, 176, 48, 152, 184, 41, 73, 222, 170, 125, 86, 197, 237, 200, 135, 80, 77, 191, 185, 76, 216, 128, 152, 196, 206, 46, 184, 145, 83, 180, 115, 55, 106, 66, 76, 245 }, new byte[] { 247, 234, 248, 78, 196, 19, 102, 195, 23, 37, 247, 235, 151, 76, 93, 228, 59, 214, 207, 73, 127, 97, 219, 131, 169, 250, 215, 161, 99, 111, 179, 173, 159, 122, 98, 148, 193, 121, 196, 12, 123, 250, 3, 223, 71, 60, 146, 200, 143, 45, 17, 140, 14, 90, 50, 171, 202, 51, 20, 163, 69, 150, 97, 159, 67, 229, 4, 67, 2, 105, 122, 180, 70, 235, 59, 77, 220, 12, 3, 239, 145, 143, 156, 146, 105, 141, 206, 141, 66, 172, 245, 102, 39, 29, 155, 7, 157, 121, 62, 116, 20, 231, 79, 194, 8, 207, 171, 218, 21, 244, 90, 179, 246, 89, 119, 22, 222, 19, 229, 85, 5, 228, 202, 230, 104, 91, 57, 147 } });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 44, 33, 65, 180, 28, 46, 205, 149, 70, 90, 47, 59, 93, 175, 85, 128, 134, 1, 25, 182, 44, 136, 132, 221, 64, 7, 217, 211, 146, 199, 123, 2, 167, 100, 75, 2, 215, 82, 43, 2, 96, 31, 53, 178, 167, 182, 68, 164, 230, 199, 250, 255, 191, 26, 11, 19, 225, 252, 190, 40, 195, 82, 98, 151 }, new byte[] { 19, 227, 116, 128, 66, 196, 109, 7, 157, 15, 224, 30, 176, 84, 179, 10, 105, 171, 14, 89, 9, 236, 130, 79, 245, 180, 204, 150, 110, 36, 46, 110, 114, 153, 164, 95, 179, 33, 6, 198, 32, 6, 241, 205, 185, 17, 82, 210, 67, 223, 49, 230, 160, 239, 236, 37, 120, 214, 232, 130, 127, 56, 203, 238, 77, 82, 84, 129, 15, 43, 246, 47, 107, 136, 10, 224, 107, 236, 82, 247, 112, 43, 27, 112, 115, 91, 248, 243, 139, 18, 128, 31, 147, 141, 93, 58, 197, 124, 226, 41, 247, 32, 218, 206, 249, 64, 88, 80, 195, 149, 31, 145, 124, 148, 214, 133, 167, 96, 0, 187, 212, 176, 155, 47, 120, 73, 254, 144 } });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }
    }
}

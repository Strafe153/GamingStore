﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class AddAdminEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "PasswordHash", "PasswordSalt" },
                values: new object[] { "admin@gmail.com", new byte[] { 153, 204, 169, 51, 26, 103, 176, 250, 127, 168, 69, 17, 240, 117, 162, 148, 59, 148, 206, 253, 129, 13, 199, 15, 135, 74, 235, 197, 19, 231, 111, 42, 25, 126, 35, 51, 37, 196, 235, 71, 160, 42, 107, 213, 213, 183, 56, 130, 49, 62, 45, 70, 5, 187, 174, 134, 42, 63, 51, 234, 62, 249, 235, 64 }, new byte[] { 236, 134, 230, 170, 253, 65, 212, 224, 74, 66, 7, 102, 60, 62, 205, 166, 196, 68, 141, 63, 123, 24, 79, 170, 156, 65, 29, 24, 240, 85, 136, 11, 232, 158, 14, 112, 186, 97, 218, 120, 68, 38, 150, 141, 159, 153, 20, 131, 55, 164, 246, 82, 250, 8, 243, 247, 88, 151, 130, 2, 134, 191, 125, 204, 67, 31, 91, 60, 115, 99, 184, 124, 255, 109, 46, 125, 182, 216, 116, 100, 124, 159, 85, 82, 156, 61, 163, 245, 129, 180, 232, 157, 140, 65, 141, 71, 183, 196, 108, 164, 43, 113, 173, 4, 57, 45, 92, 228, 133, 109, 64, 235, 233, 177, 24, 100, 93, 195, 74, 223, 39, 22, 78, 168, 226, 48, 152, 156 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "PasswordHash", "PasswordSalt" },
                values: new object[] { null, new byte[] { 247, 108, 106, 60, 85, 212, 223, 4, 89, 42, 126, 153, 75, 107, 249, 198, 227, 91, 27, 116, 217, 92, 54, 92, 149, 37, 162, 51, 26, 176, 48, 152, 184, 41, 73, 222, 170, 125, 86, 197, 237, 200, 135, 80, 77, 191, 185, 76, 216, 128, 152, 196, 206, 46, 184, 145, 83, 180, 115, 55, 106, 66, 76, 245 }, new byte[] { 247, 234, 248, 78, 196, 19, 102, 195, 23, 37, 247, 235, 151, 76, 93, 228, 59, 214, 207, 73, 127, 97, 219, 131, 169, 250, 215, 161, 99, 111, 179, 173, 159, 122, 98, 148, 193, 121, 196, 12, 123, 250, 3, 223, 71, 60, 146, 200, 143, 45, 17, 140, 14, 90, 50, 171, 202, 51, 20, 163, 69, 150, 97, 159, 67, 229, 4, 67, 2, 105, 122, 180, 70, 235, 59, 77, 220, 12, 3, 239, 145, 143, 156, 146, 105, 141, 206, 141, 66, 172, 245, 102, 39, 29, 155, 7, 157, 121, 62, 116, 20, 231, 79, 194, 8, 207, 171, 218, 21, 244, 90, 179, 246, 89, 119, 22, 222, 19, 229, 85, 5, 228, 202, 230, 104, 91, 57, 147 } });
        }
    }
}
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddSalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Users",
                type: "character varying(16)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Salt", "UserName" },
                values: new object[,]
                {
                    { new Guid("32e8110c-b664-4a8b-b69d-f59fba269314"), "1111", null, null, null, "RadiantDwarf" },
                    { new Guid("3b078e3f-e9a4-4098-adf2-ced840603bb4"), "2222", null, null, null, "Dolaprolorap" },
                    { new Guid("5399ee18-ffdf-470b-bbae-160287b33244"), "3333", null, null, null, "UltraGreed" },
                    { new Guid("da50fb97-6cca-4b03-af03-2d34ef433d59"), "4444", null, null, null, "Reveqqq" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("32e8110c-b664-4a8b-b69d-f59fba269314"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("3b078e3f-e9a4-4098-adf2-ced840603bb4"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("5399ee18-ffdf-470b-bbae-160287b33244"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("da50fb97-6cca-4b03-af03-2d34ef433d59"));

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Users");
        }
    }
}

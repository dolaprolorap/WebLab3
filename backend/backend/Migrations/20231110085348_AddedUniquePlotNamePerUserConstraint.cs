using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniquePlotNamePerUserConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2604f5ca-8e86-4c7d-99e5-ecec87514aee"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("28f89472-a9a8-43f9-9a81-c6e4e4cdd9b1"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5b11f6b7-f923-4aba-8c33-bcbe2a3f1918"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8a98da3f-e961-4db3-ab43-8c35c404b67e"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Password" },
                values: new object[,]
                {
                    { new Guid("32e8110c-b664-4a8b-b69d-f59fba269314"), "RadiantDwarf", "1111" },
                    { new Guid("3b078e3f-e9a4-4098-adf2-ced840603bb4"), "Dolaprolorap", "2222" },
                    { new Guid("5399ee18-ffdf-470b-bbae-160287b33244"), "UltraGreed", "3333" },
                    { new Guid("da50fb97-6cca-4b03-af03-2d34ef433d59"), "Reveqqq", "4444" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plots_Name_UserId",
                table: "Plots",
                columns: new[] { "Name", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Plots_Name_UserId",
                table: "Plots");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("32e8110c-b664-4a8b-b69d-f59fba269314"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3b078e3f-e9a4-4098-adf2-ced840603bb4"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5399ee18-ffdf-470b-bbae-160287b33244"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("da50fb97-6cca-4b03-af03-2d34ef433d59"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Password" },
                values: new object[,]
                {
                    { new Guid("2604f5ca-8e86-4c7d-99e5-ecec87514aee"), "UltraGreed", "3333" },
                    { new Guid("28f89472-a9a8-43f9-9a81-c6e4e4cdd9b1"), "RadiantDwarf", "1111" },
                    { new Guid("5b11f6b7-f923-4aba-8c33-bcbe2a3f1918"), "Dolaprolorap", "2222" },
                    { new Guid("8a98da3f-e961-4db3-ab43-8c35c404b67e"), "Reveqqq", "4444" }
                });
        }
    }
}

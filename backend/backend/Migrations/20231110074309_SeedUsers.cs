using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}

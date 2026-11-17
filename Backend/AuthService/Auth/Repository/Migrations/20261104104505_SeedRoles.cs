using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0456ca68-6e5f-494f-9874-224c2577ad03", "0456ca68-6e5f-494f-9874-224c2577ad03", "Doctor", "DOCTOR" },
                    { "6cb8d370-318b-453b-91f8-97f41ffbcbe3", "6cb8d370-318b-453b-91f8-97f41ffbcbe3", "Admin", "ADMIN" },
                    { "d4d4bfcb-eaa1-49f7-92cc-e27a9be9fb7e", "d4d4bfcb-eaa1-49f7-92cc-e27a9be9fb7e", "Patient", "PATIENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0456ca68-6e5f-494f-9874-224c2577ad03");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6cb8d370-318b-453b-91f8-97f41ffbcbe3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4d4bfcb-eaa1-49f7-92cc-e27a9be9fb7e");
        }
    }
}

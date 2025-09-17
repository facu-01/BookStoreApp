using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "32fe658e-2cf4-44c2-a86c-771766fd7158", null, "User", "USER" },
                    { "a21a58d3-d387-4710-9f38-659c6c53a638", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "8182828c-3c9e-40eb-9751-0e43bdc33402", 0, "9a5dc9cb-d59c-4e55-9ded-88f6888085ae", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN", "AQAAAAIAAYagAAAAEL69nkno0DdRn85YvRHDgPT3Am0EBzl7+SKyXZCpCcfO+rlmgmMyRBWf5uLU4+pqeQ==", null, false, "32f555f8-05d5-4ece-b4ae-99bc3ab9ed73", false, "admin" },
                    { "95bb6474-6e3d-46c1-a644-21397c83dc17", 0, "2257c8eb-211a-4f12-be1c-78456fef12c1", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER", "AQAAAAIAAYagAAAAECZJ8C+Zr9Od4tL9J1nb+JKoPn9ryyza93lwIjvtZOuTCvOqhg7VGHnb2t0+Y7w6mQ==", null, false, "e516f4c6-9e0b-438e-aef8-3a1517997d76", false, "user" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "a21a58d3-d387-4710-9f38-659c6c53a638", "8182828c-3c9e-40eb-9751-0e43bdc33402" },
                    { "32fe658e-2cf4-44c2-a86c-771766fd7158", "95bb6474-6e3d-46c1-a644-21397c83dc17" }
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

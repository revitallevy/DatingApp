using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentityRoleSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-id",
                column: "ConcurrencyStamp",
                value: "c3333333-3333-3333-3333-333333333333");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "member-id",
                column: "ConcurrencyStamp",
                value: "a1111111-1111-1111-1111-111111111111");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "moderator-id",
                column: "ConcurrencyStamp",
                value: "b2222222-2222-2222-2222-222222222222");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-id",
                column: "ConcurrencyStamp",
                value: "d54c053e-2b95-4b82-b86b-49cbe45f926a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "member-id",
                column: "ConcurrencyStamp",
                value: "996f1b90-100a-4d25-a1c2-35d04cc747ff");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "moderator-id",
                column: "ConcurrencyStamp",
                value: "7aeb7d3c-1474-48ee-b80f-25100f239be9");
        }
    }
}

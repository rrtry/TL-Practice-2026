using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteRoomTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomType_Property_PropertyId",
                table: "RoomType");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomType_Property_PropertyId",
                table: "RoomType",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomType_Property_PropertyId",
                table: "RoomType");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomType_Property_PropertyId",
                table: "RoomType",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

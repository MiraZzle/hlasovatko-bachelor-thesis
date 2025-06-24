using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateActivities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Activities",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<List<string>>(
                name: "Tags",
                table: "Activities",
                type: "text[]",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_OwnerId",
                table: "Activities",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Users_OwnerId",
                table: "Activities",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Users_OwnerId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_OwnerId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Activities");
        }
    }
}

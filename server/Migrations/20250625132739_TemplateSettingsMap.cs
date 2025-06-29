using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class TemplateSettingsMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_TemplateSettings_SettingsId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_SettingsId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "Templates");

            migrationBuilder.AddColumn<Guid>(
                name: "TemplateId",
                table: "TemplateSettings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TemplateSettings_TemplateId",
                table: "TemplateSettings",
                column: "TemplateId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateSettings_Templates_TemplateId",
                table: "TemplateSettings",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateSettings_Templates_TemplateId",
                table: "TemplateSettings");

            migrationBuilder.DropIndex(
                name: "IX_TemplateSettings_TemplateId",
                table: "TemplateSettings");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "TemplateSettings");

            migrationBuilder.AddColumn<Guid>(
                name: "SettingsId",
                table: "Templates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Templates_SettingsId",
                table: "Templates",
                column: "SettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_TemplateSettings_SettingsId",
                table: "Templates",
                column: "SettingsId",
                principalTable: "TemplateSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

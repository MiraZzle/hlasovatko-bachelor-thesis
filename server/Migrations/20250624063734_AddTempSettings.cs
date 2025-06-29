using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class AddTempSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Template_TemplateId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Template_TemplateId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Template_TemplateSettings_SettingsId",
                table: "Template");

            migrationBuilder.DropForeignKey(
                name: "FK_Template_Users_OwnerId",
                table: "Template");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Template",
                table: "Template");

            migrationBuilder.RenameTable(
                name: "Template",
                newName: "Templates");

            migrationBuilder.RenameIndex(
                name: "IX_Template_SettingsId",
                table: "Templates",
                newName: "IX_Templates_SettingsId");

            migrationBuilder.RenameIndex(
                name: "IX_Template_OwnerId",
                table: "Templates",
                newName: "IX_Templates_OwnerId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivationDate",
                table: "Sessions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentActivity",
                table: "Sessions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Mode",
                table: "Sessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Participants",
                table: "Sessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Templates",
                table: "Templates",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_JoinCode",
                table: "Sessions",
                column: "JoinCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Templates_TemplateId",
                table: "Activities",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Templates_TemplateId",
                table: "Sessions",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_TemplateSettings_SettingsId",
                table: "Templates",
                column: "SettingsId",
                principalTable: "TemplateSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_Users_OwnerId",
                table: "Templates",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Templates_TemplateId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Templates_TemplateId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_TemplateSettings_SettingsId",
                table: "Templates");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_Users_OwnerId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_JoinCode",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Templates",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "ActivationDate",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "CurrentActivity",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Mode",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Participants",
                table: "Sessions");

            migrationBuilder.RenameTable(
                name: "Templates",
                newName: "Template");

            migrationBuilder.RenameIndex(
                name: "IX_Templates_SettingsId",
                table: "Template",
                newName: "IX_Template_SettingsId");

            migrationBuilder.RenameIndex(
                name: "IX_Templates_OwnerId",
                table: "Template",
                newName: "IX_Template_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Template",
                table: "Template",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Template_TemplateId",
                table: "Activities",
                column: "TemplateId",
                principalTable: "Template",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Template_TemplateId",
                table: "Sessions",
                column: "TemplateId",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Template_TemplateSettings_SettingsId",
                table: "Template",
                column: "SettingsId",
                principalTable: "TemplateSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Template_Users_OwnerId",
                table: "Template",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

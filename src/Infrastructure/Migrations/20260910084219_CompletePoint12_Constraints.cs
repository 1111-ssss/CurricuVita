using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CompletePoint12_Constraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CVs_Positions_PositionId",
                table: "CVs");

            migrationBuilder.DropForeignKey(
                name: "FK_PositionAccessRules_AttributeDefinitions_AttributeDefinitio~",
                table: "PositionAccessRules");

            migrationBuilder.DropForeignKey(
                name: "FK_PositionAttributes_AttributeDefinitions_AttributeDefinition~",
                table: "PositionAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAttributeValues_AttributeDefinitions_AttributeDefinitio~",
                table: "UserAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_UserBadges_UserId",
                table: "UserBadges");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Positions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Badges",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Badges",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Badges",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "AttributeDefinitions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserBadges_UserId_BadgeId",
                table: "UserBadges",
                columns: new[] { "UserId", "BadgeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Title",
                table: "Projects",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_Title",
                table: "Positions",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_CVs_CreatedAt",
                table: "CVs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Badges_Code",
                table: "Badges",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CVs_Positions_PositionId",
                table: "CVs",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PositionAccessRules_AttributeDefinitions_AttributeDefinitio~",
                table: "PositionAccessRules",
                column: "AttributeDefinitionId",
                principalTable: "AttributeDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PositionAttributes_AttributeDefinitions_AttributeDefinition~",
                table: "PositionAttributes",
                column: "AttributeDefinitionId",
                principalTable: "AttributeDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAttributeValues_AttributeDefinitions_AttributeDefinitio~",
                table: "UserAttributeValues",
                column: "AttributeDefinitionId",
                principalTable: "AttributeDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CVs_Positions_PositionId",
                table: "CVs");

            migrationBuilder.DropForeignKey(
                name: "FK_PositionAccessRules_AttributeDefinitions_AttributeDefinitio~",
                table: "PositionAccessRules");

            migrationBuilder.DropForeignKey(
                name: "FK_PositionAttributes_AttributeDefinitions_AttributeDefinition~",
                table: "PositionAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAttributeValues_AttributeDefinitions_AttributeDefinitio~",
                table: "UserAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_UserBadges_UserId_BadgeId",
                table: "UserBadges");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Title",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Positions_Title",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_CVs_CreatedAt",
                table: "CVs");

            migrationBuilder.DropIndex(
                name: "IX_Badges_Code",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "AttributeDefinitions");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Badges",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Badges",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Badges",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_UserBadges_UserId",
                table: "UserBadges",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CVs_Positions_PositionId",
                table: "CVs",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PositionAccessRules_AttributeDefinitions_AttributeDefinitio~",
                table: "PositionAccessRules",
                column: "AttributeDefinitionId",
                principalTable: "AttributeDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PositionAttributes_AttributeDefinitions_AttributeDefinition~",
                table: "PositionAttributes",
                column: "AttributeDefinitionId",
                principalTable: "AttributeDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAttributeValues_AttributeDefinitions_AttributeDefinitio~",
                table: "UserAttributeValues",
                column: "AttributeDefinitionId",
                principalTable: "AttributeDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

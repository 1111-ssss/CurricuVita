using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TightenSchemaConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Projects_UserId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_PositionTags_PositionId",
                table: "PositionTags");

            migrationBuilder.DropIndex(
                name: "IX_DiscussionMessages_PositionId",
                table: "DiscussionMessages");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxProjectCount",
                table: "Positions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserId_EndDate",
                table: "Projects",
                columns: new[] { "UserId", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserId_StartDate",
                table: "Projects",
                columns: new[] { "UserId", "StartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PositionTags_PositionId_TagId",
                table: "PositionTags",
                columns: new[] { "PositionId", "TagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_CreatedAt",
                table: "Positions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_UpdatedAt",
                table: "Positions",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionMessages_PositionId_CreatedAt",
                table: "DiscussionMessages",
                columns: new[] { "PositionId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeDefinitions_Category",
                table: "AttributeDefinitions",
                column: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Projects_UserId_EndDate",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_UserId_StartDate",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_PositionTags_PositionId_TagId",
                table: "PositionTags");

            migrationBuilder.DropIndex(
                name: "IX_Positions_CreatedAt",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_UpdatedAt",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_DiscussionMessages_PositionId_CreatedAt",
                table: "DiscussionMessages");

            migrationBuilder.DropIndex(
                name: "IX_AttributeDefinitions_Category",
                table: "AttributeDefinitions");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MaxProjectCount",
                table: "Positions");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserId",
                table: "Projects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionTags_PositionId",
                table: "PositionTags",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionMessages_PositionId",
                table: "DiscussionMessages",
                column: "PositionId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionCompanyLevelAndFts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Positions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "Positions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_Company",
                table: "Positions",
                column: "Company");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_Level",
                table: "Positions",
                column: "Level");

            // Native PostgreSQL full-text search (tsvector/GIN, english config).
            migrationBuilder.Sql(
                "CREATE INDEX IF NOT EXISTS \"IX_Positions_SearchVector\" ON \"Positions\" USING GIN (to_tsvector('english', coalesce(\"Title\", '') || ' ' || coalesce(\"DescriptionMarkdown\", '') || ' ' || coalesce(\"Company\", '')));");
            migrationBuilder.Sql(
                "CREATE INDEX IF NOT EXISTS \"IX_Users_SearchVector\" ON \"AspNetUsers\" USING GIN (to_tsvector('english', coalesce(\"FirstName\", '') || ' ' || coalesce(\"LastName\", '') || ' ' || coalesce(\"Email\", '')));");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_Positions_SearchVector\";");
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_Users_SearchVector\";");

            migrationBuilder.DropIndex(
                name: "IX_Positions_Company",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_Level",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Positions");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FtsTrgmAndAttributeUpdatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AttributeDefinitions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            // Trigram support for ILIKE substring fallback next to tsvector/GIN FTS.
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS pg_trgm;");
            migrationBuilder.Sql(
                "CREATE INDEX IF NOT EXISTS \"IX_Positions_Title_Trgm\" ON \"Positions\" USING GIN (\"Title\" gin_trgm_ops);");
            migrationBuilder.Sql(
                "CREATE INDEX IF NOT EXISTS \"IX_Positions_Description_Trgm\" ON \"Positions\" USING GIN (\"DescriptionMarkdown\" gin_trgm_ops);");
            migrationBuilder.Sql(
                "CREATE INDEX IF NOT EXISTS \"IX_Positions_Company_Trgm\" ON \"Positions\" USING GIN (\"Company\" gin_trgm_ops);");
            migrationBuilder.Sql(
                "CREATE INDEX IF NOT EXISTS \"IX_Users_FirstName_Trgm\" ON \"AspNetUsers\" USING GIN (\"FirstName\" gin_trgm_ops);");
            migrationBuilder.Sql(
                "CREATE INDEX IF NOT EXISTS \"IX_Users_LastName_Trgm\" ON \"AspNetUsers\" USING GIN (\"LastName\" gin_trgm_ops);");
            migrationBuilder.Sql(
                "CREATE INDEX IF NOT EXISTS \"IX_Users_Email_Trgm\" ON \"AspNetUsers\" USING GIN (\"Email\" gin_trgm_ops);");
            migrationBuilder.Sql(
                "CREATE INDEX IF NOT EXISTS \"IX_Tags_Name_Trgm\" ON \"Tags\" USING GIN (\"Name\" gin_trgm_ops);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_Positions_Title_Trgm\";");
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_Positions_Description_Trgm\";");
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_Positions_Company_Trgm\";");
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_Users_FirstName_Trgm\";");
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_Users_LastName_Trgm\";");
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_Users_Email_Trgm\";");
            migrationBuilder.Sql("DROP INDEX IF EXISTS \"IX_Tags_Name_Trgm\";");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AttributeDefinitions");
        }
    }
}

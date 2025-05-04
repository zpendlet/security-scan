using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace security_scan.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FindingsJson",
                table: "Reports",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "Reports",
                newName: "Summary");

            migrationBuilder.AddColumn<string>(
                name: "SeverityLevel",
                table: "Reports",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeverityLevel",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reports",
                newName: "FindingsJson");

            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "Reports",
                newName: "Comments");
        }
    }
}

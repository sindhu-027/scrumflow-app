using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SprintManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSprintStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Sprints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Sprints");
        }
    }
}

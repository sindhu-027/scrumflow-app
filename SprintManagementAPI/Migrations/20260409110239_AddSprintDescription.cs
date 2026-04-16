using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SprintManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSprintDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sprints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sprints");
        }
    }
}

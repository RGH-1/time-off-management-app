using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace time_off_management_app.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionLevelForHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Position",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Position");
        }
    }
}

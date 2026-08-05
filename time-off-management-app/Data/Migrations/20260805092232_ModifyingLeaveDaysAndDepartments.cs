using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace time_off_management_app.Migrations
{
    /// <inheritdoc />
    public partial class ModifyingLeaveDaysAndDepartments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_AspNetUsers_ManagerId",
                table: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Department_ManagerId",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "OtherReason",
                table: "TimeOffForm",
                newName: "ReasonDescription");

            migrationBuilder.RenameColumn(
                name: "MaxDaysOff",
                table: "AspNetUsers",
                newName: "MaxOtherLeaveDays");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "AspNetUsers",
                newName: "UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_UnitId");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Reason",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAnnualLeave",
                table: "Reason",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxAnnualLeaveDays",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Unit_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ManagerId",
                table: "AspNetUsers",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PositionId",
                table: "AspNetUsers",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_DepartmentId",
                table: "Unit",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ManagerId",
                table: "AspNetUsers",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Position_PositionId",
                table: "AspNetUsers",
                column: "PositionId",
                principalTable: "Position",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Unit_UnitId",
                table: "AspNetUsers",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ManagerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Position_PositionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Unit_UnitId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ManagerId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PositionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Reason");

            migrationBuilder.DropColumn(
                name: "IsAnnualLeave",
                table: "Reason");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MaxAnnualLeaveDays",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ReasonDescription",
                table: "TimeOffForm",
                newName: "OtherReason");

            migrationBuilder.RenameColumn(
                name: "UnitId",
                table: "AspNetUsers",
                newName: "DepartmentId");

            migrationBuilder.RenameColumn(
                name: "MaxOtherLeaveDays",
                table: "AspNetUsers",
                newName: "MaxDaysOff");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_UnitId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_DepartmentId");

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Department",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Department_ManagerId",
                table: "Department",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_AspNetUsers_ManagerId",
                table: "Department",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integgreat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProjectHoursWorked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoursWorked",
                table: "Projects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HoursWorked",
                table: "Projects",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

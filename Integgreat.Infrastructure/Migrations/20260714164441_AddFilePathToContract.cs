using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integgreat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFilePathToContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileUrl",
                table: "Contracts",
                newName: "FilePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Contracts",
                newName: "FileUrl");
        }
    }
}

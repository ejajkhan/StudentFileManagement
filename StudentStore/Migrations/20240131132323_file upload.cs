using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentStore.Migrations
{
    /// <inheritdoc />
    public partial class fileupload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocumentName",
                table: "Documents",
                newName: "FileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Documents",
                newName: "DocumentName");
        }
    }
}

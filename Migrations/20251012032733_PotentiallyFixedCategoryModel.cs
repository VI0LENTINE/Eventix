using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventix.Migrations
{
    /// <inheritdoc />
    public partial class PotentiallyFixedCategoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Category",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Category",
                newName: "Name");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventix.Migrations
{
    /// <inheritdoc />
    public partial class ChangedCategoryNameToTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Title",
            //    table: "Performance");

            //migrationBuilder.DropColumn(
            //    name: "Name",
            //    table: "Performance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "Name",
            //    table: "Performance",
            //    newName: "Title");
        }
    }
}

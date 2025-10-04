using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventix.Migrations
{
    /// <inheritdoc />
    public partial class AddedExtraPerformanceFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Performance_Category_CategoryId",
                table: "Performance");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Performance",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Host",
                table: "Performance",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Performance",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Performance",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Performance_Category_CategoryId",
                table: "Performance",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Performance_Category_CategoryId",
                table: "Performance");

            migrationBuilder.DropColumn(
                name: "Host",
                table: "Performance");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Performance");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Performance");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Performance",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Performance_Category_CategoryId",
                table: "Performance",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId");
        }
    }
}

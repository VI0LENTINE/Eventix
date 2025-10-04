using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventix.Migrations
{
    /// <inheritdoc />
    public partial class NullableCategory : Migration
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddForeignKey(
                name: "FK_Performance_Category_CategoryId",
                table: "Performance",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

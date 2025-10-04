using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventix.Migrations
{
    public partial class FixPerformanceNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Fill empty Name fields with Description values
            migrationBuilder.Sql(@"
                UPDATE Performance
                SET Name = Description
                WHERE Name IS NULL OR Name = ''
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Optional: Reset Names back to empty if rolling back
            migrationBuilder.Sql(@"
                UPDATE Performance
                SET Name = ''
                WHERE Name = Description
            ");
        }
    }
}

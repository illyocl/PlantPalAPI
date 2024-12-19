using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantPalAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSizeToCalendarEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "CalendarEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "CalendarEvents");
        }
    }
}

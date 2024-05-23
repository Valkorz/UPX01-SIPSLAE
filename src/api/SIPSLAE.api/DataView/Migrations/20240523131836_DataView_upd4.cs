using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataView.Migrations
{
    /// <inheritdoc />
    public partial class DataView_upd4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MinuteInterval",
                table: "logs",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinuteInterval",
                table: "logs");
        }
    }
}

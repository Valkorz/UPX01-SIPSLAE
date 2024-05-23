using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataView.Migrations
{
    /// <inheritdoc />
    public partial class DataView_upd1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRaining",
                table: "logs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "logs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Variation",
                table: "logs",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRaining",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "logs");

            migrationBuilder.DropColumn(
                name: "Variation",
                table: "logs");
        }
    }
}

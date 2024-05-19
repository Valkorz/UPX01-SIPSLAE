using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataView.Migrations
{
    /// <inheritdoc />
    public partial class DataView_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOfRecord",
                table: "logs",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOfRecord",
                table: "logs");
        }
    }
}

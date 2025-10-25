using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentTimeTrackerApp.Migrations
{
    /// <inheritdoc />
    public partial class Timecardupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Timecards");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Timecards");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Timecards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Timecards",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Timecards",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Timecards",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StudentTimeTrackerApp.Migrations
{
    /// <inheritdoc />
    public partial class ModifyMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Messages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
            //migrationBuilder.CreateTable(
            //    name: "Messages",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        Body = table.Column<string>(type: "text", nullable: false),
            //        Sender = table.Column<string>(type: "text", nullable: false),
            //        Recipient = table.Column<string>(type: "text", nullable: false),
            //        CourseId = table.Column<int>(type: "integer", nullable: false),
            //        Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Messages", x => x.Id);
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}

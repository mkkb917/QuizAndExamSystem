using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnStatusAndPaperLayoutVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaperLayout",
                table: "PaperSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaperVersion",
                table: "PaperSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Grades",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaperLayout",
                table: "PaperSettings");

            migrationBuilder.DropColumn(
                name: "PaperVersion",
                table: "PaperSettings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Grades",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}

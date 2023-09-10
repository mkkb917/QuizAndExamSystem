using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystem.Migrations
{
    public partial class updatePaperSettingsRows : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "PaperSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "FillInBlanksCount",
                table: "PaperSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "LongQsCount",
                table: "PaperSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MCQsCount",
                table: "PaperSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "SEQsCount",
                table: "PaperSettings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubjectName",
                table: "PaperSettings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "PaperSettings");

            migrationBuilder.DropColumn(
                name: "FillInBlanksCount",
                table: "PaperSettings");

            migrationBuilder.DropColumn(
                name: "LongQsCount",
                table: "PaperSettings");

            migrationBuilder.DropColumn(
                name: "MCQsCount",
                table: "PaperSettings");

            migrationBuilder.DropColumn(
                name: "SEQsCount",
                table: "PaperSettings");

            migrationBuilder.DropColumn(
                name: "SubjectName",
                table: "PaperSettings");
        }
    }
}

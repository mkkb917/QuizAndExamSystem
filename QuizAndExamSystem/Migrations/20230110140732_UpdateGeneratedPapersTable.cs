using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGeneratedPapersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "GeneratedPapers",
                newName: "SolutionFile");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "GeneratedPapers",
                newName: "Subject");

            migrationBuilder.RenameColumn(
                name: "GeneratePaper",
                table: "GeneratedPapers",
                newName: "PaperFile");

            migrationBuilder.RenameColumn(
                name: "GenerateDate",
                table: "GeneratedPapers",
                newName: "CreatedOn");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "GeneratedPapers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "GeneratedPapers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "GeneratedPapers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccess",
                table: "GeneratedPapers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "GeneratedPapers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "GeneratedPapers");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "GeneratedPapers");

            migrationBuilder.DropColumn(
                name: "IsSuccess",
                table: "GeneratedPapers");

            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "GeneratedPapers",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "SolutionFile",
                table: "GeneratedPapers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "PaperFile",
                table: "GeneratedPapers",
                newName: "GeneratePaper");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "GeneratedPapers",
                newName: "GenerateDate");
        }
    }
}

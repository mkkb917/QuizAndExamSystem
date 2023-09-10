using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystem.Migrations
{
    public partial class UpdatePaperSettings2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PaperSettings",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "ShortQuestionsMark",
                table: "PaperSettings",
                newName: "SEQsMarks");

            migrationBuilder.RenameColumn(
                name: "MultipleChoiceQuestionsMark",
                table: "PaperSettings",
                newName: "MCQsMarks");

            migrationBuilder.RenameColumn(
                name: "LongQuestionsMark",
                table: "PaperSettings",
                newName: "LongQsMarks");

            migrationBuilder.RenameColumn(
                name: "FillInBlankMark",
                table: "PaperSettings",
                newName: "FillInBlanksMarks");

            migrationBuilder.AlterColumn<int>(
                name: "Medium",
                table: "PaperSettings",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "PaperSettings",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "SEQsMarks",
                table: "PaperSettings",
                newName: "ShortQuestionsMark");

            migrationBuilder.RenameColumn(
                name: "MCQsMarks",
                table: "PaperSettings",
                newName: "MultipleChoiceQuestionsMark");

            migrationBuilder.RenameColumn(
                name: "LongQsMarks",
                table: "PaperSettings",
                newName: "LongQuestionsMark");

            migrationBuilder.RenameColumn(
                name: "FillInBlanksMarks",
                table: "PaperSettings",
                newName: "FillInBlankMark");

            migrationBuilder.AlterColumn<bool>(
                name: "Medium",
                table: "PaperSettings",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}

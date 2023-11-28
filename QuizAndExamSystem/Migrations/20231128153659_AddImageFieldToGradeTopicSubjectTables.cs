using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddImageFieldToGradeTopicSubjectTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Topics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "SEQsCount",
                table: "PaperSettings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Grades",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Grades");

            migrationBuilder.AlterColumn<int>(
                name: "SEQsCount",
                table: "PaperSettings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}

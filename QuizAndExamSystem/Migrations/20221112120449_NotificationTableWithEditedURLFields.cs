using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamSystem.Migrations
{
    public partial class NotificationTableWithEditedURLFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SchoolLogoUrl",
                table: "SchoolInfos",
                newName: "SchoolLogo");

            migrationBuilder.RenameColumn(
                name: "SchoolLogoUrl",
                table: "PaperSettings",
                newName: "SchoolLogo");

            migrationBuilder.RenameColumn(
                name: "GeneratedPaperUrl",
                table: "GeneratedPapers",
                newName: "GeneratePaper");

            migrationBuilder.RenameColumn(
                name: "GeneratedDate",
                table: "GeneratedPapers",
                newName: "GenerateDate");

            migrationBuilder.RenameColumn(
                name: "ProfileImageUrl",
                table: "AspNetUsers",
                newName: "ProfileImage");

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Account", "1, 1"),
                    NotificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssuedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotificationFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.RenameColumn(
                name: "SchoolLogo",
                table: "SchoolInfos",
                newName: "SchoolLogoUrl");

            migrationBuilder.RenameColumn(
                name: "SchoolLogo",
                table: "PaperSettings",
                newName: "SchoolLogoUrl");

            migrationBuilder.RenameColumn(
                name: "GeneratePaper",
                table: "GeneratedPapers",
                newName: "GeneratedPaperUrl");

            migrationBuilder.RenameColumn(
                name: "GenerateDate",
                table: "GeneratedPapers",
                newName: "GeneratedDate");

            migrationBuilder.RenameColumn(
                name: "ProfileImage",
                table: "AspNetUsers",
                newName: "ProfileImageUrl");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace ResumeReview.Data.Migrations
{
    public partial class addedCloudStorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriveId",
                table: "Resume",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriveResumeId",
                table: "Resume",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriveId",
                table: "Resume");

            migrationBuilder.DropColumn(
                name: "DriveResumeId",
                table: "Resume");
        }
    }
}

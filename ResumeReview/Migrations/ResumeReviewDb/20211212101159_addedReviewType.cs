using Microsoft.EntityFrameworkCore.Migrations;

namespace ResumeReview.Migrations.ResumeReviewDb
{
    public partial class addedReviewType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReviewType",
                table: "Reviews",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewType",
                table: "Reviews");
        }
    }
}

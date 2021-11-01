using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ResumeReview.Data.Migrations
{
    public partial class addedReviewColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserViewed",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResumeId = table.Column<int>(type: "int", nullable: true),
                    UserViewedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserViewedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserViewed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserViewed_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "ResumeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserViewed_ResumeId",
                table: "UserViewed",
                column: "ResumeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserViewed");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "Reviews");
        }
    }
}

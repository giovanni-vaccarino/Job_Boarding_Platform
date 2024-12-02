using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class FixInternshipNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_JobQuestions_JobQuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Jobs_JobId",
                table: "Applications");

            migrationBuilder.DropTable(
                name: "JobQuestions");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.RenameColumn(
                name: "JobId",
                table: "Applications",
                newName: "InternshipId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_StudentId_JobId",
                table: "Applications",
                newName: "IX_Applications_StudentId_InternshipId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_JobId",
                table: "Applications",
                newName: "IX_Applications_InternshipId");

            migrationBuilder.RenameColumn(
                name: "JobQuestionId",
                table: "Answers",
                newName: "InternshipQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_JobQuestionId",
                table: "Answers",
                newName: "IX_Answers_InternshipQuestionId");

            migrationBuilder.CreateTable(
                name: "Internships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApplicationDeadline = table.Column<DateOnly>(type: "date", nullable: false),
                    Location = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    JobCategory = table.Column<int>(type: "int", nullable: true),
                    JobType = table.Column<int>(type: "int", nullable: true),
                    Requirements = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Internships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Internships_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InternshipQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InternshipId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternshipQuestions_Internships_InternshipId",
                        column: x => x.InternshipId,
                        principalTable: "Internships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InternshipQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipQuestions_InternshipId_QuestionId",
                table: "InternshipQuestions",
                columns: new[] { "InternshipId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternshipQuestions_QuestionId",
                table: "InternshipQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_CompanyId",
                table: "Internships",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_InternshipQuestions_InternshipQuestionId",
                table: "Answers",
                column: "InternshipQuestionId",
                principalTable: "InternshipQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Internships_InternshipId",
                table: "Applications",
                column: "InternshipId",
                principalTable: "Internships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_InternshipQuestions_InternshipQuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Internships_InternshipId",
                table: "Applications");

            migrationBuilder.DropTable(
                name: "InternshipQuestions");

            migrationBuilder.DropTable(
                name: "Internships");

            migrationBuilder.RenameColumn(
                name: "InternshipId",
                table: "Applications",
                newName: "JobId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_StudentId_InternshipId",
                table: "Applications",
                newName: "IX_Applications_StudentId_JobId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_InternshipId",
                table: "Applications",
                newName: "IX_Applications_JobId");

            migrationBuilder.RenameColumn(
                name: "InternshipQuestionId",
                table: "Answers",
                newName: "JobQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_InternshipQuestionId",
                table: "Answers",
                newName: "IX_Answers_JobQuestionId");

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ApplicationDeadline = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    JobCategory = table.Column<int>(type: "int", nullable: true),
                    JobType = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Requirements = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JobQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobQuestions_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_JobQuestions_JobId_QuestionId",
                table: "JobQuestions",
                columns: new[] { "JobId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobQuestions_QuestionId",
                table: "JobQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CompanyId",
                table: "Jobs",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_JobQuestions_JobQuestionId",
                table: "Answers",
                column: "JobQuestionId",
                principalTable: "JobQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Jobs_JobId",
                table: "Applications",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

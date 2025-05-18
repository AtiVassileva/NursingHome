using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingHome.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSocialDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocialDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    ResidentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UploadedById = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialDocuments_AspNetUsers_ResidentId",
                        column: x => x.ResidentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SocialDocuments_AspNetUsers_UploadedById",
                        column: x => x.UploadedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocialDocuments_ResidentId",
                table: "SocialDocuments",
                column: "ResidentId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialDocuments_UploadedById",
                table: "SocialDocuments",
                column: "UploadedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialDocuments");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingHome.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRedundantIsPaidProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Payments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Payments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

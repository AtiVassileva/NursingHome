using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NursingHome.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMonthlyFeePaymentRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_MonthlyFeeId",
                table: "Payments");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MonthlyFeeId",
                table: "Payments",
                column: "MonthlyFeeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_MonthlyFeeId",
                table: "Payments");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MonthlyFeeId",
                table: "Payments",
                column: "MonthlyFeeId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMWepApi.Migrations
{
    /// <inheritdoc />
    public partial class AddIsApprovedByManagerToDeals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApprovedByManager",
                table: "Deals",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApprovedByManager",
                table: "Deals");
        }
    }
}

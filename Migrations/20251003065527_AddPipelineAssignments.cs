using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMWepApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPipelineAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedToSalesRepId",
                table: "PipelineStages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedToSrmId",
                table: "PipelineStages",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToSalesRepId",
                table: "PipelineStages");

            migrationBuilder.DropColumn(
                name: "AssignedToSrmId",
                table: "PipelineStages");
        }
    }
}

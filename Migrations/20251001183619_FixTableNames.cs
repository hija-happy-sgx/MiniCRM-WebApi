using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMWepApi.Migrations
{
    /// <inheritdoc />
    public partial class FixTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "Manager",
                columns: table => new
                {
                    ManagerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.ManagerId);
                });

            migrationBuilder.CreateTable(
                name: "PipelineStages",
                columns: table => new
                {
                    StageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StageOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PipelineStages", x => x.StageId);
                });

            migrationBuilder.CreateTable(
                name: "SalesRepManager",
                columns: table => new
                {
                    SrmId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesRepManager", x => x.SrmId);
                    table.ForeignKey(
                        name: "FK_SalesRepManager_Manager_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Manager",
                        principalColumn: "ManagerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesRep",
                columns: table => new
                {
                    SalesRepId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SrmId = table.Column<int>(type: "int", nullable: false),
                    SalesRepManagerSrmId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesRep", x => x.SalesRepId);
                    table.ForeignKey(
                        name: "FK_SalesRep_SalesRepManager_SalesRepManagerSrmId",
                        column: x => x.SalesRepManagerSrmId,
                        principalTable: "SalesRepManager",
                        principalColumn: "SrmId");
                });

            migrationBuilder.CreateTable(
                name: "Leads",
                columns: table => new
                {
                    LeadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedToSalesRep = table.Column<int>(type: "int", nullable: true),
                    SalesRepId = table.Column<int>(type: "int", nullable: true),
                    AssignedToSrm = table.Column<int>(type: "int", nullable: true),
                    CreatedByManager = table.Column<int>(type: "int", nullable: true),
                    ManagerId = table.Column<int>(type: "int", nullable: true),
                    CreatedBySrm = table.Column<int>(type: "int", nullable: true),
                    SalesRepManagerSrmId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.LeadId);
                    table.ForeignKey(
                        name: "FK_Leads_Manager_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Manager",
                        principalColumn: "ManagerId");
                    table.ForeignKey(
                        name: "FK_Leads_SalesRepManager_AssignedToSrm",
                        column: x => x.AssignedToSrm,
                        principalTable: "SalesRepManager",
                        principalColumn: "SrmId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Leads_SalesRepManager_CreatedBySrm",
                        column: x => x.CreatedBySrm,
                        principalTable: "SalesRepManager",
                        principalColumn: "SrmId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Leads_SalesRepManager_SalesRepManagerSrmId",
                        column: x => x.SalesRepManagerSrmId,
                        principalTable: "SalesRepManager",
                        principalColumn: "SrmId");
                    table.ForeignKey(
                        name: "FK_Leads_SalesRep_SalesRepId",
                        column: x => x.SalesRepId,
                        principalTable: "SalesRep",
                        principalColumn: "SalesRepId");
                });

            migrationBuilder.CreateTable(
                name: "Deals",
                columns: table => new
                {
                    DealId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedCloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualCloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeadId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<int>(type: "int", nullable: false),
                    AssignedToSalesRep = table.Column<int>(type: "int", nullable: true),
                    SalesRepId = table.Column<int>(type: "int", nullable: true),
                    AssignedToSrm = table.Column<int>(type: "int", nullable: true),
                    CreatedByManager = table.Column<int>(type: "int", nullable: true),
                    ManagerId = table.Column<int>(type: "int", nullable: true),
                    CreatedBySrm = table.Column<int>(type: "int", nullable: true),
                    SalesRepManagerSrmId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deals", x => x.DealId);
                    table.ForeignKey(
                        name: "FK_Deals_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "LeadId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deals_Manager_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Manager",
                        principalColumn: "ManagerId");
                    table.ForeignKey(
                        name: "FK_Deals_PipelineStages_StageId",
                        column: x => x.StageId,
                        principalTable: "PipelineStages",
                        principalColumn: "StageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deals_SalesRepManager_AssignedToSrm",
                        column: x => x.AssignedToSrm,
                        principalTable: "SalesRepManager",
                        principalColumn: "SrmId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deals_SalesRepManager_CreatedBySrm",
                        column: x => x.CreatedBySrm,
                        principalTable: "SalesRepManager",
                        principalColumn: "SrmId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deals_SalesRepManager_SalesRepManagerSrmId",
                        column: x => x.SalesRepManagerSrmId,
                        principalTable: "SalesRepManager",
                        principalColumn: "SrmId");
                    table.ForeignKey(
                        name: "FK_Deals_SalesRep_SalesRepId",
                        column: x => x.SalesRepId,
                        principalTable: "SalesRep",
                        principalColumn: "SalesRepId");
                });

            migrationBuilder.CreateTable(
                name: "CommunicationLogs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeadId = table.Column<int>(type: "int", nullable: true),
                    DealId = table.Column<int>(type: "int", nullable: true),
                    SalesRepId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunicationLogs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_CommunicationLogs_Deals_DealId",
                        column: x => x.DealId,
                        principalTable: "Deals",
                        principalColumn: "DealId");
                    table.ForeignKey(
                        name: "FK_CommunicationLogs_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "LeadId");
                    table.ForeignKey(
                        name: "FK_CommunicationLogs_SalesRep_SalesRepId",
                        column: x => x.SalesRepId,
                        principalTable: "SalesRep",
                        principalColumn: "SalesRepId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationLogs_DealId",
                table: "CommunicationLogs",
                column: "DealId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationLogs_LeadId",
                table: "CommunicationLogs",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunicationLogs_SalesRepId",
                table: "CommunicationLogs",
                column: "SalesRepId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_AssignedToSrm",
                table: "Deals",
                column: "AssignedToSrm");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_CreatedBySrm",
                table: "Deals",
                column: "CreatedBySrm");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_LeadId",
                table: "Deals",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_ManagerId",
                table: "Deals",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_SalesRepId",
                table: "Deals",
                column: "SalesRepId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_SalesRepManagerSrmId",
                table: "Deals",
                column: "SalesRepManagerSrmId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_StageId",
                table: "Deals",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_AssignedToSrm",
                table: "Leads",
                column: "AssignedToSrm");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_CreatedBySrm",
                table: "Leads",
                column: "CreatedBySrm");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ManagerId",
                table: "Leads",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_SalesRepId",
                table: "Leads",
                column: "SalesRepId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_SalesRepManagerSrmId",
                table: "Leads",
                column: "SalesRepManagerSrmId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesRep_SalesRepManagerSrmId",
                table: "SalesRep",
                column: "SalesRepManagerSrmId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesRepManager_ManagerId",
                table: "SalesRepManager",
                column: "ManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "CommunicationLogs");

            migrationBuilder.DropTable(
                name: "Deals");

            migrationBuilder.DropTable(
                name: "Leads");

            migrationBuilder.DropTable(
                name: "PipelineStages");

            migrationBuilder.DropTable(
                name: "SalesRep");

            migrationBuilder.DropTable(
                name: "SalesRepManager");

            migrationBuilder.DropTable(
                name: "Manager");
        }
    }
}

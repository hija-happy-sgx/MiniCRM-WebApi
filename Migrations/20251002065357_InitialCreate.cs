using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMWepApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunicationLogs_SalesRep_SalesRepId",
                table: "CommunicationLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Manager_ManagerId",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_SalesRepManager_AssignedToSrm",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_SalesRepManager_CreatedBySrm",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_SalesRepManager_SalesRepManagerSrmId",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_SalesRep_SalesRepId",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Manager_ManagerId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_SalesRepManager_AssignedToSrm",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_SalesRepManager_CreatedBySrm",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_SalesRepManager_SalesRepManagerSrmId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_SalesRep_SalesRepId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesRep_SalesRepManager_SalesRepManagerSrmId",
                table: "SalesRep");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesRepManager_Manager_ManagerId",
                table: "SalesRepManager");

            migrationBuilder.DropIndex(
                name: "IX_Leads_ManagerId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_SalesRepId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Deals_ManagerId",
                table: "Deals");

            migrationBuilder.DropIndex(
                name: "IX_Deals_SalesRepId",
                table: "Deals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesRepManager",
                table: "SalesRepManager");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesRep",
                table: "SalesRep");

            migrationBuilder.DropIndex(
                name: "IX_SalesRep_SalesRepManagerSrmId",
                table: "SalesRep");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Manager",
                table: "Manager");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admin",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "SalesRepId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "SalesRepId",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "SalesRepManagerSrmId",
                table: "SalesRep");

            migrationBuilder.RenameTable(
                name: "SalesRepManager",
                newName: "SalesRepManagers");

            migrationBuilder.RenameTable(
                name: "SalesRep",
                newName: "SalesReps");

            migrationBuilder.RenameTable(
                name: "Manager",
                newName: "Managers");

            migrationBuilder.RenameTable(
                name: "Admin",
                newName: "Admins");

            migrationBuilder.RenameIndex(
                name: "IX_SalesRepManager_ManagerId",
                table: "SalesRepManagers",
                newName: "IX_SalesRepManagers_ManagerId");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Managers",
                newName: "Password");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Managers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesRepManagers",
                table: "SalesRepManagers",
                column: "SrmId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesReps",
                table: "SalesReps",
                column: "SalesRepId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Managers",
                table: "Managers",
                column: "ManagerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admins",
                table: "Admins",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_AssignedToSalesRep",
                table: "Leads",
                column: "AssignedToSalesRep");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_CreatedByManager",
                table: "Leads",
                column: "CreatedByManager");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_AssignedToSalesRep",
                table: "Deals",
                column: "AssignedToSalesRep");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_CreatedByManager",
                table: "Deals",
                column: "CreatedByManager");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReps_SrmId",
                table: "SalesReps",
                column: "SrmId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunicationLogs_SalesReps_SalesRepId",
                table: "CommunicationLogs",
                column: "SalesRepId",
                principalTable: "SalesReps",
                principalColumn: "SalesRepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_Managers_CreatedByManager",
                table: "Deals",
                column: "CreatedByManager",
                principalTable: "Managers",
                principalColumn: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_SalesRepManagers_AssignedToSrm",
                table: "Deals",
                column: "AssignedToSrm",
                principalTable: "SalesRepManagers",
                principalColumn: "SrmId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_SalesRepManagers_CreatedBySrm",
                table: "Deals",
                column: "CreatedBySrm",
                principalTable: "SalesRepManagers",
                principalColumn: "SrmId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_SalesRepManagers_SalesRepManagerSrmId",
                table: "Deals",
                column: "SalesRepManagerSrmId",
                principalTable: "SalesRepManagers",
                principalColumn: "SrmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_SalesReps_AssignedToSalesRep",
                table: "Deals",
                column: "AssignedToSalesRep",
                principalTable: "SalesReps",
                principalColumn: "SalesRepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Managers_CreatedByManager",
                table: "Leads",
                column: "CreatedByManager",
                principalTable: "Managers",
                principalColumn: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_SalesRepManagers_AssignedToSrm",
                table: "Leads",
                column: "AssignedToSrm",
                principalTable: "SalesRepManagers",
                principalColumn: "SrmId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_SalesRepManagers_CreatedBySrm",
                table: "Leads",
                column: "CreatedBySrm",
                principalTable: "SalesRepManagers",
                principalColumn: "SrmId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_SalesRepManagers_SalesRepManagerSrmId",
                table: "Leads",
                column: "SalesRepManagerSrmId",
                principalTable: "SalesRepManagers",
                principalColumn: "SrmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_SalesReps_AssignedToSalesRep",
                table: "Leads",
                column: "AssignedToSalesRep",
                principalTable: "SalesReps",
                principalColumn: "SalesRepId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesRepManagers_Managers_ManagerId",
                table: "SalesRepManagers",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "ManagerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReps_SalesRepManagers_SrmId",
                table: "SalesReps",
                column: "SrmId",
                principalTable: "SalesRepManagers",
                principalColumn: "SrmId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunicationLogs_SalesReps_SalesRepId",
                table: "CommunicationLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Managers_CreatedByManager",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_SalesRepManagers_AssignedToSrm",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_SalesRepManagers_CreatedBySrm",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_SalesRepManagers_SalesRepManagerSrmId",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_SalesReps_AssignedToSalesRep",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Managers_CreatedByManager",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_SalesRepManagers_AssignedToSrm",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_SalesRepManagers_CreatedBySrm",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_SalesRepManagers_SalesRepManagerSrmId",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_SalesReps_AssignedToSalesRep",
                table: "Leads");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesRepManagers_Managers_ManagerId",
                table: "SalesRepManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesReps_SalesRepManagers_SrmId",
                table: "SalesReps");

            migrationBuilder.DropIndex(
                name: "IX_Leads_AssignedToSalesRep",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_CreatedByManager",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Deals_AssignedToSalesRep",
                table: "Deals");

            migrationBuilder.DropIndex(
                name: "IX_Deals_CreatedByManager",
                table: "Deals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesReps",
                table: "SalesReps");

            migrationBuilder.DropIndex(
                name: "IX_SalesReps_SrmId",
                table: "SalesReps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesRepManagers",
                table: "SalesRepManagers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Managers",
                table: "Managers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admins",
                table: "Admins");

            migrationBuilder.RenameTable(
                name: "SalesReps",
                newName: "SalesRep");

            migrationBuilder.RenameTable(
                name: "SalesRepManagers",
                newName: "SalesRepManager");

            migrationBuilder.RenameTable(
                name: "Managers",
                newName: "Manager");

            migrationBuilder.RenameTable(
                name: "Admins",
                newName: "Admin");

            migrationBuilder.RenameIndex(
                name: "IX_SalesRepManagers_ManagerId",
                table: "SalesRepManager",
                newName: "IX_SalesRepManager_ManagerId");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Manager",
                newName: "PasswordHash");

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Leads",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalesRepId",
                table: "Leads",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Deals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalesRepId",
                table: "Deals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalesRepManagerSrmId",
                table: "SalesRep",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Manager",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesRep",
                table: "SalesRep",
                column: "SalesRepId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesRepManager",
                table: "SalesRepManager",
                column: "SrmId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manager",
                table: "Manager",
                column: "ManagerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admin",
                table: "Admin",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ManagerId",
                table: "Leads",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Leads_SalesRepId",
                table: "Leads",
                column: "SalesRepId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_ManagerId",
                table: "Deals",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_SalesRepId",
                table: "Deals",
                column: "SalesRepId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesRep_SalesRepManagerSrmId",
                table: "SalesRep",
                column: "SalesRepManagerSrmId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunicationLogs_SalesRep_SalesRepId",
                table: "CommunicationLogs",
                column: "SalesRepId",
                principalTable: "SalesRep",
                principalColumn: "SalesRepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_Manager_ManagerId",
                table: "Deals",
                column: "ManagerId",
                principalTable: "Manager",
                principalColumn: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_SalesRepManager_AssignedToSrm",
                table: "Deals",
                column: "AssignedToSrm",
                principalTable: "SalesRepManager",
                principalColumn: "SrmId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_SalesRepManager_CreatedBySrm",
                table: "Deals",
                column: "CreatedBySrm",
                principalTable: "SalesRepManager",
                principalColumn: "SrmId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_SalesRepManager_SalesRepManagerSrmId",
                table: "Deals",
                column: "SalesRepManagerSrmId",
                principalTable: "SalesRepManager",
                principalColumn: "SrmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_SalesRep_SalesRepId",
                table: "Deals",
                column: "SalesRepId",
                principalTable: "SalesRep",
                principalColumn: "SalesRepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Manager_ManagerId",
                table: "Leads",
                column: "ManagerId",
                principalTable: "Manager",
                principalColumn: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_SalesRepManager_AssignedToSrm",
                table: "Leads",
                column: "AssignedToSrm",
                principalTable: "SalesRepManager",
                principalColumn: "SrmId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_SalesRepManager_CreatedBySrm",
                table: "Leads",
                column: "CreatedBySrm",
                principalTable: "SalesRepManager",
                principalColumn: "SrmId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_SalesRepManager_SalesRepManagerSrmId",
                table: "Leads",
                column: "SalesRepManagerSrmId",
                principalTable: "SalesRepManager",
                principalColumn: "SrmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_SalesRep_SalesRepId",
                table: "Leads",
                column: "SalesRepId",
                principalTable: "SalesRep",
                principalColumn: "SalesRepId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesRep_SalesRepManager_SalesRepManagerSrmId",
                table: "SalesRep",
                column: "SalesRepManagerSrmId",
                principalTable: "SalesRepManager",
                principalColumn: "SrmId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesRepManager_Manager_ManagerId",
                table: "SalesRepManager",
                column: "ManagerId",
                principalTable: "Manager",
                principalColumn: "ManagerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

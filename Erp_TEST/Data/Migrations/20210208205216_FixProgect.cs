using Microsoft.EntityFrameworkCore.Migrations;

namespace Erp_TEST.Data.Migrations
{
    public partial class FixProgect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbFiles_Project_ProjectId",
                table: "DbFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Types_ProjectTypeId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_ListUsers_UserId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Project_ProjectId",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project",
                table: "Project");

            migrationBuilder.RenameTable(
                name: "Project",
                newName: "Projects");

            migrationBuilder.RenameIndex(
                name: "IX_Project_UserId",
                table: "Projects",
                newName: "IX_Projects_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Project_ProjectTypeId",
                table: "Projects",
                newName: "IX_Projects_ProjectTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DbFiles_Projects_ProjectId",
                table: "DbFiles",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Types_ProjectTypeId",
                table: "Projects",
                column: "ProjectTypeId",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ListUsers_UserId",
                table: "Projects",
                column: "UserId",
                principalTable: "ListUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Projects_ProjectId",
                table: "Skills",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbFiles_Projects_ProjectId",
                table: "DbFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Types_ProjectTypeId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ListUsers_UserId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Projects_ProjectId",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Project");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_UserId",
                table: "Project",
                newName: "IX_Project_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_ProjectTypeId",
                table: "Project",
                newName: "IX_Project_ProjectTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project",
                table: "Project",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DbFiles_Project_ProjectId",
                table: "DbFiles",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Types_ProjectTypeId",
                table: "Project",
                column: "ProjectTypeId",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_ListUsers_UserId",
                table: "Project",
                column: "UserId",
                principalTable: "ListUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Project_ProjectId",
                table: "Skills",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

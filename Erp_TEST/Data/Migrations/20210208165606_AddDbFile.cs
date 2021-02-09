using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Erp_TEST.Data.Migrations
{
    public partial class AddDbFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "DbFiles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "DbFiles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DbFiles_ProjectId",
                table: "DbFiles",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_DbFiles_Project_ProjectId",
                table: "DbFiles",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbFiles_Project_ProjectId",
                table: "DbFiles");

            migrationBuilder.DropIndex(
                name: "IX_DbFiles_ProjectId",
                table: "DbFiles");

            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "DbFiles");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "DbFiles");
        }
    }
}

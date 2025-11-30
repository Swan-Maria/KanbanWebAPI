using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KanbanWebAPI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class DeleteTaskItemUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskItemUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskItemUser",
                columns: table => new
                {
                    TasksTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItemUser", x => new { x.TasksTaskId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_TaskItemUser_Task_TasksTaskId",
                        column: x => x.TasksTaskId,
                        principalTable: "Task",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskItemUser_User_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskItemUser_UsersUserId",
                table: "TaskItemUser",
                column: "UsersUserId");
        }
    }
}

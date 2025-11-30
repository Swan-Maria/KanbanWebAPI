using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KanbanWebAPI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class DeleteTaskAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignment_Task_TasksTaskId",
                table: "TaskAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignment_User_UsersUserId",
                table: "TaskAssignment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskAssignment",
                table: "TaskAssignment");

            migrationBuilder.RenameTable(
                name: "TaskAssignment",
                newName: "TaskItemUser");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAssignment_UsersUserId",
                table: "TaskItemUser",
                newName: "IX_TaskItemUser_UsersUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskItemUser",
                table: "TaskItemUser",
                columns: new[] { "TasksTaskId", "UsersUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItemUser_Task_TasksTaskId",
                table: "TaskItemUser",
                column: "TasksTaskId",
                principalTable: "Task",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItemUser_User_UsersUserId",
                table: "TaskItemUser",
                column: "UsersUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItemUser_Task_TasksTaskId",
                table: "TaskItemUser");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskItemUser_User_UsersUserId",
                table: "TaskItemUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskItemUser",
                table: "TaskItemUser");

            migrationBuilder.RenameTable(
                name: "TaskItemUser",
                newName: "TaskAssignment");

            migrationBuilder.RenameIndex(
                name: "IX_TaskItemUser_UsersUserId",
                table: "TaskAssignment",
                newName: "IX_TaskAssignment_UsersUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskAssignment",
                table: "TaskAssignment",
                columns: new[] { "TasksTaskId", "UsersUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignment_Task_TasksTaskId",
                table: "TaskAssignment",
                column: "TasksTaskId",
                principalTable: "Task",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignment_User_UsersUserId",
                table: "TaskAssignment",
                column: "UsersUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

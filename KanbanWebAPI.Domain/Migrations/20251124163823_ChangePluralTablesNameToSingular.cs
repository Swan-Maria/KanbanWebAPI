using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KanbanWebAPI.Domain.Migrations
{
    /// <inheritdoc />
    public partial class ChangePluralTablesNameToSingular : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Teams_TeamId",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Columns_Boards_BoardId",
                table: "Columns");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Tasks_TasksTaskId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignments_Users_UsersUserId",
                table: "TaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAudits_Tasks_TaskId",
                table: "TaskAudits");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAudits_Tasks_TaskItemTaskId",
                table: "TaskAudits");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAudits_Users_ChengedByUserId",
                table: "TaskAudits");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAudits_Users_UserId",
                table: "TaskAudits");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Columns_ColumnId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Teams_TeamsTeamId",
                table: "TeamMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_Users_UsersUserId",
                table: "TeamMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teams",
                table: "Teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamMembers",
                table: "TeamMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskAudits",
                table: "TaskAudits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskAssignments",
                table: "TaskAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Columns",
                table: "Columns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Boards",
                table: "Boards");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Teams",
                newName: "Team");

            migrationBuilder.RenameTable(
                name: "TeamMembers",
                newName: "TeamMember");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Task");

            migrationBuilder.RenameTable(
                name: "TaskAudits",
                newName: "TaskAudit");

            migrationBuilder.RenameTable(
                name: "TaskAssignments",
                newName: "TaskAssignment");

            migrationBuilder.RenameTable(
                name: "Columns",
                newName: "Column");

            migrationBuilder.RenameTable(
                name: "Boards",
                newName: "Board");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "User",
                newName: "IX_User_Email");

            migrationBuilder.RenameIndex(
                name: "IX_TeamMembers_UsersUserId",
                table: "TeamMember",
                newName: "IX_TeamMember_UsersUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ColumnId",
                table: "Task",
                newName: "IX_Task_ColumnId");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "TaskAudit",
                newName: "TaskItemId");

            migrationBuilder.RenameColumn(
                name: "ChengedByUserId",
                table: "TaskAudit",
                newName: "CreateByUserId");

            migrationBuilder.RenameColumn(
                name: "ChangedAt",
                table: "TaskAudit",
                newName: "CreateAt");

            migrationBuilder.RenameColumn(
                name: "ChangeDescription",
                table: "TaskAudit",
                newName: "Action");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAudits_UserId",
                table: "TaskAudit",
                newName: "IX_TaskAudit_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAudits_TaskItemTaskId",
                table: "TaskAudit",
                newName: "IX_TaskAudit_TaskItemTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAudits_TaskId",
                table: "TaskAudit",
                newName: "IX_TaskAudit_TaskItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAudits_ChengedByUserId",
                table: "TaskAudit",
                newName: "IX_TaskAudit_CreateByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAssignments_UsersUserId",
                table: "TaskAssignment",
                newName: "IX_TaskAssignment_UsersUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Columns_BoardId",
                table: "Column",
                newName: "IX_Column_BoardId");

            migrationBuilder.RenameIndex(
                name: "IX_Boards_TeamId",
                table: "Board",
                newName: "IX_Board_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Team",
                table: "Team",
                column: "TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamMember",
                table: "TeamMember",
                columns: new[] { "TeamsTeamId", "UsersUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Task",
                table: "Task",
                column: "TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskAudit",
                table: "TaskAudit",
                column: "AuditId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskAssignment",
                table: "TaskAssignment",
                columns: new[] { "TasksTaskId", "UsersUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Column",
                table: "Column",
                column: "ColumnId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Board",
                table: "Board",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Board_Team_TeamId",
                table: "Board",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Column_Board_BoardId",
                table: "Column",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "BoardId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Column_ColumnId",
                table: "Task",
                column: "ColumnId",
                principalTable: "Column",
                principalColumn: "ColumnId",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAudit_Task_TaskItemId",
                table: "TaskAudit",
                column: "TaskItemId",
                principalTable: "Task",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAudit_Task_TaskItemTaskId",
                table: "TaskAudit",
                column: "TaskItemTaskId",
                principalTable: "Task",
                principalColumn: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAudit_User_CreateByUserId",
                table: "TaskAudit",
                column: "CreateByUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAudit_User_UserId",
                table: "TaskAudit",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMember_Team_TeamsTeamId",
                table: "TeamMember",
                column: "TeamsTeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMember_User_UsersUserId",
                table: "TeamMember",
                column: "UsersUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Board_Team_TeamId",
                table: "Board");

            migrationBuilder.DropForeignKey(
                name: "FK_Column_Board_BoardId",
                table: "Column");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Column_ColumnId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignment_Task_TasksTaskId",
                table: "TaskAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignment_User_UsersUserId",
                table: "TaskAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAudit_Task_TaskItemId",
                table: "TaskAudit");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAudit_Task_TaskItemTaskId",
                table: "TaskAudit");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAudit_User_CreateByUserId",
                table: "TaskAudit");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAudit_User_UserId",
                table: "TaskAudit");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMember_Team_TeamsTeamId",
                table: "TeamMember");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMember_User_UsersUserId",
                table: "TeamMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamMember",
                table: "TeamMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Team",
                table: "Team");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskAudit",
                table: "TaskAudit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskAssignment",
                table: "TaskAssignment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Task",
                table: "Task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Column",
                table: "Column");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Board",
                table: "Board");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "TeamMember",
                newName: "TeamMembers");

            migrationBuilder.RenameTable(
                name: "Team",
                newName: "Teams");

            migrationBuilder.RenameTable(
                name: "TaskAudit",
                newName: "TaskAudits");

            migrationBuilder.RenameTable(
                name: "TaskAssignment",
                newName: "TaskAssignments");

            migrationBuilder.RenameTable(
                name: "Task",
                newName: "Tasks");

            migrationBuilder.RenameTable(
                name: "Column",
                newName: "Columns");

            migrationBuilder.RenameTable(
                name: "Board",
                newName: "Boards");

            migrationBuilder.RenameIndex(
                name: "IX_User_Email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameIndex(
                name: "IX_TeamMember_UsersUserId",
                table: "TeamMembers",
                newName: "IX_TeamMembers_UsersUserId");

            migrationBuilder.RenameColumn(
                name: "TaskItemId",
                table: "TaskAudits",
                newName: "TaskId");

            migrationBuilder.RenameColumn(
                name: "CreateByUserId",
                table: "TaskAudits",
                newName: "ChengedByUserId");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "TaskAudits",
                newName: "ChangedAt");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "TaskAudits",
                newName: "ChangeDescription");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAudit_UserId",
                table: "TaskAudits",
                newName: "IX_TaskAudits_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAudit_TaskItemTaskId",
                table: "TaskAudits",
                newName: "IX_TaskAudits_TaskItemTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAudit_TaskItemId",
                table: "TaskAudits",
                newName: "IX_TaskAudits_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAudit_CreateByUserId",
                table: "TaskAudits",
                newName: "IX_TaskAudits_ChengedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskAssignment_UsersUserId",
                table: "TaskAssignments",
                newName: "IX_TaskAssignments_UsersUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Task_ColumnId",
                table: "Tasks",
                newName: "IX_Tasks_ColumnId");

            migrationBuilder.RenameIndex(
                name: "IX_Column_BoardId",
                table: "Columns",
                newName: "IX_Columns_BoardId");

            migrationBuilder.RenameIndex(
                name: "IX_Board_TeamId",
                table: "Boards",
                newName: "IX_Boards_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamMembers",
                table: "TeamMembers",
                columns: new[] { "TeamsTeamId", "UsersUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teams",
                table: "Teams",
                column: "TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskAudits",
                table: "TaskAudits",
                column: "AuditId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskAssignments",
                table: "TaskAssignments",
                columns: new[] { "TasksTaskId", "UsersUserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Columns",
                table: "Columns",
                column: "ColumnId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Boards",
                table: "Boards",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Teams_TeamId",
                table: "Boards",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Columns_Boards_BoardId",
                table: "Columns",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "BoardId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Tasks_TasksTaskId",
                table: "TaskAssignments",
                column: "TasksTaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignments_Users_UsersUserId",
                table: "TaskAssignments",
                column: "UsersUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAudits_Tasks_TaskId",
                table: "TaskAudits",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAudits_Tasks_TaskItemTaskId",
                table: "TaskAudits",
                column: "TaskItemTaskId",
                principalTable: "Tasks",
                principalColumn: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAudits_Users_ChengedByUserId",
                table: "TaskAudits",
                column: "ChengedByUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAudits_Users_UserId",
                table: "TaskAudits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Columns_ColumnId",
                table: "Tasks",
                column: "ColumnId",
                principalTable: "Columns",
                principalColumn: "ColumnId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Teams_TeamsTeamId",
                table: "TeamMembers",
                column: "TeamsTeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_Users_UsersUserId",
                table: "TeamMembers",
                column: "UsersUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

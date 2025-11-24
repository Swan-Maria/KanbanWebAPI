using Bogus;

using KanbanWebAPI.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace KanbanWebAPI.Domain;

public static class SetDb
{
    public static void Seed(AppDbContext context)
    {
        var random = new Random();

        // users
        if (!context.User.Any())
        {
            var users = new Faker<User>()
                .RuleFor(u => u.UserId, f => Guid.NewGuid())
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .Generate(10);

            context.User.AddRange(users);
            context.SaveChanges();
        }

        var allUsers = context.User.ToList();

        // teams
        if (!context.Team.Any())
        {
            var teams = new Faker<Team>()
                .RuleFor(t => t.TeamId, f => Guid.NewGuid())
                .RuleFor(t => t.TeamName, f => f.Company.CompanyName())
                .Generate(3);

            context.Team.AddRange(teams);
            context.SaveChanges();
        }

        var allTeams = context.Team.ToList();

        // boards
        if (!context.Board.Any())
        {
            var boards = new Faker<Board>()
                .RuleFor(b => b.BoardId, f => Guid.NewGuid())
                .RuleFor(b => b.BoardName, f => f.Company.CatchPhrase())
                .RuleFor(b => b.TeamId, f => f.PickRandom(allTeams).TeamId)
                .Generate(5);

            context.Board.AddRange(boards);
            context.SaveChanges();
        }

        var allBoards = context.Board.Include(b => b.Columns).ToList();

        // columns
        if (!context.Column.Any())
        {
            var columnNames = new[] { "To Do", "In Progress", "Review", "Done" };
            var columns = new List<Column>();

            foreach (var board in allBoards)
            {
                // Backlog
                columns.Add(new Column
                {
                    ColumnId = Guid.NewGuid(),
                    ColumnName = "Backlog",
                    BoardId = board.BoardId
                });

                // other columns
                foreach (var name in columnNames)
                {
                    columns.Add(new Column
                    {
                        ColumnId = Guid.NewGuid(),
                        ColumnName = name,
                        BoardId = board.BoardId
                    });
                }
            }

            context.Column.AddRange(columns);
            context.SaveChanges();
        }

        var allColumns = context.Column.ToList();
        var backlogColumns = allColumns.Where(c => c.ColumnName == "Backlog").ToList();
        var optionalColumns = allColumns.Where(c => c.ColumnName != "Backlog").ToList();


        // tasks
        if (!context.Task.Any())
        {
            // All tasks create at Backlog
            var tasks = new Faker<TaskItem>()
                .RuleFor(t => t.TaskId, f => Guid.NewGuid())
                .RuleFor(t => t.Title, f => f.Hacker.Verb() + " " + f.Hacker.Noun())
                .RuleFor(t => t.Description, f => f.Lorem.Sentence())
                .RuleFor(t => t.ColumnId, f => f.PickRandom(backlogColumns).ColumnId)
                .Generate(20);

            context.Task.AddRange(tasks);
            context.SaveChanges();

            // task assignment
            foreach (var task in tasks)
            {
                var assignedUsers = allUsers.OrderBy(u => Guid.NewGuid())
                    .Take(random.Next(1, 4))
                    .ToList();

                foreach (var user in assignedUsers)
                {
                    task.Users.Add(user);
                }
            }
            context.SaveChanges();

            // task audit (history)
            var audits = new Faker<TaskAudit>()
                .RuleFor(a => a.AuditId, f => Guid.NewGuid())
                .RuleFor(a => a.TaskItemId, f => f.PickRandom(tasks).TaskId)
                .RuleFor(a => a.Action, f => f.Lorem.Sentence())
                .RuleFor(a => a.CreateAt, f => f.Date.Past(1))
                .RuleFor(a => a.CreateByUserId, f => f.PickRandom(allUsers).UserId)
                .Generate(20);

            context.TaskAudit.AddRange(audits);
            context.SaveChanges();

            // Random remove tasks from Backlog to optional columns
            foreach (var task in tasks)
            {
                if (random.NextDouble() > 0.5 && optionalColumns.Any())
                {
                    task.ColumnId = optionalColumns[random.Next(optionalColumns.Count)].ColumnId;
                }
            }

            context.SaveChanges();
        }
    }
}
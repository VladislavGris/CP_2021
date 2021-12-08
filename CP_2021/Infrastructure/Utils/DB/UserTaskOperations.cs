using CP_2021.Data;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Models.ProcedureResuts;
using CP_2021.Models.ProcedureResuts.Plan;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CP_2021.Infrastructure.Utils.DB
{
    internal class UserTaskOperations
    {
        static UserTaskOperations()
        {
            _context = ApplicationUnitSingleton.GetApplicationContext();
        }

        private static ApplicationContext _context;

        public static IEnumerable<TaskReport> GetAllGivenTasksByUser(Guid id)
        {
            return _context.TaskReport.FromSqlRaw(Procedures.GetAllGivenTasksByUser, id);
        }

        public static IEnumerable<TaskReport> GetAllCompletedTasksByUser(Guid id)
        {
            return _context.TaskReport.FromSqlRaw(Procedures.GetAllCompletedTasksByUser, id);
        }

        public static IEnumerable<TaskReport> GetAllIncompetedTasksByUser(Guid id)
        {
            return _context.TaskReport.FromSqlRaw(Procedures.GetAllIncompetedTasksByUser, id);
        }

        public static IEnumerable<TaskReport> GetAllToDoTasks(Guid id)
        {
            return _context.TaskReport.FromSqlRaw(Procedures.GetAllToDoTasks, id);
        }

        public static IEnumerable<TaskReport> GetAllToDoTasksNoReport(Guid id)
        {
            return _context.TaskReport.FromSqlRaw(Procedures.GetAllToDoTasksNoReport, id);
        }

        public static IEnumerable<TaskReport> GetAllToDoTasksWithReport(Guid id)
        {
            return _context.TaskReport.FromSqlRaw(Procedures.GetAllToDoTasksWithReport, id);
        }

        public static IEnumerable<UserNames> GetUserNames(Guid currentUserId)
        {
            return _context.UserNames.FromSqlRaw(Procedures.GetUserNames, currentUserId);
        }

        public static void CreateNewTask(TaskReport task, Guid givenUserId)
        {
            _context.Database.ExecuteSqlRaw(Procedures.CreateNewTask, givenUserId, task.ToId, task.Header, task.CompleteDate, task.Description);
        }

        public static void DeleteTaskById(Guid taskId)
        {
            _context.Database.ExecuteSqlRaw(Procedures.DropUserTask, taskId);
        }

        public static void AddReportToTask(Guid taskId, string description)
        {
            _context.Database.ExecuteSqlRaw(Procedures.AddReportToTask, taskId, description);
        }
    }
}

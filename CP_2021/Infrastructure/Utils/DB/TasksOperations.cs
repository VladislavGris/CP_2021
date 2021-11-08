using CP_2021.Data;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Models.ProcedureResuts.Plan;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CP_2021.Infrastructure.Utils.DB
{
    internal class TasksOperations
    {
        private static ApplicationContext _context;

        public static List<Task_Hierarchy_Formatting> GetTasksByParent(Guid? parentId)
        {
            return _context.Task_Hierarchy_Formatting.FromSqlRaw(Procedures.GetTasksByParent, parentId).AsNoTracking().ToList();
        }

        public static Task_Hierarchy_Formatting InsertEmptyTask(Guid? parentId, int lineOrder)
        {
            return _context.Task_Hierarchy_Formatting.FromSqlRaw(Procedures.InsertEmptyTask, parentId, lineOrder).AsNoTracking().ToList().FirstOrDefault();
        }

        static TasksOperations()
        {
            _context = ApplicationUnitSingleton.GetInstance().context;
        }
    }
}

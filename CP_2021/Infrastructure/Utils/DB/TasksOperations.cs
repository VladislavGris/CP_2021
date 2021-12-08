using CP_2021.Data;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Models.DataWindowEntities;
using CP_2021.Models.ProcedureResuts.Plan;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CP_2021.Infrastructure.Utils.DB
{
    internal class TasksOperations
    {
        private static ApplicationContext _context;

        public static IEnumerable<Task_Hierarchy_Formatting> GetTasksByParent(Guid? parentId)
        {
            var result = _context.Task_Hierarchy_Formatting.FromSqlRaw(Procedures.GetTasksByParent, parentId).AsNoTracking();
            return result;
        }

        public static IEnumerable<Task_Hierarchy_Formatting> GetTasksByParentNULL()
        {
            var result = _context.Task_Hierarchy_Formatting.FromSqlRaw(Procedures.GetTasksByParentNULL).AsNoTracking();
            return result;

        }

        public static Task_Hierarchy_Formatting InsertEmptyTask(Guid? parentId, int lineOrder)
        {
            return _context.Task_Hierarchy_Formatting.FromSqlRaw(Procedures.InsertEmptyTask, parentId, lineOrder).AsNoTracking().ToList().FirstOrDefault();
        }

        public static Task_Hierarchy_Formatting GetTaskById(Guid id)
        {
            return _context.Task_Hierarchy_Formatting.FromSqlRaw(Procedures.GetTaskById, id).AsNoTracking().AsEnumerable().FirstOrDefault();
        }

        public static void UpdateProductionPlan(Guid id, string incDoc, string taskName, int? count, string specCost, string note, bool expanded, short completion, string editingBy)
        {
            _context.Database.ExecuteSqlRaw(Procedures.UpdateProductionPlan, id, incDoc, taskName, count, specCost, note, expanded, completion, editingBy);
        }

        public static void DropTaskBtId(Guid id)
        {
            _context.Database.ExecuteSqlRaw(Procedures.DropTaskById, id);
        }

        public static void UpTask(Guid taskToUp, Guid taskToDown)
        {
            _context.Database.ExecuteSqlRaw(Procedures.UpTask, taskToUp, taskToDown);
        }

        public static void DownTask(Guid taskToDown, Guid taskToUp)
        {
            _context.Database.ExecuteSqlRaw(Procedures.DownTask, taskToDown, taskToUp);
        }

        public static void LevelUpTask(Guid id, int lineOrder, Guid? parentId, int parentLineOrder)
        {
            _context.Database.ExecuteSqlRaw(Procedures.LevelUpTask, id, lineOrder, parentId, parentLineOrder);
        }

        public static void LevelDownTask(Guid id, int lineOrder, Guid? parentId, Guid newParentId)
        {
            _context.Database.ExecuteSqlRaw(Procedures.LevelDownTask, id, lineOrder, parentId, newParentId);
        }

        #region DataWindows
        #region Complectation
        public static ComplectationWindowEntity GetComplectationWindowEntity(Guid taskId)
        {
            return _context.ComplectationData.FromSqlRaw(Procedures.GetComplectationWindowData, taskId).AsNoTracking().AsEnumerable().FirstOrDefault();
        }
        public static void UpdateComplectationWindowData(Guid id, string complectation, string stateNumber, DateTime? complectationDate, DateTime? onStorageDate, double? percentage, string rack, string shelf, string note)
        {
            _context.Database.ExecuteSqlRaw(Procedures.UpdateComplectationWindowData, id, complectation, complectationDate, percentage, onStorageDate, stateNumber, rack, shelf, note);
        }
        #endregion


        #endregion

        static TasksOperations()
        {
            Debug.WriteLine("Constructor start:" + DateTime.Now.TimeOfDay);
            _context = ApplicationUnitSingleton.GetApplicationContext();
            Debug.WriteLine("Constructor end:" + DateTime.Now.TimeOfDay);
        }
    }
}

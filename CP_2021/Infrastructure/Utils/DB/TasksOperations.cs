using CP_2021.Data;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Models.DataWindowEntities;
using CP_2021.Models.ProcedureResuts;
using CP_2021.Models.ProcedureResuts.Plan;
using CP_2021.Models.ViewEntities;
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

        public static Task_Hierarchy_Formatting PasteTask(Guid taskId, Guid? parentId, int lineOrder)
        {
            return _context.Task_Hierarchy_Formatting.FromSqlRaw(Procedures.PasteTask, taskId, parentId, lineOrder).AsNoTracking().AsEnumerable().FirstOrDefault();
        }

        public static void SetBold(Guid taskId, bool isBold)
        {
            _context.Database.ExecuteSqlRaw(Procedures.SetBold, taskId, isBold);
        }

        public static void SetItalic(Guid taskId,bool isItalic)
        {
            _context.Database.ExecuteSqlRaw(Procedures.SetItalic, taskId, isItalic);
        }

        public static void SetFontSize(Guid taskId, int fontSize)
        {
            _context.Database.ExecuteSqlRaw(Procedures.SetFontSize, taskId, fontSize);
        }

        public static IEnumerable<HeadTasks> GetHeadTasks()
        {
            return _context.HeadTasks.FromSqlRaw(Procedures.GetHeadTasks).AsNoTracking().AsEnumerable();
        }

        public static IEnumerable<ManufactureNames> GetManufactureNames()
        {
            return _context.ManufactureNames.FromSqlRaw(Procedures.GetManufactureNames).AsNoTracking().AsEnumerable();
        }

        public static IEnumerable<CoopWork> GetCoopWork()
        {
            return _context.CoopWork.FromSqlRaw(Procedures.GetCoopWork).AsNoTracking().AsEnumerable();
        }

        public static IEnumerable<SpecificationsOnControl> GetSpecificationsOnControl()
        {
            return _context.SpecOnControl.FromSqlRaw(Procedures.GetSpecificationsOnControl).AsNoTracking().AsEnumerable();
        }

        public static IEnumerable<SpecificationsInVipisk> GetSpecificationsInVipisk()
        {
            return _context.SpecInVipisk.FromSqlRaw(Procedures.GetSpecificationsInVipisk).AsNoTracking().AsEnumerable();
        }

        public static IEnumerable<InProgressView> GetTasksInProgress()
        {
            return _context.InProductionView.FromSqlRaw(Procedures.GetTasksInProgress).AsNoTracking().AsEnumerable();
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

        #region ConsumeAct
        
        public static ConsumeActWindowEntity GetConsumeActWindowEntity(Guid taskId)
        {
            return _context.ConsumeActData.FromSqlRaw(Procedures.GetConsumeActData, taskId).AsNoTracking().AsEnumerable().FirstOrDefault();
        }

        public static void UpdateConsumeActData(Guid id, string actNumber, DateTime? actDate, bool actCreation, bool byAct, string note)
        {
            _context.Database.ExecuteSqlRaw(Procedures.UpdateConsumeAct, id, actNumber, actDate, actCreation, byAct, note);
        }

        #endregion

        #region Documentation
        
        public static DocumentWindowEntity GetDocumentationData(Guid taskId)
        {
            return _context.DocumentationData.FromSqlRaw(Procedures.GetDocumentData, taskId).AsNoTracking().AsEnumerable().FirstOrDefault();
        }

        public static void UpdateDocumentData(Guid taskId, string managDoc, DateTime? vishDate, DateTime? realDate)
        {
            _context.Database.ExecuteSqlRaw(Procedures.UpdateDocumentData, taskId, managDoc, vishDate, realDate);
        }

        #endregion

        #region Giving

        public static GivingWindowEntity GetGivingData(Guid taskId)
        {
            return _context.GivingData.FromSqlRaw(Procedures.GetGivingData, taskId).AsNoTracking().AsEnumerable().FirstOrDefault();
        }

        public static void UpdateGivingData(bool state, string bill, string report, string returnReport, DateTime? receivingDate, bool returnGiving, string purchaseGoods, string note, Guid taskId)
        {
            _context.Database.ExecuteSqlRaw(Procedures.UpdateGivingData,state, bill, report, returnReport, receivingDate, returnGiving, purchaseGoods, note, taskId);
        }

        #endregion

        #region In_Production

        public static InProductionWindowEntity GetInProductionData(Guid taskId)
        {
            return _context.InProductionData.FromSqlRaw(Procedures.GetInProductionData, taskId).AsNoTracking().AsEnumerable().FirstOrDefault();
        }

        public static void UpdateInProductionData(Guid id, string number, DateTime? givingDate, string execName, string instExecName, DateTime? completionDate, DateTime? projectedDate, string note)
        {
            _context.Database.ExecuteSqlRaw(Procedures.UpdateInProductionData, id, number, givingDate, execName, instExecName, completionDate, projectedDate, note);
        }

        #endregion

        #region LaborCosts

        public static LaborCostsWindowEntity GetLaborCostsData(Guid taskId)
        {
            return _context.LaborCostsData.FromSqlRaw(Procedures.GetLaborCostsData, taskId).AsNoTracking().AsEnumerable().FirstOrDefault();
        }

        public static void UpdateLaborCostsData(Guid id, string project, string subcont, string markingRank, float? markingHours, string assemblyRank, float? assemblyHours, string settingsRank, float? settingsHours, DateTime? date)
        {
            _context.Database.ExecuteSqlRaw(Procedures.UpdateLaborCostsData, id, project, subcont, markingRank, markingHours, assemblyRank, assemblyHours,settingsRank, settingsHours, date);
        }

        #endregion

        #region Manufacture

        public static ManufactureWindowEnity GetManufactureData(Guid taskId)
        {
            return _context.ManufactureData.FromSqlRaw(Procedures.GetManufactureData, taskId).AsNoTracking().AsEnumerable().FirstOrDefault();
        }

        public static void UpdateManufactureData(Guid id, string name, string letterNum, string specNum, bool onControl, bool vipiskSpec, DateTime? predictDate, DateTime? factDate, bool execAct, DateTime? execTerm, string calendarDays, string workingDays, string note)
        {
            _context.Database.ExecuteSqlRaw(Procedures.UpdateManufactureData, id, name, letterNum, specNum, onControl, vipiskSpec, predictDate,factDate, execAct, execTerm, calendarDays, workingDays, note);
        }

        #endregion

        #region Payment

        public static PaymentWindowEntity GetPaymentData(Guid taskId)
        {
            return _context.PaymentData.FromSqlRaw(Procedures.GetPaymentData, taskId).AsNoTracking().AsEnumerable().FirstOrDefault();
        }

        public static void UpdatePaymentData(Guid id, string contract, string specSum, string project, string price, string note, bool isFP, string FPSum, DateTime? FPDate, bool isSP, string SPSum, DateTime? SPDate, bool isFuP, string FuPSum, DateTime? FuPDate)
        {
            _context.Database.ExecuteSqlRaw(Procedures.UpdatePaymentData, id, contract, specSum,project, price, note, isFP, FPSum, FPDate, isSP, SPSum, SPDate, isFuP, FuPSum, FuPDate);
        }

        #endregion

        #region TimedGiving

        public static TimedGivingWindowEntity GetTimedGivingData(Guid taskId)
        {
            return _context.TimedGivings.FromSqlRaw(Procedures.GetTimedGivingData,taskId).AsNoTracking().AsEnumerable().FirstOrDefault();
        }

        public static void UpdateTimedGivingData(Guid id, bool isTimedGiving, bool isSKBCheck, bool isOECStorage, string skbNumber, string fio, DateTime? givingDate, string note)
        {
            _context.Database.ExecuteSqlRaw(Procedures.UpdateTimedGivingData, id, isTimedGiving, isSKBCheck, isOECStorage, skbNumber, fio, givingDate, note);
        }

        #endregion
        #endregion

        #region Search

        public static IEnumerable<SearchProcResult> SearchProductionTask(string searchString)
        {
            SearchProcResult task = _context.SearchResults.FromSqlRaw(Procedures.SearchProductionPlan, searchString).AsNoTracking().AsEnumerable().FirstOrDefault();
            return _context.SearchResults.FromSqlRaw(Procedures.SearchProductionPlan, searchString).AsNoTracking();
        }

        public static IEnumerable<SearchProcResult> SearchAct(string searchString)
        {
            return _context.SearchResults.FromSqlRaw(Procedures.SearchAct, searchString).AsNoTracking();
        }

        public static IEnumerable<SearchProcResult> SearchComplectation(string searchString)
        {
            return _context.SearchResults.FromSqlRaw(Procedures.SearchComplectation, searchString).AsNoTracking();
        }

        public static IEnumerable<SearchProcResult> SearchGiving(string searchString)
        {
            return _context.SearchResults.FromSqlRaw(Procedures.SearchGiving, searchString).AsNoTracking();
        }

        public static IEnumerable<SearchProcResult> SearchIn_Production(string searchString)
        {
            return _context.SearchResults.FromSqlRaw(Procedures.SearchIn_Production, searchString).AsNoTracking();
        }

        public static IEnumerable<SearchProcResult> SearchLaborCosts(string searchString)
        {
            return _context.SearchResults.FromSqlRaw(Procedures.SearchIn_Production, searchString).AsNoTracking();
        }

        public static IEnumerable<SearchProcResult> SearchManufacture(string searchString)
        {
            return _context.SearchResults.FromSqlRaw(Procedures.SearchManufacture, searchString).AsNoTracking();
        }

        public static IEnumerable<SearchProcResult> SearchPayment(string searchString)
        {
            return _context.SearchResults.FromSqlRaw(Procedures.SearchPayment, searchString).AsNoTracking();
        }

        public static IEnumerable<SearchGoTo> GetAllParents(Guid taskId)
        {
            return _context.SearchGoTo.FromSqlRaw(Procedures.GetAllParents, taskId).AsNoTracking();
        }
        #endregion

        public static IEnumerable<ManufactureNames> GetManufactures()
        {
            return _context.ManufactureNames.FromSqlRaw(Procedures.GetManufactureNames).AsNoTracking().AsEnumerable();
        }

        public static IEnumerable<ActCreation> GetActCreation()
        {
            return _context.ActCreation.FromSqlRaw(Procedures.GetActCreation).AsNoTracking().AsEnumerable();
        }

        public static IEnumerable<DocumInWork> GetDocumInWork()
        {
            return _context.DocumInWork.FromSqlRaw(Procedures.GetDocumInWork).AsNoTracking().AsEnumerable();
        }

        public static IEnumerable<InWork> GetInWork()
        {
            return _context.InWork.FromSqlRaw(Procedures.GetInWork).AsNoTracking().AsEnumerable();
        }

        public static IEnumerable<NoSpec> GetNoSpec()
        {
            return _context.NoSpec.FromSqlRaw(Procedures.GetNoSpec).AsNoTracking().AsEnumerable();
        }

        public static IEnumerable<OECStorage> GetOECStorage()
        {
            return _context.OECStorage.FromSqlRaw(Procedures.GetOECStorage).AsNoTracking().AsEnumerable();
        }

        public static IEnumerable<SKBCheck> GetSKBCheck()
        {
            return _context.SKBCheck.FromSqlRaw(Procedures.GetSKBCheck).AsNoTracking().AsEnumerable();
        }

        public static IEnumerable<TimedGiving> GetTimedGiving()
        {
            return _context.TimedGiving.FromSqlRaw(Procedures.GetTimedGivingReport).AsNoTracking().AsEnumerable();
        }

        public static IEnumerable<WorkedDocs> GetWorkedDocs()
        {
            return _context.WorkedDocs.FromSqlRaw(Procedures.GetWorkedDocs).AsNoTracking().AsEnumerable();
        }

        public static IEnumerable<VKOnStorage> GetVKOnStorage()
        {
            return _context.VKOnStorage.FromSqlRaw(Procedures.GetVKOnStorage).AsNoTracking().AsEnumerable();
        }

        static TasksOperations()
        {
            Debug.WriteLine("Constructor start:" + DateTime.Now.TimeOfDay);
            _context = ApplicationUnitSingleton.GetApplicationContext();
            Debug.WriteLine("Constructor end:" + DateTime.Now.TimeOfDay);
        }
    }
}

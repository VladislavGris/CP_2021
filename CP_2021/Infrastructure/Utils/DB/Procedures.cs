namespace CP_2021.Infrastructure.Utils.DB
{
    internal class Procedures
    {
        /// <summary>Login, Password, Name, Surname</summary>
        public static string RegisterUser = "exec RegisterUser {0}, {1}, {2}, {3}";
        /// <summary>Id</summary>
        public static string GetUserById = "exec GetUserById {0}";
        /// <summary>Login</summary>
        public static string CheckLogin = "exec {0} = CheckLogin {1}";
        /// <summary>Return value, Login, Password</summary>
        public static string CheckUserCreditionals = "exec {0} = CheckUserCreditionals {1}, {2}";
        /// <summary>Login</summary>
        public static string GetUserByLogin = "exec GetUserByLogin {0}";

        /// <summary>ParentId</summary>
        public static string GetTasksByParent = "exec GetTasksByParent {0}";
        public static string GetSubTasksByProjectName = "exec GetSubTasksByProjectName {0}";
        public static string GetTasksByParentNULL = "exec GetTasksByParent";
        public static string GetTaskById = "exec GetTaskById {0}";
        /// <summary>ParentId, LineOrder</summary>
        public static string InsertEmptyTask = "exec InsertEmptyTask {0}, {1}";
        /// <summary>Id, IncDoc, TaskName, Count, SpecCost, Note, Expanded, Completion, EditingBy</summary>
        public static string UpdateProductionPlan = "exec Update_Production_Plan {0},{1},{2},{3},{4},{5},{6},{7},{8}";
        /// <summary>Id</summary>
        public static string DropTaskById = "exec DropTaskById {0}";
        /// <summary>TaskToUp, TaskToDown</summary>
        public static string UpTask = "exec UpTask {0},{1}";
        /// <summary>TaskToDown, TaskToUp</summary>
        public static string DownTask = "exec DownTask {0},{1}";
        /// <summary>Id, LineOrder, ParentId, ParentLineOrder</summary>
        public static string LevelUpTask = "exec LevelUpTask {0},{1},{2},{3}";
        /// <summary>Id, LineOrder, ParentId, NewParentId</summary>
        public static string LevelDownTask = "exec LevelDownTask {0},{1},{2},{3}";
        /// <summary>TaskId, ParentId, LineOrder</summary>
        public static string PasteTask = "exec PasteTask {0},{1},{2}";
        /// <summary>TaskId, BoldValue</summary>
        public static string SetBold = "exec SetBold {0},{1}";
        /// <summary>TaskId, ItalicValue</summary>
        public static string SetItalic = "exec SetItalic {0},{1}";
        /// <summary>TaskId, UnderlineValue</summary>
        public static string SetUnderline = "exec SetUnderline {0},{1}";
        /// <summary>TaskId, FontSizeValue</summary>
        public static string SetFontSize = "exec SetFontSize {0},{1}";
        /// <summary>TaskId, </summary>
        public static string SetColor = "exec SetColor {0},{1}";

        public static string ExportTaskWithChildren = "exec ExportTaskWithChildren {0}, {1} output";
        /// <summary>Filepath, parentId, lineOrder</summary>
        public static string ImportFromXML = "exec ImportFromXML {0},{1},{2}";
        public static string ExportTasksXML = "exec ExportTasksXML";
        /// <summary>Id</summary>
        public static string ExportTaskById = "exec ExportTaskById {0}";

        /// <summary>TaskId</summary>
        public static string GetComplectationWindowData = "exec GetComplectationData {0}";
        /// <summary>Id Complectation C_Date Comp_Percentage OnStorageDate StateNumber Rack Shelf Note</summary>
        public static string UpdateComplectationWindowData = "exec UpdataComplecationData {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
        /// <summary>TaskId</summary>
        public static string GetConsumeActData = "exec GetConsumeActDate {0}";
        /// <summary>Id ActNumber ActDate ActCreation ByAct Note</summary>
        public static string UpdateConsumeAct = "exec UpdateConsumeAct {0},{1},{2},{3},{4},{5}";
        /// <summary>TaskId</summary>
        public static string GetDocumentData = "exec GetDocumentData {0}";
        /// <summary>TaskId, ManagDoc, VishDate, RealDate</summary>
        public static string UpdateDocumentData = "exec UpdateDocumentData {0},{1},{2},{3}";

        public static string GetGivingData = "exec GetGivingData {0}";
        /// <summary>state, bill, report, return report, receiving date, returnGiving, purchaseGoods, note, taskId</summary>
        public static string UpdateGivingData = "exec UpdateGivingData {0},{1},{2},{3},{4},{5},{6},{7},{8}";

        public static string GetInProductionData = "exec GetInProductionData {0}";
        /// <summary>id, number, givingDate, execName, instExecName, completDate, projDate, note</summary>
        public static string UpdateInProductionData = "exec UpdateInProductionData {0},{1},{2},{3},{4},{5},{6},{7}";

        public static string GetLaborCostsData = "exec GetLaborCostsData {0}";
        /// <summary>id, project, subcont, markingrank, markinghours, assemblyrank,assemblyhours,settingsrank,settingshours,date</summary>
        public static string UpdateLaborCostsData = "exec UpdateLaborCostsData {0},{1},{2},{3},{4},{5},{6},{7},{8},{9}";

        public static string GetManufactureData = "exec GetManufactureData {0}";
        public static string UpdateManufactureData = "exec UpdateManufactureData {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12}";

        public static string GetPaymentData = "exec GetPaymentData {0}";
        public static string UpdatePaymentData = "exec UpdatePaymentData {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}";
        /// <summary>ProductionTaskId</summary>
        public static string GetTimedGivingData = "exec GetTimedGivingData {0}";
        /// <summary>Id, IsTimedGiving, IsSKBCheck, IsOECStorage, SKBNumber, FIO, GivingDate, Note</summary>
        public static string UpdateTimedGivingData = "exec UpdateTimedGiving {0},{1},{2},{3},{4},{5},{6},{7},{8};";

        // Given tasks
        public static string GetAllGivenTasksByUser = "exec GetAllGivenTasksByUser {0}";
        public static string GetAllCompletedTasksByUser = "exec GetAllCompletedTasksByUser {0}";
        public static string GetAllIncompetedTasksByUser = "exec GetAllIncompetedTasksByUser {0}";
        // ToDo tasks
        public static string GetAllToDoTasks = "exec GetAllToDoTasks {0}";
        public static string GetAllToDoTasksNoReport = "exec GetAllToDoTasksNoReport {0}";
        public static string GetAllToDoTasksWithReport = "exec GetAllToDoTasksWithReport {0}";
        public static string GetUserNames = "exec GetUserNames {0}";
        /// <summary>GivenUserId Id, header, completeDate, description</summary>
        public static string CreateNewTask = "exec CreateTaskToUser {0},{1},{2},{3},{4}";
        public static string DropUserTask = "exec DropUserTaskById {0}";
        /// <summary>TaskId ReportDescription</summary>
        public static string AddReportToTask = "exec AddReportToTask {0},{1}";

        public static string GetHeadTasks = "exec GetHeadTasks";
        public static string GetManufactureNames = "exec GetManufactureNames";
        public static string GetSKBNumbers = "exec GetSKBNumbers";

        public static string GetTasksInProgress = "exec GetTasksInProgress";
        public static string GetSpecificationsOnControl = "exec GetSpecificationsOnControl";
        public static string GetSpecificationsInVipisk = "exec GetSpecificationsInVipisk";

        public static string GetCoopWork = "exec GetCoopWork";
        public static string GetActCreation = "exec GetActCreation";
        public static string GetDocumInWork = "exec GetDocumInWork";
        public static string GetInWork = "exec GetInWork";
        public static string GetNoSpec = "exec GetNoSpec";
        public static string GetOECStorage = "exec GetOECStorage";
        public static string GetSKBCheck = "exec GetSKBCheck";
        public static string GetTimedGivingReport = "exec GetTimedGivingReport";
        public static string GetWorkedDocs = "exec GetWorkedDocs";
        public static string GetVKOnStorage = "exec GetVKOnStorage";
        public static string GetPaymentReport = "exec GetPaymentReport";

        public static string SearchProductionPlan = "exec SearchProductionPlan ";
        public static string SearchAct = "exec SearchAct ";
        public static string SearchComplectation = "exec SearchComplectation ";
        public static string SearchGiving = "exec SearchGiving ";
        public static string SearchIn_Production = "exec SearchIn_Production ";
        public static string SearchLaborCosts = "exec SearchLaborCosts ";
        public static string SearchManufacture = "exec SearchManufacture ";
        public static string SearchPayment = "exec SearchPayment ";
        public static string GetAllParents = "exec GetAllParents {0}";
    }
}

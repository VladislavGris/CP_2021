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
        public static string GetTasksByParentNULL = "exec GetTasksByParent with recompile";
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

        public static string ExportTaskWithChildren = "exec ExportTaskWithChildren {0}, {1} output";
        public static string ExportTasksXML = "exec ExportTasksXML";
        /// <summary>Id</summary>
        public static string ExportTaskById = "exec ExportTaskById {0}";
    }
}

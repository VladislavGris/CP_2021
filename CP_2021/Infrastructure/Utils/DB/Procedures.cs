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
        /// <summary>ParentId, LineOrder</summary>
        public static string InsertEmptyTask = "exec InsertEmptyTask {0}, {1}";
        /// <summary>Id, IncDoc, TaskName, Count, SpecCost, Note, Expanded, Completion, EditingBy</summary>
        public static string UpdateProductionPlan = "exec Update_Production_Plan {0},{1},{2},{3},{4},{5},{6},{7},{8}";
        /// <summary>Id</summary>
        public static string DropTaskById = "exec DropTaskById {0}";
    }
}

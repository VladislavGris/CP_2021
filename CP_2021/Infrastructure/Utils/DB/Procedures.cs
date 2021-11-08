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
    }
}

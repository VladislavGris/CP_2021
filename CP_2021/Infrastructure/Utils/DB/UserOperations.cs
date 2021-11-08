using CP_2021.Data;
using CP_2021.Models.DBModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CP_2021.Infrastructure.Utils.DB
{ 
    internal class UserOperations
    {
        private static readonly ApplicationContext _context;

        public static bool UserExists(string login)
        {
            var parmReturn = new SqlParameter
            {
                ParameterName = "ReturnValue",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output,
            };
            var result = _context.Database.ExecuteSqlRaw(Procedures.CheckLogin, parmReturn, login);
            if ((int)parmReturn.Value > 0) 
                return true;
            return false;
        }

        public static UserDB RegisterUser(string login, string password, string name, string surname)
        {
            string passwordHash = PasswordHashing.CreateHash(password);
            UserDB user = _context.Users.FromSqlRaw(Procedures.RegisterUser, login, passwordHash, name, surname).AsNoTracking().ToList().FirstOrDefault();
            return user;
        }

        public static UserDB CheckUserCreditionals(string login, string password)
        {
            UserDB user = GetEnteredUser(login);
            if (user != null && PasswordHashing.ValidatePassword(password, user.Password))
                return user;
            return null;
        }

        public static UserDB GetEnteredUser(string login)
        {
            return _context.Users.FromSqlRaw(Procedures.GetUserByLogin, login).AsNoTracking().ToList().FirstOrDefault();
        }

        static UserOperations()
        {
            _context = new ApplicationContext();
        }
    }
}

using CP_2021.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Singletons
{
    class UserDataSingleton
    {
        private static UserDataSingleton _instance;
        public UserDB user;

        public static UserDataSingleton GetInstance()
        {
            if(_instance == null)
            {
                _instance = new UserDataSingleton();
            }
            return _instance;
        }

        private UserDataSingleton()
        {
            user = new UserDB();
        }

        public void SetUser(UserDB user)
        {
            if (_instance != null)
            {
                _instance.user = user;
            } 
        }
    }
}

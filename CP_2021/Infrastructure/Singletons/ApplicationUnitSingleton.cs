using CP_2021.Data;
using CP_2021.Infrastructure.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Singletons
{
    class ApplicationUnitSingleton
    {
        public ApplicationUnit dbUnit;
        private static ApplicationUnitSingleton _instance;

        public static ApplicationUnitSingleton GetInstance()
        {
            if(_instance == null)
            {
                _instance = new ApplicationUnitSingleton();
            }
            return _instance;
        }

        private ApplicationUnitSingleton()
        {
            dbUnit = new ApplicationUnit(new ApplicationContext());
        }
    }
}

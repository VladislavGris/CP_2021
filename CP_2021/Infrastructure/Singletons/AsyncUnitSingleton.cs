using CP_2021.Data;
using CP_2021.Infrastructure.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Singletons
{
    class AsyncUnitSingleton
    {
        public AsyncDBUnit dbUnit;
        private static AsyncUnitSingleton _instance;

        public static AsyncUnitSingleton GetInstance()
        {
            if(_instance == null)
            {
                _instance = new AsyncUnitSingleton();
            }
            return _instance;
        }

        private AsyncUnitSingleton()
        {
            dbUnit = new AsyncDBUnit(new ApplicationContext());
        }
    }
}

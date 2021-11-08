using CP_2021.Data;
using CP_2021.Infrastructure.Units;

namespace CP_2021.Infrastructure.Singletons
{
    class ApplicationUnitSingleton
    {
        public ApplicationUnit dbUnit;
        public ApplicationContext context;
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
            dbUnit = new ApplicationUnit(context = new ApplicationContext());
        }

        public static void RecreateUnit()
        {
            _instance = new ApplicationUnitSingleton();
        }
    }
}

using CP_2021.Data;
using CP_2021.Infrastructure.Units;

namespace CP_2021.Infrastructure.Singletons
{
    class ApplicationUnitSingleton
    {
        public ApplicationUnit dbUnit;
        private static ApplicationContext context;
        private static ApplicationUnitSingleton _instance;

        public static ApplicationUnitSingleton GetInstance()
        {
            if(_instance == null)
            {
                _instance = new ApplicationUnitSingleton();
            }
            return _instance;
        }

        public static ApplicationContext GetApplicationContext()
        {
            if(context == null)
            {
                context = new ApplicationContext();
            }
            return context;
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

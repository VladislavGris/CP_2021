using CP_2021.Data;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Units;
using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Threading
{
    class UpdatingThread
    {
        private ApplicationUnit _unit;
        private string _message;

        public UpdatingThread(string message)
        {
            _message = message;
            _unit = ApplicationUnitSingleton.GetInstance().dbUnit;
        }

        public void StartThread()
        {
            Thread updatingThread = new Thread(new ThreadStart(this.Updating));
            updatingThread.Start();
        }

        private void Updating()
        {
            while (true)
            {
                Thread.Sleep(60_000);
                ApplicationUnit updatedUnit = new ApplicationUnit(new ApplicationContext());
                if (!updatedUnit.Equals(_unit))
                {
                    Debug.WriteLine("New data in database");
                    _message = "В базе появилсиь новые данные";
                }
            }
        }
    }
}

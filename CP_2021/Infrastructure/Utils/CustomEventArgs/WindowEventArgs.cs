using System;

namespace CP_2021.Infrastructure.Utils.CustomEventArgs
{
    enum DataWindow
    {
        Complectation,
        ConsumeAct,
        Document,
        Giving,
        InProduction,
        LaborCosts,
        Manufacture,
        Payment,
        TimedGiving
    }

    internal class WindowEventArgs : EventArgs
    {
        public DataWindow dataWindow;
        public Guid taskId;
    }
}

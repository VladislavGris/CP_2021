﻿using System;

namespace CP_2021.Infrastructure.Utils.CustomEventArgs
{
    enum DataWindow
    {
        Complectation
    }

    internal class WindowEventArgs : EventArgs
    {
        public DataWindow dataWindow;
        public Guid taskId;
    }
}

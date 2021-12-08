using CP_2021.Models.ProcedureResuts.Plan;
using System;

namespace CP_2021.Infrastructure.Utils.CustomEventArgs
{
    internal class UserTaskEventArgs : EventArgs
    {
        public TaskReport Task { get; set; }
    }
}

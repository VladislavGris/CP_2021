using System;

namespace CP_2021.Infrastructure.Utils.CustomEventArgs
{
    class TaskIdEventArgs:EventArgs
    {
        public Guid Id { get; set; }
    }
}

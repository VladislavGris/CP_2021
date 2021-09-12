using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CP_2021.Infrastructure.Commands.Base
{
    interface IAsyncCommand : ICommand
    {
        IEnumerable<Task> RunningTasks { get; }
        bool CanExecute();
        Task ExecuteAsync();
    }
}

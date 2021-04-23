using CP_2021.Infrastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CP_2021.Infrastructure.Commands
{
    class MinimizeWindowCommand : Command
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => ((Window)parameter).WindowState = WindowState.Minimized;
    }
}

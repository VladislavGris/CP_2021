using System;
using System.Collections.Generic;
using CP_2021.ViewModels.Base;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CP_2021.Infrastructure.Commands;
using CP_2021.ViewModels;
using CP_2021.Views.Windows;

namespace CP_2021.ViewModels
{
    class RegistrationViewModel : ViewModelBase
    {
        #region Команды

        #region OpenLoginWindowCommand

        public ICommand OpenLoginWindowCommand { get; }

        private bool CanOpenLoginWindowCommandExecute(object p) => true;

        private void OnOpenLoginWindowCommandExecuted(object p)
        {
            LoginScreen login = new LoginScreen();
            ((RegistrationScreen)p).Close();
            login.Show();
        }

        #endregion

        #endregion

        public RegistrationViewModel()
        {
            OpenLoginWindowCommand = new LambdaCommand(OnOpenLoginWindowCommandExecuted, CanOpenLoginWindowCommandExecute);
        }
    }
}

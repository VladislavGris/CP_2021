using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CP_2021.Infrastructure.Commands;
using CP_2021.ViewModels;
using CP_2021.Views.Windows;

namespace CP_2021.ViewModels
{
    class LoginViewModel
    {
        #region Команды

        #region OpenRegistrationWindowCommand

        public ICommand OpenRegistrationWindowCommand { get; }

        private bool CanOpenRegistrationWindowCommandExecute(object p) => true;

        private void OnOpenRegistrationWindowCommandExecuted(object p)
        {
            RegistrationScreen registration = new RegistrationScreen();
            ((LoginScreen)p).Close();
            registration.Show();
        }

        #endregion

        #endregion

        public LoginViewModel()
        {
            OpenRegistrationWindowCommand = new LambdaCommand(OnOpenRegistrationWindowCommandExecuted, CanOpenRegistrationWindowCommandExecute);
        }
    }
}

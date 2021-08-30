using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CP_2021.Infrastructure.Commands;
using CP_2021.ViewModels;
using CP_2021.Views.Windows;
using CP_2021.ViewModels.Base;
using CP_2021.Infrastructure.Units;
using CP_2021.Data;
using System.Windows.Data;
using Microsoft.Data.SqlClient;
using System.Windows;
using CP_2021.Infrastructure;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Models.DBModels;

namespace CP_2021.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        #region Свойства

        #region Login

        private String _login;

        public String Login
        {
            get => _login;
            set => Set(ref _login, value);
        }

        #endregion

        #region Password

        private String _password;

        public String Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        #endregion

        #endregion

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

        #region SubmitCommand

        public ICommand SubmitCommand { get; }

        private bool CanSubmitCommandExecute(object p) => !String.IsNullOrEmpty(Login) || !String.IsNullOrEmpty(Password);

        private void OnSubmitCommandExecuted(object p)
        {
            if(LoginOrPasswordHasIncorrectLength())
            {
                MessageBox.Show("Логин и пароль должны содержать от 4 до 15 символов");
                return;
            }
            try
            {
                UserDB user;
                using (ApplicationContext context = new ApplicationContext())
                {
                    user = context.Users.Where(u => u.Login == Login).FirstOrDefault();
                }
                if (user != null && PasswordHashing.ValidatePassword(Password, user.Password))
                {
                    UserDataSingleton.GetInstance().SetUser(user);
                    ProductionPlan plan = new ProductionPlan();
                    ((LoginScreen)p).Close();
                    plan.Show();
                }
                else
                {
                    MessageBox.Show("Логин или пароль введены неверно");
                }
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.Message);
                Application.Current.Shutdown();
            }
        }

        #endregion

        #endregion

        #region Methods

        private bool LoginOrPasswordHasIncorrectLength()
        {
            return (Login.Length < 4 || Login.Length > 15) || (Password.Length < 4 || Password.Length > 15);
        }

        #endregion

        public LoginViewModel()
        {
            OpenRegistrationWindowCommand = new LambdaCommand(OnOpenRegistrationWindowCommandExecuted, CanOpenRegistrationWindowCommandExecute);
            SubmitCommand = new LambdaCommand(OnSubmitCommandExecuted, CanSubmitCommandExecute);
        }
    }
}

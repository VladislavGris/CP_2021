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

namespace CP_2021.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        #region Поля

        private readonly string FIND_USER = "SELECT * FROM Users WHERE Login=@login AND Password=@password";
        private ApplicationUnit _unit;

        #endregion

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

        private bool CanSubmitCommandExecute(object p) => true;

        private void OnSubmitCommandExecuted(object p)
        {
            SqlParameter loginParam = new SqlParameter("@login", Login);
            SqlParameter passwordParam = new SqlParameter("@password", Password);
            var user = _unit.DBUsers.GetWithRawSql(FIND_USER, loginParam, passwordParam);
            if (user.ToList().Count != 0)
            {
                ProductionPlan plan = new ProductionPlan();
                ((LoginScreen)p).Close();
                plan.Show();
            }
            else
            {
                MessageBox.Show("Логин или пароль введены неверно");
            }
        }

        #endregion

        #endregion

        public LoginViewModel()
        {
            OpenRegistrationWindowCommand = new LambdaCommand(OnOpenRegistrationWindowCommandExecuted, CanOpenRegistrationWindowCommandExecute);
            SubmitCommand = new LambdaCommand(OnSubmitCommandExecuted, CanSubmitCommandExecute);
            _unit = new ApplicationUnit(new ApplicationContext());
        }
    }
}

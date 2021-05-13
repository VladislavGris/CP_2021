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

namespace CP_2021.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        #region Поля

        private readonly string FIND_USER = "SELECT * FROM Users WHERE Login=@login";
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

        private bool CanSubmitCommandExecute(object p) => Login != null && Login != "" && Password != null && Password != "";

        private void OnSubmitCommandExecuted(object p)
        {
            if((Login.Length < 4 || Login.Length > 15)||(Password.Length < 4 || Password.Length > 15))
            {
                MessageBox.Show("Логин и пароль должны содержать от 4 до 15 символов");
                return;
            }
            SqlParameter loginParam = new SqlParameter("@login", Login);
            var user = _unit.DBUsers.GetWithRawSql(FIND_USER, loginParam);
            if (user.ToList().Count != 0 && PasswordHashing.ValidatePassword(Password, user.ToList().ElementAt(0).Password))
            {
                    ProductionPlan plan = new ProductionPlan();
                    plan.DataContext = new ProductionPlanViewModel(user.ToList().ElementAt(0), _unit);
                    //((ProductionPlanViewModel)plan.DataContext).User = user.ToList().ElementAt(0);
                    //((ProductionPlanViewModel)plan.DataContext).Unit = _unit;
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

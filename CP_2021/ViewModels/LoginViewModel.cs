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
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using CP_2021.Infrastructure.Utils.DB;

namespace CP_2021.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        #region Свойства

        private ApplicationContext _context;

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

        #region LoadingVisibility

        private Visibility _loadingVisibility = Visibility.Hidden;

        public Visibility LoadingVisibility
        {
            get => _loadingVisibility;
            set => Set(ref _loadingVisibility, value);
        }

        #endregion

        #region LoadingSpin

        private bool _loadingSpin = false;

        public bool LoadingSpin
        {
            get => _loadingSpin;
            set => Set(ref _loadingSpin, value);
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
            try
            {
                UserDB user = UserOperations.CheckUserCreditionals(Login, Password);
                if (user == null)
                {
                    MessageBox.Show("Логин или пароль введены неверно");
                    return;
                }
                UserDataSingleton.GetInstance().SetUser(UserOperations.GetEnteredUser(Login));
                ProductionPlan planWindow = new ProductionPlan();
                ((LoginScreen)p).Close();
                planWindow.Show();

            }
            catch(SqlException ex){
                MessageBox.Show(ex.Message);
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

            _context = new ApplicationContext();
        }
    }
}

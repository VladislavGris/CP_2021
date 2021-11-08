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
using CP_2021.Infrastructure.Units;
using System.Windows;
using CP_2021.Data;
using Microsoft.Data.SqlClient;
using CP_2021.Models.DBModels;
using CP_2021.Infrastructure;
using CP_2021.Infrastructure.Singletons;
using Microsoft.EntityFrameworkCore;
using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.Base;

namespace CP_2021.ViewModels
{
    class RegistrationViewModel : ViewModelBase
    {
        #region Свойства
        ApplicationContext _context;
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

        #region Name

        private String _name;

        public String Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        #endregion

        #region Surname

        private String _surname;

        public String Surname
        {
            get => _surname;
            set => Set(ref _surname, value);
        }

        #endregion

        #endregion

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

        #region SubmitCommand

        public ICommand SubmitCommand { get; }

        private bool CanSubmitCommandExecute(object p) =>   !string.IsNullOrEmpty(Login) &&
                                                            !string.IsNullOrEmpty(Password) &&
                                                            !string.IsNullOrEmpty(Name) && 
                                                            !string.IsNullOrEmpty(Surname);

        private void OnSubmitCommandExecuted(object currentWindow)
        {
            CheckLoginPasswordLength();
            CheckNameSurnameLength();
            try
            {

                if (UserOperations.UserExists(Login))
                {
                    MessageBox.Show("Такой логин уже существует");
                    return;
                }
                UserDataSingleton.GetInstance().SetUser(UserOperations.RegisterUser(Login, Password, Name, Surname));
                ProductionPlan planWindow = new ProductionPlan();
                ((RegistrationScreen)currentWindow).Close();
                planWindow.Show();
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Shutdown();
            }
        }

        #endregion

        #endregion

        #region Methods

        private void CheckLoginPasswordLength()
        {
            if ((Login.Length < 4 || Login.Length > 15) || (Password.Length < 4 || Password.Length > 15))
            {
                MessageBox.Show("Логин и пароль должны содержать от 4 до 15 символов");
                return;
            }
        }

        private void CheckNameSurnameLength()
        {
            if (Name.Length > 50 || Surname.Length > 50)
            {
                MessageBox.Show("Максимальная длина имени или фамилии - 50 символов");
                return;
            }
        }

        #endregion

        public RegistrationViewModel()
        {
            #region Команды
            OpenLoginWindowCommand = new LambdaCommand(OnOpenLoginWindowCommandExecuted, CanOpenLoginWindowCommandExecute);
            SubmitCommand = new LambdaCommand(OnSubmitCommandExecuted, CanSubmitCommandExecute);
            #endregion
            _context = new ApplicationContext();
        }
    }
}

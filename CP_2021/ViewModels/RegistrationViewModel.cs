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

namespace CP_2021.ViewModels
{
    class RegistrationViewModel : ViewModelBase
    {
        #region Поля

        private ApplicationUnit _unit;
        private readonly string FIND_USER = "SELECT * FROM Users WHERE Login=@login";

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

        private bool CanSubmitCommandExecute(object p) => true;

        private void OnSubmitCommandExecuted(object p)
        {
            if ((Login.Length < 4 || Login.Length > 15) || (Password.Length < 4 || Password.Length > 15))
            {
                MessageBox.Show("Логин и пароль должны содержать от 4 до 15 символов");
                return;
            }
            if(Name.Length == 0 || Surname.Length == 0)
            {
                MessageBox.Show("Поля Имя или Фамилия не должны быть пустыми");
                return;
            }
            if(Name.Length > 50 || Surname.Length > 50)
            {
                MessageBox.Show("Максимальная длина имени или фамилии - 50 символов");
                return;
            }
            SqlParameter loginParam = new SqlParameter("@login", Login);
            var user = _unit.DBUsers.GetWithRawSql(FIND_USER, loginParam);
            if(user.ToList().Count > 0)
            {
                MessageBox.Show("Такой логин уже существует");
                return;
            }
            UserDB newUser = new UserDB(Login, Password, Name, Surname);
            _unit.DBUsers.Insert(newUser);
            _unit.Commit();
            ProductionPlan plan = new ProductionPlan();
            plan.DataContext = new ProductionPlanViewModel();
            ((ProductionPlanViewModel)plan.DataContext).User = newUser;
            ((ProductionPlanViewModel)plan.DataContext).Unit = _unit;
            ((RegistrationScreen)p).Close();
            plan.Show();
        }

        #endregion

        #endregion

        public RegistrationViewModel()
        {
            OpenLoginWindowCommand = new LambdaCommand(OnOpenLoginWindowCommandExecuted, CanOpenLoginWindowCommandExecute);
            SubmitCommand = new LambdaCommand(OnSubmitCommandExecuted, CanSubmitCommandExecute);
            _unit = new ApplicationUnit(new ApplicationContext());
        }
    }
}

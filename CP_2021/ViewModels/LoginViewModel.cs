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

namespace CP_2021.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        #region Свойства

        Thread _backgroundThread;

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
            if (!_backgroundThread.IsAlive)
            {
                // Отображение вращающего элемента, изображающего загрузку
                LoadingVisibility = Visibility.Visible;
                // Созадние потока для входа в систему
                _backgroundThread = new Thread(new ThreadStart(() => TryLogin((Window)p)));
                _backgroundThread.Start();
            }
            
            //await Task.Run(() => TryLogin((Window)p));
        }

        private void TryLogin(Window currentWindow)
        {
            if (!LoginOrPasswordHasIncorrectLength())
            {
                try
                {
                    UserDB user = ApplicationUnitSingleton.GetInstance().dbUnit.DBUsers.Get().Where(u => u.Login == Login).FirstOrDefault();

                    if (user != null && PasswordHashing.ValidatePassword(Password, user.Password))
                    {
                        UserDataSingleton.GetInstance().SetUser(user);
                        // Вызов через диспетчер приложения т.к. ЭУ изменяется и создается только из главного потока
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var plan = new ProductionPlan();
                            plan.Show();
                            currentWindow.Close();
                        });
                    }
                    else
                    {
                        MessageBox.Show("Логин или пароль введены неверно");
                    }
                }
                catch (SqlException e)
                {
                    // Ошибка подключения к базе данных
                    MessageBox.Show(e.Message);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Application.Current.Shutdown();
                    });
                    
                }
            }
            else
            {
                MessageBox.Show("Логин и пароль должны содержать от 4 до 15 символов");
            }
            // Остановка отображения загрузки
            Application.Current.Dispatcher.Invoke(() =>
            {
                LoadingVisibility = Visibility.Hidden;
            });
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

            _backgroundThread = new Thread(new ThreadStart(()=>Console.Write("")));
            _backgroundThread.IsBackground = true;
        }
    }
}

using CP_2021.Infrastructure.Commands;
using CP_2021.ViewModels.Base;
using CP_2021.ViewModels;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using CP_2021.Views.UserControls;
using CP_2021.Models.DBModels;
using CP_2021.Infrastructure.Units;
using System.Diagnostics;
using CP_2021.Views.Windows;
using System;
using CP_2021.Infrastructure.Singletons;

namespace CP_2021.ViewModels
{
    internal class ProductionPlanViewModel : ViewModelBase
    {
        #region Свойства

        #region ContentContainerContent

        private UserControl _ContentContainerContent;

        public UserControl ContentContainerContent
        {
            get => _ContentContainerContent;
            set => Set(ref _ContentContainerContent, value);
        }

        #endregion

        #region ButtonCloseMenuVisibility

        private Visibility _ButtonCloseMenuVisibility = Visibility.Collapsed;

        public Visibility ButtonCloseMenuVisibility
        {
            get => _ButtonCloseMenuVisibility;
            set
            {
                _ButtonCloseMenuVisibility = value;
                OnPropertyChanged("ButtonCloseMenuVisibility");
            }
        }

        #endregion

        #region ButtonOpenMenuVisibility

        private Visibility _ButtonOpenMenuVisibility = Visibility.Visible;

        public Visibility ButtonOpenMenuVisibility
        {
            get => _ButtonOpenMenuVisibility;
            set
            {
                _ButtonOpenMenuVisibility = value;
                OnPropertyChanged("ButtonOpenMenuVisibility");
            }
        }

        #endregion

        #region Title
        private string _Title = "Company Planner";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        #region User
        private UserDB _user;

        public UserDB User
        {
            get => _user;
            set => Set(ref _user, value);
        }
        #endregion

        #region Unit

        private ApplicationUnit _unit;

        public ApplicationUnit Unit
        {
            get => _unit;
            set => Set(ref _unit, value);
        }

        #endregion


        #endregion

        #region Команды

        #region ButtonCloseMenuCommand

        public ICommand ButtonCloseMenuCommand { get; }

        private bool CanButtonCloseMenuCommandExecute(object p) => true;

        private void OnButtonCloseMenuCommandExecuted(object p)
        {
            ButtonOpenMenuVisibility = Visibility.Visible;
            ButtonCloseMenuVisibility = Visibility.Collapsed;
        }

        #endregion

        #region ButtonOpenMenuCommand

        public ICommand ButtonOpenMenuCommand { get; }

        private bool CanButtonOpenMenuCommandExecute(object p) => true;

        private void OnButtonOpenMenuCommandExecuted(object p)
        {
            ButtonOpenMenuVisibility = Visibility.Collapsed;
            ButtonCloseMenuVisibility = Visibility.Visible;
        }

        #endregion

        #region ChangeContentCommand

        public ICommand ChangeContentCommand { get; }

        private bool CanChangeContentCommandExecute(object p) => true;

        private void OnChangeContentCommandExecuted(object p)
        {
            var parameters = (object[])p;
            var list = (ListViewItem)parameters[0];
            var window = (Window)parameters[1];
            switch (list.Name)
            {
                case "ItemProductionPlan":
                    InitProductionPlanControl();
                    break;
                case "ItemReports":
                    InitReports();
                    break;
                case "ItemTasks":
                    InitMyTasks();
                    break;
                case "ItemSettings":
                    ContentContainerContent = new UserControlSettings();
                    break;
                case "ItemGivenTasks":
                    InitGivenTasksControl();
                    break;
                case "ItemAdministrativePannel":
                    InitAdministrativePannelControl();
                    break;
                case "ItemLogOut":
                    MessageBoxResult result = MessageBox.Show("Вы действительно хотите выйти", "Выход", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            LoginScreen login = new LoginScreen();
                            UserDataSingleton.GetInstance().ClearUser();
                            window.Close();
                            login.Show();
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region WindowLoadedCommand

        public ICommand WindowLoadedCommand { get; }

        private bool CanWindowLoadedCommandExecute(object p) => true;

        private void OnWindowLoadedCommandExecuted(object p)
        {
            InitProductionPlanControl();
        }

        #endregion

        #endregion

        #region Методы

        private void InitProductionPlanControl()
        {
            UserControlProductionPlan control = new UserControlProductionPlan();
            control.DataContext = new ProductionPlanControlViewModel();
            ContentContainerContent = control;
        }

        private void InitAdministrativePannelControl()
        {
            UserControlAdministrativePannel pannel = new UserControlAdministrativePannel();
            pannel.DataContext = new AdministrationPannelViewModel(Unit, User);
            ContentContainerContent = pannel;
        }

        private void InitGivenTasksControl()
        {
            UserControlGivenTasks tasks = new UserControlGivenTasks();
            tasks.DataContext = new GivenTasksViewModel(Unit, User);
            ContentContainerContent = tasks;
        }

        private void InitMyTasks()
        {
            UserControlTasks tasks = new UserControlTasks();
            tasks.DataContext = new MyTasksViewModel();
            ContentContainerContent = tasks;
        }

        private void InitReports()
        {
            UserControlReports reports = new UserControlReports();
            reports.DataContext = new ReportsViewModel();
            ContentContainerContent = reports;
        }

        #endregion

        public ProductionPlanViewModel()
        {
            #region Команды

            ButtonCloseMenuCommand = new LambdaCommand(OnButtonCloseMenuCommandExecuted, CanButtonCloseMenuCommandExecute);
            ButtonOpenMenuCommand = new LambdaCommand(OnButtonOpenMenuCommandExecuted, CanButtonOpenMenuCommandExecute);
            ChangeContentCommand = new LambdaCommand(OnChangeContentCommandExecuted, CanChangeContentCommandExecute);
            WindowLoadedCommand = new LambdaCommand(OnWindowLoadedCommandExecuted, CanWindowLoadedCommandExecute);
            #endregion

            User = UserDataSingleton.GetInstance().user;
            Unit = ApplicationUnitSingleton.GetInstance().dbUnit;
        }
    }
}

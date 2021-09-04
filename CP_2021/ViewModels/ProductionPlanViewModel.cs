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
        #region User
        private UserDB _user;
        // Через Position пользователя осуществляется вилимость кнопок боковой панели окна
        public UserDB User
        {
            get => _user;
            set => Set(ref _user, value);
        }
        #endregion
        #region ProductionPlanControl
        private UserControlProductionPlan _planControl;

        public UserControlProductionPlan PlanControl
        {
            get => _planControl;
            set => Set(ref _planControl, value);
        }
        #endregion
        #region Controls
        #region AdminControl
        private UserControlAdministrativePannel _adminControl;

        public UserControlAdministrativePannel AdminControl
        {
            get => _adminControl;
            set => Set(ref _adminControl, value);
        }
        #endregion
        #region GivenTaskControl
        private UserControlGivenTasks _givenTaskControl;

        public UserControlGivenTasks GivenTaskControl
        {
            get => _givenTaskControl;
            set => Set(ref _givenTaskControl, value);
        }
        #endregion
        #region MyTasksControl
        private UserControlTasks _myTasksControl;

        public UserControlTasks MyTasksControl
        {
            get => _myTasksControl;
            set => Set(ref _myTasksControl, value);
        }
        #endregion
        #region ReportsControl
        private UserControlReports _reportsControl;

        public UserControlReports ReportsControl
        {
            get => _reportsControl;
            set => Set(ref _reportsControl, value);
        }
        #endregion
        #region HelpControl
        private UserControlHelp _helpControl;

        public UserControlHelp HelpControl
        {
            get => _helpControl;
            set => Set(ref _helpControl, value);
        }
        #endregion
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
                    SetPlanCurrent();
                    break;
                case "ItemReports":
                    SetReportsCurrent();
                    break;
                case "ItemTasks":
                    SetMyTasksCurrent();
                    break;
                case "ItemHelp":
                    SetHelpCurrent();
                    break;
                case "ItemGivenTasks":
                    SetGevenTasksCurrent();
                    break;
                case "ItemAdministrativePannel":
                    SetAdminPannelCurrent();
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
            SetPlanCurrent();
        }

        #endregion

        #endregion

        #region Методы

        private void SetPlanCurrent()
        {
            if (PlanControl is null)
            {
                PlanControl = new UserControlProductionPlan();
            }
            ContentContainerContent = PlanControl;
        }
        private void SetAdminPannelCurrent()
        {
            if(AdminControl is null)
            {
                AdminControl = new UserControlAdministrativePannel();
            }
            
            ContentContainerContent = AdminControl;
        }
        private void SetGevenTasksCurrent()
        {
            if(GivenTaskControl is null)
            {
                GivenTaskControl = new UserControlGivenTasks();
            }
            
            ContentContainerContent = GivenTaskControl;
        }
        private void SetMyTasksCurrent()
        {
            if(MyTasksControl is null)
            {
                MyTasksControl = new UserControlTasks();
            }
            
            ContentContainerContent = MyTasksControl;
        }
        private void SetReportsCurrent()
        {
            if(ReportsControl is null)
            {
                ReportsControl = new UserControlReports();
            }
            
            ContentContainerContent = ReportsControl;
        }
        private void SetHelpCurrent()
        {
            if(HelpControl is null)
            {
                HelpControl = new UserControlHelp();
            }
            ContentContainerContent = HelpControl;
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
        }
    }
}

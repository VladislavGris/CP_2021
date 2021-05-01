using CP_2021.Infrastructure.Commands;
using CP_2021.ViewModels.Base;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using CP_2021.Views.UserControls;
using CP_2021.Models.DBModels;
using CP_2021.Infrastructure.Units;
using System.Diagnostics;

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
        private string _Title = "Production Plan";

        /// <summary>Заголовок окна</summary>
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
            switch (((ListViewItem)p).Name)
            {
                case "ItemProductionPlan":
                    InitProductionPlanControl();
                    break;
                case "ItemReports":
                    ContentContainerContent = new UserControlReports();
                    break;
                case "ItemTasks":
                    ContentContainerContent = new UserControlTasks();
                    break;
                case "ItemSettings":
                    ContentContainerContent = new UserControlSettings();
                    break;
                case "ItemGivenTasks":
                    ContentContainerContent = new UserControlGivenTasks();
                    break;
                case "ItemAdministrativePannel":
                    ContentContainerContent = new UserControlAdministrativePannel();
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
            control.DataContext = new ProductionPlanControlViewModel(Unit, User);
            ContentContainerContent = control;
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
        }
    }
}

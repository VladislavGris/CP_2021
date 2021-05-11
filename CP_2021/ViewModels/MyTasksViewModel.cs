using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Units;
using CP_2021.Models.DBModels;
using CP_2021.ViewModels.Base;
using CP_2021.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CP_2021.ViewModels
{
    class MyTasksViewModel : ViewModelBase
    {

        #region Свойства

        #region Unit

        private ApplicationUnit _unit;

        public ApplicationUnit Unit
        {
            get => _unit;
            set => Set(ref _unit, value);
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

        #region Tasks

        private ObservableCollection<TaskDB> _tasks;

        public ObservableCollection<TaskDB> Tasks
        {
            get => _tasks;
            set => Set(ref _tasks, value);
        }

        #endregion

        #endregion

        #region Команды

        #region OpenSendReportWindowCommand

        public ICommand OpenSendReportWindowCommand { get; }

        private bool CanOpenSendReportWindowCommandExecute(object p) => true;

        private void OnOpenSendReportWindowCommandExecuted(object p)
        {
            SendReportWindow window = new SendReportWindow();
            window.DataContext = new SendReportViewModel(Unit, User, (ReportDB)p);
            window.Show();
        }

        #endregion

        #region OpenShowReportWindowCommand

        public ICommand OpenShowReportWindowCommand { get; }

        private bool CanOpenShowReportWindowCommandExecute(object p) => true;

        private void OnOpenShowReportWindowCommandExecuted(object p)
        {
            ViewReportWindow window = new ViewReportWindow();
            window.DataContext = new ViewReportViewModel(Unit, User, (ReportDB)p);
            window.Show();
        }

        #endregion

        #region SendReportCommand

        public ICommand SendReportCommand { get; }

        private bool CanSendReportCommandExecute(object p) => true;

        private void OnSendReportCommandExecuted(object p)
        {
            
        }

        #endregion

        #region ShowReportCommand

        public ICommand ShowReportCommand { get; }

        private bool CanShowReportCommandExecute(object p) => true;

        private void OnShowReportCommandExecuted(object p)
        {

        }

        #endregion

        #endregion

        public MyTasksViewModel() { }

        public MyTasksViewModel(ApplicationUnit unit, UserDB user)
        {
            #region Команды

            OpenSendReportWindowCommand = new LambdaCommand(OnOpenSendReportWindowCommandExecuted, CanOpenSendReportWindowCommandExecute);
            OpenShowReportWindowCommand = new LambdaCommand(OnOpenShowReportWindowCommandExecuted, CanOpenShowReportWindowCommandExecute);
            SendReportCommand = new LambdaCommand(OnSendReportCommandExecuted, CanSendReportCommandExecute);
            ShowReportCommand = new LambdaCommand(OnShowReportCommandExecuted, CanShowReportCommandExecute);


            #endregion

            Unit = unit;
            User = user;
            Tasks = new ObservableCollection<TaskDB>(unit.UserTasks.Get().Where(u => u.To.Equals(User)));
        }
    }
}

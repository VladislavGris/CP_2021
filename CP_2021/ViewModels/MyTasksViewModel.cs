using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Units;
using CP_2021.Infrastructure.Utils.CustomEventArgs;
using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.DBModels;
using CP_2021.Models.ProcedureResuts.Plan;
using CP_2021.ViewModels.Base;
using CP_2021.Views.Windows;
using Notifications.Wpf;
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

        #region Tasks

        private ObservableCollection<TaskReport> _tasks;

        public ObservableCollection<TaskReport> Tasks
        {
            get => _tasks;
            set => Set(ref _tasks, value);
        }

        #endregion

        #region FilterSelection

        private int _filterSelection = 0;

        public int FilterSelection
        {
            get => _filterSelection;
            set => Set(ref _filterSelection, value);
        }

        #endregion

        #endregion

        public void GetAddedReport(object sender, UserTaskEventArgs e)
        {
            foreach(var task in Tasks)
            {
                if(task.Id == e.Task.Id)
                {
                    task.ReportState = true;
                    task.ReportDescription = e.Task.ReportDescription;
                }
            }

        }

        #region Команды

        #region OpenSendReportWindowCommand

        public ICommand OpenSendReportWindowCommand { get; }

        private bool CanOpenSendReportWindowCommandExecute(object p) => true;

        private void OnOpenSendReportWindowCommandExecuted(object p)
        {
            SendReportWindow window = new SendReportWindow();
            window.DataContext = new SendReportViewModel((TaskReport)p);
            ((SendReportViewModel)window.DataContext).OnTaskAdded += GetAddedReport;
            window.Show();
        }

        #endregion

        #region OpenShowReportWindowCommand

        public ICommand OpenShowReportWindowCommand { get; }

        private bool CanOpenShowReportWindowCommandExecute(object p) => true;

        private void OnOpenShowReportWindowCommandExecuted(object p)
        {
            ViewReportWindow window = new ViewReportWindow();
            window.DataContext = new ViewReportViewModel((TaskReport)p);
            window.Show();
        }

        #endregion

        #region RefreshCommand

        public ICommand RefreshCommand { get; }

        private bool CanRefreshCommandExecute(object p) => true;

        private void OnRefreshCommandExecuted(object p)
        {
            Update();
        }

        #endregion

        #region FilterSelectionChanged

        public ICommand FilterSelectionChanged { get; }

        private bool CanFilterSelectionChangedExecute(object p) => true;

        private void OnFilterSelectionChangedExecuted(object p)
        {
            Update();
        }

        #endregion

        #endregion

        private void Update()
        {
            switch (FilterSelection)
            {
                case 0:
                    Tasks = new ObservableCollection<TaskReport>(UserTaskOperations.GetAllToDoTasks(UserDataSingleton.GetInstance().user.Id));
                    break;
                case 1:
                    Tasks = new ObservableCollection<TaskReport>(UserTaskOperations.GetAllToDoTasksWithReport(UserDataSingleton.GetInstance().user.Id));
                    break;
                case 2:
                    Tasks = new ObservableCollection<TaskReport>(UserTaskOperations.GetAllToDoTasksNoReport(UserDataSingleton.GetInstance().user.Id));
                    break;
                default:
                    break;
            }
        }

        public MyTasksViewModel() 
        {
            #region Команды

            OpenSendReportWindowCommand = new LambdaCommand(OnOpenSendReportWindowCommandExecuted, CanOpenSendReportWindowCommandExecute);
            OpenShowReportWindowCommand = new LambdaCommand(OnOpenShowReportWindowCommandExecuted, CanOpenShowReportWindowCommandExecute);
            RefreshCommand = new LambdaCommand(OnRefreshCommandExecuted, CanRefreshCommandExecute);
            FilterSelectionChanged = new LambdaCommand(OnFilterSelectionChangedExecuted, CanFilterSelectionChangedExecute);

            #endregion
            Tasks = new ObservableCollection<TaskReport>(UserTaskOperations.GetAllToDoTasks(UserDataSingleton.GetInstance().user.Id));
            //var notificationManager = new NotificationManager();
            //notificationManager.Show(new NotificationContent
            //{
            //    Title = "Задачи",
            //    Message = "У вас появились новые задачи",
            //    Type = NotificationType.Information
            //});
        }
    }
}

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
    class GivenTasksViewModel : ViewModelBase
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

        #region SelectedTask

        private TaskDB _selectedTask;

        public TaskDB SelectedTask
        {
            get => _selectedTask;
            set => Set(ref _selectedTask, value);
        }

        #endregion

        #endregion

        #region Команды

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

        #region OpenAddTaskWindow

        public ICommand OpenAddTaskWindow { get; }

        private bool CanOpenAddTaskWindowExecute(object p) => true;

        private void OnOpenAddTaskWindowExecuted(object p)
        {
               
            AddTaskWindow window = new AddTaskWindow();
            window.DataContext = new AddTaskWindowViewModel(Unit, User, (GivenTasksViewModel)p);
            window.Show();
        }

        #endregion

        #region RemoveTaskCommand

        public ICommand RemoveTaskCommand { get; }

        private bool CanRemoveTaskCommandExecute(object p) => true;

        private void OnRemoveTaskCommandExecuted(object p)
        {
            Unit.UserTasks.Delete((TaskDB)p);
            Unit.Commit();
            Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.Report.To.Equals(User)));
        }

        #endregion

        #region UpdateCollectionCommand

        public ICommand UpdateCollectionCommand { get; }

        private bool CanUpdateCollectionCommandExecute(object p) => true;

        private void OnUpdateCollectionCommandExecuted(object p)
        {
            Unit.Refresh();
            Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.Report.To.Equals(User)));
        }

        #endregion


        #endregion

        public GivenTasksViewModel() { }
        public GivenTasksViewModel(ApplicationUnit unit, UserDB user)
        {
            #region Команды
            OpenShowReportWindowCommand = new LambdaCommand(OnOpenShowReportWindowCommandExecuted, CanOpenShowReportWindowCommandExecute);
            OpenAddTaskWindow = new LambdaCommand(OnOpenAddTaskWindowExecuted, CanOpenAddTaskWindowExecute);
            RemoveTaskCommand = new LambdaCommand(OnRemoveTaskCommandExecuted, CanRemoveTaskCommandExecute);
            UpdateCollectionCommand = new LambdaCommand(OnUpdateCollectionCommandExecuted, CanUpdateCollectionCommandExecute);

            #endregion

            Unit = unit;
            User = user;
            Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.Report.To.Equals(User)));
            
        }
    }
}

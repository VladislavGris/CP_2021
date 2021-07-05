using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
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
using System.Windows;
using System.Windows.Input;

namespace CP_2021.ViewModels
{
    class GivenTasksViewModel : ViewModelBase
    {

        #region Свойства

        private ApplicationUnit Unit;

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

        #region FilterSelection

        private int _filterSelection = 0;

        public int FilterSelection
        {
            get => _filterSelection;
            set => Set(ref _filterSelection, value);
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
            window.DataContext = new ViewReportViewModel((ReportDB)p);
            window.Show();
        }

        #endregion

        #region OpenAddTaskWindow

        public ICommand OpenAddTaskWindow { get; }

        private bool CanOpenAddTaskWindowExecute(object p) => true;

        private void OnOpenAddTaskWindowExecuted(object p)
        {
               
            AddTaskWindow window = new AddTaskWindow();
            window.DataContext = new AddTaskWindowViewModel((GivenTasksViewModel)p);
            window.Show();
        }

        #endregion

        #region RemoveTaskCommand

        public ICommand RemoveTaskCommand { get; }

        private bool CanRemoveTaskCommandExecute(object p) => true;

        private void OnRemoveTaskCommandExecuted(object p)
        {
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить задачу {((TaskDB)p).Header}?", "Удаление", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Unit.UserTasks.Delete((TaskDB)p);
                    Unit.Commit();
                    Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.Report.To.Equals(User)));
                    break;
            }
        }

        #endregion

        #region UpdateCollectionCommand

        public ICommand UpdateCollectionCommand { get; }

        private bool CanUpdateCollectionCommandExecute(object p) => true;

        private void OnUpdateCollectionCommandExecuted(object p)
        {
            Unit.Refresh();
            switch (FilterSelection)
            {
                case 0:
                    Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.Report.To.Equals(User)));
                    break;
                case 1:
                    Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.Report.To.Equals(User) && t.Report.State == true));
                    break;
                case 2:
                    Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.Report.To.Equals(User) && t.Report.State == false));
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region FilterSelectionChanged

        public ICommand FilterSelectionChanged { get; }

        private bool CanFilterSelectionChangedExecute(object p) => true;

        private void OnFilterSelectionChangedExecuted(object p)
        {
            Unit.Refresh();
            switch (FilterSelection)
            {
                case 0:
                    Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.Report.To.Equals(User)));
                    break;
                case 1:
                    Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.Report.To.Equals(User) && t.Report.State==true));
                    break;
                case 2:
                    Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.Report.To.Equals(User) && t.Report.State == false));
                    break;
                default:
                    break;
            }
        }

        #endregion

        #endregion

        public GivenTasksViewModel() 
        {
            #region Команды
            OpenShowReportWindowCommand = new LambdaCommand(OnOpenShowReportWindowCommandExecuted, CanOpenShowReportWindowCommandExecute);
            OpenAddTaskWindow = new LambdaCommand(OnOpenAddTaskWindowExecuted, CanOpenAddTaskWindowExecute);
            RemoveTaskCommand = new LambdaCommand(OnRemoveTaskCommandExecuted, CanRemoveTaskCommandExecute);
            UpdateCollectionCommand = new LambdaCommand(OnUpdateCollectionCommandExecuted, CanUpdateCollectionCommandExecute);
            FilterSelectionChanged = new LambdaCommand(OnFilterSelectionChangedExecuted, CanFilterSelectionChangedExecute);

            #endregion

            Unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            User = UserDataSingleton.GetInstance().user;
            Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.Report.To.Equals(User)));
        }
    }
}

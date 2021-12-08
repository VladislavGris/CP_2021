using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Utils.CustomEventArgs;
using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.ProcedureResuts.Plan;
using CP_2021.ViewModels.Base;
using CP_2021.Views.Windows;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CP_2021.ViewModels
{
    class GivenTasksViewModel : ViewModelBase
    {

        #region Свойства

        #region GivenTasks

        private ObservableCollection<TaskReport> _givenTasks;

        public ObservableCollection<TaskReport> GivenTasks
        {
            get => _givenTasks;
            set => Set(ref _givenTasks, value);
        }

        #endregion

        #region SelectedTask

        private TaskReport _selectedTask;

        public TaskReport SelectedTask
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

        // Event handler for task addition
        public void GetAddedTask(object sender, UserTaskEventArgs e)
        {
            // 0 -all
            // 1 - completed
            // 2 - incomplete
            if(FilterSelection == 2 || FilterSelection == 0)
                GivenTasks.Add(e.Task);
        }

        #region Команды

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

        #region OpenAddTaskWindow

        public ICommand OpenAddTaskWindow { get; }

        private bool CanOpenAddTaskWindowExecute(object p) => true;

        private void OnOpenAddTaskWindowExecuted(object p)
        {

            AddTaskWindow window = new AddTaskWindow();
            window.DataContext = new AddTaskWindowViewModel();
            ((AddTaskWindowViewModel)window.DataContext).OnTaskAdded += GetAddedTask;
            window.Show();
        }

        #endregion

        #region RemoveTaskCommand

        public ICommand RemoveTaskCommand { get; }

        private bool CanRemoveTaskCommandExecute(object p) => true;

        private void OnRemoveTaskCommandExecuted(object p)
        {
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить задачу {((TaskReport)p).Header}?", "Удаление", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        UserTaskOperations.DeleteTaskById(((TaskReport)p).Id);

                        GivenTasks.Remove((TaskReport)p);
                    }catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
            }
        }

        #endregion

        #region UpdateCollectionCommand

        public ICommand UpdateCollectionCommand { get; }

        private bool CanUpdateCollectionCommandExecute(object p) => true;

        private void OnUpdateCollectionCommandExecuted(object p)
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

        private void Update()
        {
            switch (FilterSelection)
            {
                case 0:
                    GivenTasks = new ObservableCollection<TaskReport>(UserTaskOperations.GetAllGivenTasksByUser(UserDataSingleton.GetInstance().user.Id));
                    break;
                case 1:
                    GivenTasks = new ObservableCollection<TaskReport>(UserTaskOperations.GetAllCompletedTasksByUser(UserDataSingleton.GetInstance().user.Id));
                    break;
                case 2:
                    GivenTasks = new ObservableCollection<TaskReport>(UserTaskOperations.GetAllIncompetedTasksByUser(UserDataSingleton.GetInstance().user.Id));
                    break;
                default:
                    break;
            }
        }

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
            GivenTasks = new ObservableCollection<TaskReport>(UserTaskOperations.GetAllGivenTasksByUser(UserDataSingleton.GetInstance().user.Id));
        }
    }
}

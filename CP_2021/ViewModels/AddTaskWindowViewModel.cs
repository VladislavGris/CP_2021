using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Units;
using CP_2021.Infrastructure.Utils.CustomEventArgs;
using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.DBModels;
using CP_2021.Models.ProcedureResuts;
using CP_2021.Models.ProcedureResuts.Plan;
using CP_2021.ViewModels.Base;
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
    class AddTaskWindowViewModel : ViewModelBase
    {
        #region Свойства

        #region SelectedUser

        private UserNames _selectedUser;

        public UserNames SelectedUser
        {
            get => _selectedUser;
            set => Set(ref _selectedUser, value);
        }

        #endregion

        #region Task

        private TaskReport _task = new TaskReport() { CompleteDate = DateTime.Now};

        public TaskReport Task
        {
            get => _task;
            set => Set(ref _task, value);
        }

        #endregion

        //#region Users

        //private ObservableCollection<UserDB> _users;

        //public ObservableCollection<UserDB> Users
        //{
        //    get => _users;
        //    set => Set(ref _users, value);
        //}

        //#endregion

        #region UserNames

        private ObservableCollection<UserNames> _userNames;

        public ObservableCollection<UserNames> UserNames
        {
            get => _userNames;
            set => Set(ref _userNames, value);
        }

        #endregion

        #endregion

        #region Команды

        #region CancelCommand

        public ICommand CancelCommand { get; }

        private bool CanCancelCommandExecute(object p) => true;

        private void OnCancelCommandExecuted(object p)
        {
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите отменить создание задачи?", "Отмена", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    ((Window)p).Close();
                    break;
            }
        }

        #endregion

        #region AddTaskCommand

        public ICommand AddTaskCommand { get; }

        private bool CanAddTaskCommandExecute(object p) => true;

        private void OnAddTaskCommandExecuted(object p)
        {
            if (SelectedUser == null || Task.Description == null || Task.Header == null)
            {
                MessageBox.Show("Все поля должны быть заполнены");
                return;
            }
            Task.ToId = SelectedUser.Id;
            Task.Name = SelectedUser.Name;
            Task.Surname = SelectedUser.Surname;
            UserTaskOperations.CreateNewTask(Task,UserDataSingleton.GetInstance().user.Id);

            UserTaskEventArgs args = new UserTaskEventArgs();
            args.Task = Task;
            OnSendTaskIdToMainWindow(args);

            ((Window)p).Close();
        }

        #endregion

        #endregion

        #region Events

        public event EventHandler<UserTaskEventArgs> OnTaskAdded;
        // Отправка task на окно тасков
        protected void OnSendTaskIdToMainWindow(UserTaskEventArgs e)
        {
            EventHandler<UserTaskEventArgs> handler = OnTaskAdded;
            handler?.Invoke(this, e);
        }

        #endregion

        public AddTaskWindowViewModel() {
            AddTaskCommand = new LambdaCommand(OnAddTaskCommandExecuted, CanAddTaskCommandExecute);
            CancelCommand = new LambdaCommand(OnCancelCommandExecuted, CanCancelCommandExecute);

            UserNames = new ObservableCollection<UserNames>(UserTaskOperations.GetUserNames(UserDataSingleton.GetInstance().user.Id));
        }

        public AddTaskWindowViewModel(GivenTasksViewModel vm)
        {
            
        }
    }
}

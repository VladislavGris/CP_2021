using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Units;
using CP_2021.Models.DBModels;
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

        #region SelectedUser

        private UserDB _selectedUser;

        public UserDB SelectedUser
        {
            get => _selectedUser;
            set => Set(ref _selectedUser, value);
        }

        #endregion

        #region Task

        private TaskDB _task;

        public TaskDB Task
        {
            get => _task;
            set => Set(ref _task, value);
        }

        #endregion

        #region Users

        private ObservableCollection<UserDB> _users;

        public ObservableCollection<UserDB> Users
        {
            get => _users;
            set => Set(ref _users, value);
        }

        #endregion


        #endregion

        #region Команды

        #region CancelCommand

        public ICommand CancelCommand { get; }

        private bool CanCancelCommandExecute(object p) => true;

        private void OnCancelCommandExecuted(object p)
        {
            ((Window)p).Close();
        }

        #endregion

        #region AddTaskCommand

        public ICommand AddTaskCommand { get; }

        private bool CanAddTaskCommandExecute(object p) => true;

        private void OnAddTaskCommandExecuted(object p)
        {
            Task.To = SelectedUser;
            ReportDB report = new ReportDB(User, Task);
            Task.Report = report;
            Unit.UserTasks.Insert(Task);
            Unit.Commit();
            ((Window)p).Close();
        }

        #endregion

        #endregion

        public AddTaskWindowViewModel() { }

        public AddTaskWindowViewModel(ApplicationUnit unit, UserDB user)
        {
            #region Команды

            CancelCommand = new LambdaCommand(OnCancelCommandExecuted, CanCancelCommandExecute);
            AddTaskCommand = new LambdaCommand(OnAddTaskCommandExecuted, CanAddTaskCommandExecute);

            #endregion

            Unit = unit;
            User = user;
            Users = new ObservableCollection<UserDB>(unit.DBUsers.Get().Where(u => !u.Equals(user) && u.Position != 2));
            Task = new TaskDB();
        }
    }
}

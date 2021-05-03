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

        #endregion

        #region Команды

        #region OpenAddTaskWindow

        public ICommand OpenAddTaskWindow { get; }

        private bool CanOpenAddTaskWindowExecute(object p) => true;

        private void OnOpenAddTaskWindowExecuted(object p)
        {
            AddTaskWindow window = new AddTaskWindow();
            window.DataContext = new AddTaskWindowViewModel(Unit, User);
            window.Show();
        }

        #endregion

        #endregion

        public GivenTasksViewModel() { }
        public GivenTasksViewModel(ApplicationUnit unit, UserDB user)
        {
            #region Команды

            OpenAddTaskWindow = new LambdaCommand(OnOpenAddTaskWindowExecuted, CanOpenAddTaskWindowExecute);

            #endregion

            Unit = unit;
            User = user;
            Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.Report.To.Equals(User)));
        }
    }
}

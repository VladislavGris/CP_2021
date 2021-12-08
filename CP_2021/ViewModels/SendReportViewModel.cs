using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Utils.CustomEventArgs;
using CP_2021.Models.ProcedureResuts.Plan;
using CP_2021.ViewModels.Base;
using CP_2021.Infrastructure.Utils.DB;
using System;
using System.Windows;
using System.Windows.Input;

namespace CP_2021.ViewModels
{
    class SendReportViewModel : ViewModelBase
    {
        #region Task

        private TaskReport _task;

        public TaskReport Task
        {
            get => _task;
            set => Set(ref _task, value);
        }

        #endregion

        #region SubmitCommand

        public ICommand SubmitCommand { get; }

        private bool CanSubmitCommandExecute(object p) => !String.IsNullOrEmpty(Task?.ReportDescription);

        private void OnSubmitCommandExecuted(object p)
        {
            Task.ReportState = true;

            UserTaskOperations.AddReportToTask(Task.Id, Task.ReportDescription);

            UserTaskEventArgs args = new UserTaskEventArgs();
            args.Task = Task;
            OnSendTaskIdToMainWindow(args);

            ((Window)p).Close();
        }

        #endregion

        #region OnTaskAddedEvent

        public event EventHandler<UserTaskEventArgs> OnTaskAdded;
        // Отправка task на окно тасков
        protected void OnSendTaskIdToMainWindow(UserTaskEventArgs e)
        {
            EventHandler<UserTaskEventArgs> handler = OnTaskAdded;
            handler?.Invoke(this, e);
        }

        #endregion

        public SendReportViewModel(TaskReport task) {
            #region Команды
            SubmitCommand = new LambdaCommand(OnSubmitCommandExecuted, CanSubmitCommandExecute);
            #endregion

            Task = task;
        }
    }
}

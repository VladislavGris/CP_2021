using CP_2021.Data;
using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Utils.CustomEventArgs;
using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.Classes;
using CP_2021.Models.DataWindowEntities;
using CP_2021.ViewModels.Base;
using CP_2021.Views.Windows.DataWindows;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace CP_2021.ViewModels.DataWindowViewModels
{
    internal class ConsumeActWindowVM : ViewModelBase
    {
        private ConsumeActWindowEntity _entity;
        public ConsumeActWindowEntity Entity
        {
            get => _entity;
            set => Set(ref _entity, value);
        }
        private ApplicationContext _context;
        private ProductionTask _task;

        #region SaveCommand

        public ICommand SaveCommand { get; }

        private bool CanSaveCommandExecute(object p) => true;

        private void OnSaveCommandExecuted(object p)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите сохранить изменения?", "Сохранить", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    TasksOperations.UpdateConsumeActData(_entity.Id, _entity.ActNumber, _entity.ActDate.Value, _entity.ActCreation, _entity.ByAct, _entity.Note);
                    _task.data.Complectation = _entity.ActNumber;
                    ((Window)p).Close();
                    break;
                case MessageBoxResult.No:
                    ((Window)p).Close();
                    break;
                case MessageBoxResult.Cancel:
                case MessageBoxResult.None:
                    break;
            }
        }

        #endregion
        public event EventHandler<WindowEventArgs> SendIdToPlan;

        protected virtual void OnSendTaskIdToReportVM(WindowEventArgs e)
        {
            EventHandler<WindowEventArgs> handler = SendIdToPlan;
            handler?.Invoke(this, e);
        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            WindowEventArgs args = new WindowEventArgs();
            args.dataWindow = DataWindow.Complectation;
            args.taskId = _task.data.Id;
            OnSendTaskIdToReportVM(args);
        }

        public ConsumeActWindowVM() { }

        public ConsumeActWindowVM(ConsumeActWindowEntity entity, ProductionTask task, ConsumeActWindow window)
        {
            SaveCommand = new LambdaCommand(OnSaveCommandExecuted, CanSaveCommandExecute);

            Entity = entity;
            _task = task;
            _context = ApplicationUnitSingleton.GetApplicationContext();
            window.Closing += DataWindow_Closing;
        }
    }
}

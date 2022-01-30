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
        private bool _isActEmptyOrNull = false;

        #region SaveCommand

        public ICommand SaveCommand { get; }

        private bool CanSaveCommandExecute(object p) => true;

        private void OnSaveCommandExecuted(object p)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите сохранить изменения?", "Сохранить", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        if(!String.IsNullOrEmpty(Entity.ActNumber) && _isActEmptyOrNull)
                        {
                            Entity.ActCreation = false;
                            Entity.ByAct = true;
                        }
                        TasksOperations.UpdateConsumeActData(Entity.Id, 
                            Entity.ActNumber, 
                            Entity.ActDate.HasValue ? Entity.ActDate.Value : null, 
                            Entity.ActCreation, 
                            Entity.ByAct, 
                            Entity.Note);
                        _task.data.ActNumber = Entity.ActNumber;
                        _task.data.ActDate = Entity.ActDate;
                        _task.data.ActCreation = Entity.ActCreation;
                        _task.data.ByAct = Entity.ByAct;
                    }catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
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
            args.dataWindow = DataWindow.ConsumeAct;
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
            if (String.IsNullOrEmpty(_task.data.ActNumber))
            {
                _isActEmptyOrNull = true;
            }
            else
            {
                _isActEmptyOrNull = false;
            }
        }
    }
}

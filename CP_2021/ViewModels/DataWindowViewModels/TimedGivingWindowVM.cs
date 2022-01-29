using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Utils.CustomEventArgs;
using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.Classes;
using CP_2021.Models.DataWindowEntities;
using CP_2021.ViewModels.Base;
using CP_2021.Views.Windows.DataWindows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CP_2021.ViewModels.DataWindowViewModels
{
    internal class TimedGivingWindowVM : ViewModelBase
    {
        private TimedGivingWindowEntity _entity;
        public TimedGivingWindowEntity Entity
        {
            get => _entity;
            set => Set(ref _entity, value);
        }

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
                    try
                    {
                        TasksOperations.UpdateTimedGivingData(Entity.Id,
                            Entity.IsTimedGiving,
                            Entity.IsSKBCheck,
                            Entity.IsOECStorage,
                            Entity.SKBNumber,
                            Entity.FIO,
                            Entity.GivingDate.HasValue ? Entity.GivingDate.Value : null,
                            Entity.Note);
                        _task.data.IsTimedGiving = Entity.IsTimedGiving;
                        _task.data.IsOECStorage = Entity.IsOECStorage;
                        _task.data.IsSKBCheck = Entity.IsSKBCheck;
                    }
                    catch (Exception ex)
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
            args.dataWindow = DataWindow.Complectation;
            args.taskId = _task.data.Id;
            OnSendTaskIdToReportVM(args);
        }

        public TimedGivingWindowVM() { }

        public TimedGivingWindowVM(TimedGivingWindowEntity entity, ProductionTask task, TimedGivingWindow window)
        {
            SaveCommand = new LambdaCommand(OnSaveCommandExecuted, CanSaveCommandExecute);

            Entity = entity;
            _task = task;
            window.Closing += DataWindow_Closing;
        }
    }
}

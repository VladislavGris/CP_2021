﻿using CP_2021.Data;
using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
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
    internal class InProductionWindowVM : ViewModelBase
    {
        private InProductionWindowEntity _entity;
        public InProductionWindowEntity Entity
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
                    try
                    {
                        TasksOperations.UpdateInProductionData(Entity.Id, Entity.Number, Entity.GivingDate.HasValue ? Entity.GivingDate : null,
                            Entity.Executor,
                            Entity.InstallExecutor,
                            Entity.CompletionDate.HasValue ? Entity.CompletionDate : null,
                            Entity.ProjectedDate.HasValue ? Entity.ProjectedDate:null,
                            Entity.Note);
                        _task.data.GivingDate = Entity.GivingDate;
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
            args.dataWindow = DataWindow.InProduction;
            args.taskId = _task.data.Id;
            OnSendTaskIdToReportVM(args);
        }

        public InProductionWindowVM() { }

        public InProductionWindowVM(InProductionWindowEntity entity, ProductionTask task, InProductionWindow window)
        {
            SaveCommand = new LambdaCommand(OnSaveCommandExecuted, CanSaveCommandExecute);

            Entity = entity;
            _task = task;
            _context = ApplicationUnitSingleton.GetApplicationContext();
            window.Closing += DataWindow_Closing;
        }
    }
}

using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Utils.CustomEventArgs;
using CP_2021.Models.ViewEntities;
using CP_2021.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CP_2021.ViewModels.Reports
{
    internal abstract class BaseReport<T> : ViewModelBase where T:BaseViewEntity
    {
        #region Свойства
        #region FullContent

        private ObservableCollection<T> _fullContent;

        public ObservableCollection<T> FullContent
        {
            get => _fullContent;
            set => Set(ref _fullContent, value);
        }

        #endregion

        #region Content

        private ObservableCollection<T> _content;

        public ObservableCollection<T> Content
        {
            get => _content;
            set => Set(ref _content, value);
        }

        #endregion

        #region HeadTasks

        private ObservableCollection<string> _headTasks;

        public ObservableCollection<string> HeadTasks
        {
            get => _headTasks;
            set => Set(ref _headTasks, value);
        }

        #endregion

        #region Manufactirers

        private ObservableCollection<string> _manufactirers;

        public ObservableCollection<string> Manufactirers
        {
            get => _manufactirers;
            set => Set(ref _manufactirers, value);
        }

        #endregion

        #region SelectedHead

        private string _selectedHead;

        public string SelectedHead
        {
            get => _selectedHead;
            set => Set(ref _selectedHead, value);
        }

        #endregion

        #region SelectedManufacture

        private string _selectedManufacture;

        public string SelectedManufacture
        {
            get => _selectedManufacture;
            set => Set(ref _selectedManufacture, value);
        }

        #endregion

        #region SelectedRow

        private T _selectedRow;

        public T SelectedRow
        {
            get => _selectedRow;
            set => Set(ref _selectedRow, value);
        }

        #endregion
        #endregion

        #region Events

        public event EventHandler<TaskIdEventArgs> SendTaskIdToReportVM;

        protected virtual void OnSendTaskIdToReportVM(TaskIdEventArgs e)
        {
            EventHandler<TaskIdEventArgs> handler = SendTaskIdToReportVM;
            handler?.Invoke(this, e);
        }

        #endregion

        #region Команды

        #region ProjectChangedCommand

        public ICommand ProjectChangedCommand { get; }

        private bool CanProjectChangedCommandExecute(object p) => true;

        private void OnProjectChangedCommandExecuted(object p)
        {
            Content = new ObservableCollection<T>(FullContent.Where(t => t.Project == SelectedHead));
            if(!String.IsNullOrEmpty(SelectedManufacture))
            {
                Content = new ObservableCollection<T>(Content.Where(t => t.Manufacturer == SelectedManufacture));
            }
        }

        #endregion

        #region ManufactureChangedCommand

        public ICommand ManufactureChangedCommand { get; }

        private bool CanManufactureChangedCommandExecute(object p) => true;

        private void OnManufactureChangedCommandExecuted(object p)
        {
            Content = new ObservableCollection<T>(FullContent.Where(t => t.Manufacturer == SelectedManufacture));
            if(!String.IsNullOrEmpty(SelectedHead))
            {
                Content = new ObservableCollection<T>(Content.Where(t => t.Project == SelectedHead));
            }
        }

        #endregion

        #region GotoTaskCommand

        public ICommand GotoTaskCommand { get; }

        private bool CanGotoTaskCommandExecute(object p) => true;

        private void OnGotoTaskCommandExecuted(object p)
        {
            TaskIdEventArgs args = new TaskIdEventArgs() { Id = SelectedRow.Id };
            OnSendTaskIdToReportVM(args);
        }

        #endregion

        #region GotoParentTaskCommand

        public ICommand GotoParentTaskCommand { get; }

        private bool CanGotoParentTaskCommandExecute(object p) => SelectedRow?.ParentId != null;

        private void OnGotoParentTaskCommandExecuted(object p)
        {
            TaskIdEventArgs args = new TaskIdEventArgs() { Id = SelectedRow.ParentId.Value };
            OnSendTaskIdToReportVM(args);
        }

        #endregion

        #endregion

        public BaseReport()
        {

            ProjectChangedCommand = new LambdaCommand(OnProjectChangedCommandExecuted, CanProjectChangedCommandExecute);
            GotoTaskCommand = new LambdaCommand(OnGotoTaskCommandExecuted, CanGotoTaskCommandExecute);
            GotoParentTaskCommand = new LambdaCommand(OnGotoParentTaskCommandExecuted, CanGotoParentTaskCommandExecute);
            ManufactureChangedCommand = new LambdaCommand(OnManufactureChangedCommandExecuted, CanManufactureChangedCommandExecute);
        }
    }
}

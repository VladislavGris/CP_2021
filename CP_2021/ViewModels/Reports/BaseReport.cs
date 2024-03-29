﻿using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Utils.CustomEventArgs;
using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.ViewEntities;
using CP_2021.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        #region SubTasksOfHead

        private ObservableCollection<string> _subTasksOfHead;

        public ObservableCollection<string> SubTasksOfHead
        {
            get => _subTasksOfHead;
            set => Set(ref _subTasksOfHead, value);
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

        #region SelectedSubTaskOfHead

        private string _selectedSubTaskOfHead;

        public string SelectedSubTaskOfHead
        {
            get => _selectedSubTaskOfHead;
            set => Set(ref _selectedSubTaskOfHead, value);
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

        #region GenerateReportCommand

        public ICommand GenerateReportCommand { get; }

        private bool CanGenerateReportCommandExecute(object p) => true;

        protected virtual void OnGenerateReportCommandExecuted(object p)
        {
            var heads = TasksOperations.GetHeadTasks();
            var manufacturers = TasksOperations.GetManufactures();

            HeadTasks = new ObservableCollection<string>();
            SubTasksOfHead = new ObservableCollection<string>();
            Manufactirers = new ObservableCollection<string>();
            foreach (var head in heads)
            {
                HeadTasks.Add(head.Task);
            }
            foreach (var manufacture in manufacturers)
            {
                Manufactirers.Add(manufacture.Name);
            }
        }

        #endregion

        #region GeneratePDFCommand

        public ICommand GeneratePDFCommand { get; }

        private bool CanGeneratePDFCommandExecute(object p) => Content != null && Content.Count > 0;

        protected virtual void OnGeneratePDFCommandExecuted(object p)
        {
            
        }

        #endregion

        #region ProjectChangedCommand

        public ICommand ProjectChangedCommand { get; }

        private bool CanProjectChangedCommandExecute(object p) => true;

        protected virtual void OnProjectChangedCommandExecuted(object p)
        {
            // Загрузка подпроектов выбранного головного проекта
            var tasks = TasksOperations.GetSubTasksByProjectName(SelectedHead);
            SubTasksOfHead = new ObservableCollection<string>();
            SelectedSubTaskOfHead = null;
            foreach (var task in tasks)
            {
                SubTasksOfHead.Add(task.Name);
            }
            ApplyFilters();
        }

        #endregion

        #region SubTaskOfHeadChangedCommand 

        public ICommand SubTaskOfHeadChangedCommand { get; }

        private bool CanSubTaskOfHeadChangedCommandExecute(object p) => !String.IsNullOrEmpty(SelectedHead);

        protected virtual void OnSubTaskOfHeadChangedCommandExecuted(object p)
        {
            ApplyFilters();
        }

        #endregion

        #region ManufactureChangedCommand

        public ICommand ManufactureChangedCommand { get; }

        private bool CanManufactureChangedCommandExecute(object p) => true;

        protected virtual void OnManufactureChangedCommandExecuted(object p)
        {
            ApplyFilters();
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


        #region DropFiltersCommand

        public ICommand DropFiltersCommand { get; }

        private bool CanDropFiltersCommandExecute(object p) => FullContent != null;

        protected virtual void OnDropFiltersCommandExecuted(object p)
        {
            SelectedHead = null;
            SelectedManufacture = null;
            Content = FullContent;
        }

        #endregion

        #endregion

        private void ApplyFilters()
        {
            Content = FullContent;
            if(!String.IsNullOrEmpty(SelectedHead))
            {
                Content = new ObservableCollection<T>(Content.Where(t => t.Project == SelectedHead));
            }
            if (!String.IsNullOrEmpty(SelectedManufacture))
            {
                Content = new ObservableCollection<T>(Content.Where(t=>t.Manufacturer == SelectedManufacture));
            }
            if(!String.IsNullOrEmpty(SelectedSubTaskOfHead))
            {
                Content = new ObservableCollection<T>(Content.Where(t => t.SubProject == SelectedSubTaskOfHead));
            }
        }

        public BaseReport()
        {

            ProjectChangedCommand = new LambdaCommand(OnProjectChangedCommandExecuted, CanProjectChangedCommandExecute);
            SubTaskOfHeadChangedCommand = new LambdaCommand(OnSubTaskOfHeadChangedCommandExecuted, CanSubTaskOfHeadChangedCommandExecute);
            GotoTaskCommand = new LambdaCommand(OnGotoTaskCommandExecuted, CanGotoTaskCommandExecute);
            GotoParentTaskCommand = new LambdaCommand(OnGotoParentTaskCommandExecuted, CanGotoParentTaskCommandExecute);
            ManufactureChangedCommand = new LambdaCommand(OnManufactureChangedCommandExecuted, CanManufactureChangedCommandExecute);
            DropFiltersCommand = new LambdaCommand(OnDropFiltersCommandExecuted, CanDropFiltersCommandExecute);
            GenerateReportCommand = new LambdaCommand(OnGenerateReportCommandExecuted, CanGenerateReportCommandExecute);
            GeneratePDFCommand = new LambdaCommand(OnGeneratePDFCommandExecuted, CanGeneratePDFCommandExecute);
        }
    }
}

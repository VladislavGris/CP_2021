using Common.Wpf.Data;
using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Units;
using CP_2021.Models.Classes;
using CP_2021.Models.DBModels;
using CP_2021.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CP_2021.ViewModels
{
    class ReportsViewModel : ViewModelBase
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

        #region NoSpecification

        private ObservableCollection<ProductionTaskDB> _noSpecification;

        public ObservableCollection<ProductionTaskDB> NoSpecification
        {
            get => _noSpecification;
            set => Set(ref _noSpecification, value);
        }

        #endregion

        #region GivingReports

        private ObservableCollection<ProductionTaskDB> _givingReports;

        public ObservableCollection<ProductionTaskDB> GivingReports
        {
            get => _givingReports;
            set => Set(ref _givingReports, value);
        }

        #endregion

        #region GivingAvailability

        private TreeGridModel _givingAvailability = new TreeGridModel();

        public TreeGridModel GivingAvailability
        {
            get => _givingAvailability;
            set => Set(ref _givingAvailability, value);
        }

        #endregion

        #region InProduction

        private TreeGridModel _inProduction = new TreeGridModel();

        public TreeGridModel InProduction
        {
            get => _inProduction;
            set => Set(ref _inProduction, value);
        }

        #endregion

        #region ExecutorInProduction

        private TreeGridModel _executorInProduction = new TreeGridModel();

        public TreeGridModel ExecutorInProduction
        {
            get => _executorInProduction;
            set => Set(ref _executorInProduction, value);
        }

        #endregion

        #region ExecutorCompleted

        private TreeGridModel _executorCompleted = new TreeGridModel();

        public TreeGridModel ExecutorCompleted
        {
            get => _executorCompleted;
            set => Set(ref _executorCompleted, value);
        }

        #endregion

        #region DateFrom

        private DateTime _dateFrom = DateTime.Today;

        public DateTime DateFrom
        {
            get => _dateFrom;
            set => Set(ref _dateFrom, value);
        }

        #endregion

        #region DateTo

        private DateTime _dateTo = DateTime.Today;

        public DateTime DateTo
        {
            get => _dateTo;
            set => Set(ref _dateTo, value);
        }

        #endregion

        #region ExecutorName

        private string _executorName;

        public string ExecutorName
        {
            get => _executorName;
            set => Set(ref _executorName, value);
        }

        #endregion

        #endregion

        #region Команды

        #region GenerateNoSpecificationReportCommand

        public ICommand GenerateNoSpecificationReportCommand { get; }

        private bool CanGenerateNoSpecificationReportCommandExecute(object p) => true;

        private void OnGenerateNoSpecificationReportCommandExecuted(object p)
        {
            NoSpecification = new ObservableCollection<ProductionTaskDB>(Unit.Tasks.Get().Where(t => (t.Manufacture.SpecNum == null || t.Manufacture.SpecNum == "") && t.Manufacture.Name != null && t.Manufacture.Name != "" && t.Manufacture.LetterNum != null && t.Manufacture.LetterNum != ""));
        }

        #endregion

        #region GenerateGivingReportsCommand

        public ICommand GenerateGivingReportsCommand { get; }

        private bool CanGenerateGivingReportsCommandExecute(object p) => true;

        private void OnGenerateGivingReportsCommandExecuted(object p)
        {
            GivingReports = new ObservableCollection<ProductionTaskDB>(Unit.Tasks.Get().Where(t => (t.Giving.Report == null || t.Giving.Report == "") && t.Giving.Bill != null && t.Giving.Bill != "" && t.Manufacture.SpecNum != null && t.Manufacture.SpecNum != ""));
        }

        #endregion

        #region GenerateGivingAvailabilityCommand

        public ICommand GenerateGivingAvailabilityCommand { get; }

        private bool CanGenerateGivingAvailabilityCommandExecute(object p) => true;

        private void OnGenerateGivingAvailabilityCommandExecuted(object p)
        {
            GivingAvailability = new TreeGridModel();
            foreach (var task in Unit.Tasks.Get().ToList())
            {
                if (task.Giving.State.HasValue && task.Giving.State.Value == true && task.Giving.ReceivingDate.HasValue && task.Giving.ReceivingDate > DateFrom && task.Giving.ReceivingDate < DateTo)
                {
                    ProductionTask selectedTask = new ProductionTask(task);
                    ProductionTask parent = new ProductionTask();
                    if (task.MyParent != null)
                    {
                        parent = new ProductionTask(task.MyParent.Parent);
                        if (!GivingAvailability.Contains(parent))
                        {
                            GivingAvailability.Add(parent);
                        }
                        parent.Children.Add(selectedTask);
                        parent.HasChildren = true;
                    }
                    else
                    {
                        GivingAvailability.Add(selectedTask);
                    }

                }
            }
        }

        #endregion

        #region GenerateInProductionCommand

        public ICommand GenerateInProductionCommand { get; }

        private bool CanGenerateInProductionCommandExecute(object p) => true;

        private void OnGenerateInProductionCommandExecuted(object p)
        {
            InProduction = new TreeGridModel();
            foreach (var task in Unit.Tasks.Get().ToList())
            {

                if (task.Completion == 4)
                {
                    ProductionTask selectedTask = new ProductionTask(task);
                    ProductionTask parent = new ProductionTask();
                    if (task.MyParent != null)
                    {
                        parent = new ProductionTask(task.MyParent.Parent);
                        if (!InProduction.Contains(parent))
                        {
                            InProduction.Add(parent);
                        }
                        parent.Children.Add(selectedTask);
                        parent.HasChildren = true;
                    }
                    else
                    {
                        InProduction.Add(selectedTask);
                    }
                }
            }
        }

        #endregion

        #region GenerateExecutorInProductionCommand

        public ICommand GenerateExecutorInProductionCommand { get; }

        private bool CanGenerateExecutorInProductionCommandExecute(object p) => ExecutorName != null;

        private void OnGenerateExecutorInProductionCommandExecuted(object p)
        {
            var tasks = Unit.Tasks.Get().Where(t => (String.Equals(t.InProduction.ExecutorName?.ToLower(), ExecutorName.ToLower()) || String.Equals(t.InProduction.ExecutorName2?.ToLower(), ExecutorName.ToLower())) && t.InProduction.CompletionDate == null && t.InProduction.GivingDate!=null &&t.InProduction.GivingDate > DateFrom && t.InProduction.GivingDate < DateTo);
            ExecutorInProduction = new TreeGridModel();
            foreach(var task in tasks)
            {
                ProductionTask selectedTask = new ProductionTask(task);
                if(task.MyParent != null)
                {
                    ProductionTask parent = new ProductionTask(task.MyParent.Parent);
                    ExecutorInProduction.Add(parent);
                    parent.Children.Add(selectedTask);
                    parent.HasChildren = true;
                }
                else
                {
                    ExecutorInProduction.Add(selectedTask);
                }
            }
        }

        #endregion

        #region GenerateExecutorCompletedCommand

        public ICommand GenerateExecutorCompletedCommand { get; }

        private bool CanGenerateExecutorCompletedCommandExecute(object p) => ExecutorName != null;

        private void OnGenerateExecutorCompletedCommandExecuted(object p)
        {
            var tasks = Unit.Tasks.Get().Where(t => (String.Equals(t.InProduction.ExecutorName?.ToLower(), ExecutorName.ToLower()) || String.Equals(t.InProduction.ExecutorName2?.ToLower(), ExecutorName.ToLower())) && t.InProduction.CompletionDate != null &&t.InProduction.CompletionDate > DateFrom && t.InProduction.CompletionDate < DateTo);
            foreach(var task in tasks)
            {
                ProductionTask selectedTask = new ProductionTask(task);
                if(task.MyParent != null)
                {
                    ProductionTask parent = new ProductionTask(task.MyParent.Parent);
                    ExecutorCompleted.Add(parent);
                    parent.Children.Add(selectedTask);
                    parent.HasChildren = true;
                }
                else
                {
                    ExecutorCompleted.Add(selectedTask);
                }
            }
        }

        #endregion

        #endregion

        public ReportsViewModel() { }

        public ReportsViewModel(ApplicationUnit unit)
        {
            #region Команды

            GenerateNoSpecificationReportCommand = new LambdaCommand(OnGenerateNoSpecificationReportCommandExecuted, CanGenerateNoSpecificationReportCommandExecute);
            GenerateGivingReportsCommand = new LambdaCommand(OnGenerateGivingReportsCommandExecuted, CanGenerateGivingReportsCommandExecute);
            GenerateGivingAvailabilityCommand = new LambdaCommand(OnGenerateGivingAvailabilityCommandExecuted, CanGenerateGivingAvailabilityCommandExecute);
            GenerateInProductionCommand = new LambdaCommand(OnGenerateInProductionCommandExecuted, CanGenerateInProductionCommandExecute);
            GenerateExecutorInProductionCommand = new LambdaCommand(OnGenerateExecutorInProductionCommandExecuted, CanGenerateExecutorInProductionCommandExecute);
            GenerateExecutorCompletedCommand = new LambdaCommand(OnGenerateExecutorCompletedCommandExecuted, CanGenerateExecutorCompletedCommandExecute);
            #endregion

            Unit = unit;
        }
    }
}

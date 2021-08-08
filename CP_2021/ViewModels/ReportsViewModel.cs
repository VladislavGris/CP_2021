using Common.Wpf.Data;
using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.PDF;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Units;
using CP_2021.Models.Classes;
using CP_2021.Models.DBModels;
using CP_2021.ViewModels.Base;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
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

        private ApplicationUnit Unit;

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

        #region Генерация отчетов в приложении

        #region GenerateNoSpecificationReportCommand

        public ICommand GenerateNoSpecificationReportCommand { get; }

        private bool CanGenerateNoSpecificationReportCommandExecute(object p) => true;

        private void OnGenerateNoSpecificationReportCommandExecuted(object p)
        {
            NoSpecification = new ObservableCollection<ProductionTaskDB>(Unit.Tasks.Get().
                Where(t => String.IsNullOrEmpty(t.Manufacture.SpecNum) &&
                            !String.IsNullOrEmpty(t.Manufacture.Name) &&
                            !String.IsNullOrEmpty(t.Manufacture.LetterNum)));
        }

        #endregion

        #region GenerateGivingReportsCommand

        public ICommand GenerateGivingReportsCommand { get; }

        private bool CanGenerateGivingReportsCommandExecute(object p) => true;

        private void OnGenerateGivingReportsCommandExecuted(object p)
        {
            GivingReports = new ObservableCollection<ProductionTaskDB>(Unit.Tasks.Get().
                Where(t => String.IsNullOrEmpty(t.Giving.Report) &&
                            !String.IsNullOrEmpty(t.Giving.Bill) &&
                            !String.IsNullOrEmpty(t.Manufacture.SpecNum)));
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
                if (task.Giving.State.HasValue && task.Giving.State.Value == true && 
                    task.Giving.ReceivingDate.HasValue && task.Giving.ReceivingDate >= DateFrom && task.Giving.ReceivingDate <= DateTo)
                {
                    ProductionTask selectedTask = new ProductionTask(task);
                    ProductionTask parent = new ProductionTask();
                    if (task.MyParent.Parent != null)
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
                if (task.Completion == 3)
                {
                    ProductionTask selectedTask = new ProductionTask(task);
                    ProductionTask parent = new ProductionTask();
                    if (task.MyParent.Parent != null)
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
            var tasks = Unit.Tasks.Get().
                Where(t => (String.Equals(t.InProduction.ExecutorName?.ToLower(), ExecutorName.ToLower()) ||
                            String.Equals(t.InProduction.ExecutorName2?.ToLower(), ExecutorName.ToLower())) &&
                            t.InProduction.CompletionDate == null &&
                            t.InProduction.GivingDate != null &&
                            t.InProduction.GivingDate >= DateFrom &&
                            t.InProduction.GivingDate <= DateTo);
            ExecutorInProduction = new TreeGridModel();
            foreach (var task in tasks)
            {
                ProductionTask selectedTask = new ProductionTask(task);
                if (task.MyParent.Parent != null)
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
            var tasks = Unit.Tasks.Get().
                Where(t => (String.Equals(t.InProduction.ExecutorName?.ToLower(), ExecutorName.ToLower()) ||
                            String.Equals(t.InProduction.ExecutorName2?.ToLower(), ExecutorName.ToLower())) &&
                            t.InProduction.CompletionDate != null &&
                            t.InProduction.CompletionDate >= DateFrom &&
                            t.InProduction.CompletionDate <= DateTo);
            ExecutorCompleted = new TreeGridModel();
            foreach (var task in tasks)
            {
                ProductionTask selectedTask = new ProductionTask(task);
                if (task.MyParent.Parent != null)
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

        #region Генерация PDF отчетов

        #region GenerateNoSpecPDFCommand

        public ICommand GenerateNoSpecPDFCommand { get; }

        private bool CanGenerateNoSpecPDFCommandExecute(object p) => NoSpecification != null;

        private void OnGenerateNoSpecPDFCommandExecuted(object p)
        {
            PdfDocument.GenerateNoSpecificationReport(NoSpecification);
        }

        #endregion

        #region GenerateGivingReportsPDFCommand

        public ICommand GenerateGivingReportsPDFCommand { get; }

        private bool CanGenerateGivingReportsPDFCommandExecute(object p) => GivingReports != null;

        private void OnGenerateGivingReportsPDFCommandExecuted(object p)
        {
            PdfDocument.GenerateGivingReportsPDF(GivingReports);
        }

        #endregion

        #region GenerateGivingAvailabilityPDFCommand

        public ICommand GenerateGivingAvailabilityPDFCommand { get; }

        private bool CanGenerateGivingAvailabilityPDFCommandExecute(object p) => GivingAvailability != null;

        private void OnGenerateGivingAvailabilityPDFCommandExecuted(object p)
        {
            PdfDocument.GenerateGivingAvailabilityPDF(GivingAvailability, DateFrom, DateTo);
        }

        #endregion

        #region GenerateInProductionPDFCommand

        public ICommand GenerateInProductionPDFCommand { get; }

        private bool CanGenerateInProductionPDFCommandExecute(object p) => InProduction != null;

        private void OnGenerateInProductionPDFCommandExecuted(object p)
        {
            PdfDocument.GenerateInProductionPDF(InProduction);
        }

        #endregion

        #region GenerateExecutorInProductionPDFCommand

        public ICommand GenerateExecutorInProductionPDFCommand { get; }

        private bool CanGenerateExecutorInProductionPDFCommandExecute(object p) => ExecutorInProduction != null;

        private void OnGenerateExecutorInProductionPDFCommandExecuted(object p)
        {
            PdfDocument.GenerateExecutorInProductionPDF(ExecutorInProduction, DateFrom, DateTo, ExecutorName);
        }

        #endregion

        #region GenerateExecutorCompletedPDFCommand

        public ICommand GenerateExecutorCompletedPDFCommand { get; }

        private bool CanGenerateExecutorCompletedPDFCommandExecute(object p) => ExecutorCompleted != null;

        private void OnGenerateExecutorCompletedPDFCommandExecuted(object p)
        {
            PdfDocument.GenerateExecutorCompletedPDF(ExecutorCompleted, DateFrom, DateTo, ExecutorName);
        }

        #endregion

        #endregion

        #endregion

        public ReportsViewModel() 
        {
            #region Команды
            GenerateNoSpecificationReportCommand = new LambdaCommand(OnGenerateNoSpecificationReportCommandExecuted, CanGenerateNoSpecificationReportCommandExecute);
            GenerateGivingReportsCommand = new LambdaCommand(OnGenerateGivingReportsCommandExecuted, CanGenerateGivingReportsCommandExecute);
            GenerateGivingAvailabilityCommand = new LambdaCommand(OnGenerateGivingAvailabilityCommandExecuted, CanGenerateGivingAvailabilityCommandExecute);
            GenerateInProductionCommand = new LambdaCommand(OnGenerateInProductionCommandExecuted, CanGenerateInProductionCommandExecute);
            GenerateExecutorInProductionCommand = new LambdaCommand(OnGenerateExecutorInProductionCommandExecuted, CanGenerateExecutorInProductionCommandExecute);
            GenerateExecutorCompletedCommand = new LambdaCommand(OnGenerateExecutorCompletedCommandExecuted, CanGenerateExecutorCompletedCommandExecute);
            GenerateNoSpecPDFCommand = new LambdaCommand(OnGenerateNoSpecPDFCommandExecuted, CanGenerateNoSpecPDFCommandExecute);
            GenerateGivingReportsPDFCommand = new LambdaCommand(OnGenerateGivingReportsPDFCommandExecuted, CanGenerateGivingReportsPDFCommandExecute);
            GenerateGivingAvailabilityPDFCommand = new LambdaCommand(OnGenerateGivingAvailabilityPDFCommandExecuted, CanGenerateGivingAvailabilityPDFCommandExecute);
            GenerateInProductionPDFCommand = new LambdaCommand(OnGenerateInProductionPDFCommandExecuted, CanGenerateInProductionPDFCommandExecute);
            GenerateExecutorInProductionPDFCommand = new LambdaCommand(OnGenerateExecutorInProductionPDFCommandExecuted, CanGenerateExecutorInProductionPDFCommandExecute);
            GenerateExecutorCompletedPDFCommand = new LambdaCommand(OnGenerateExecutorCompletedPDFCommandExecuted, CanGenerateExecutorCompletedPDFCommandExecute);
            #endregion

            Unit = ApplicationUnitSingleton.GetInstance().dbUnit;
        }
    }
}

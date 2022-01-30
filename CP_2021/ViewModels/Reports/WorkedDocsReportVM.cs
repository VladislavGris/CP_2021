using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.ViewEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CP_2021.ViewModels.Reports
{
    internal class WorkedDocsReportVM : BaseReport<WorkedDocs>
    {
        #region GenerateReportCommand

        public ICommand GenerateReportCommand { get; }

        private bool CanGenerateReportCommandExecute(object p) => true;

        private void OnGenerateReportCommandExecuted(object p)
        {
            var heads = TasksOperations.GetHeadTasks();
            var manufacturers = TasksOperations.GetManufactures();
            HeadTasks = new ObservableCollection<string>();
            Manufactirers = new ObservableCollection<string>();
            foreach (var head in heads)
            {
                HeadTasks.Add(head.Task);
            }
            foreach (var manufacture in manufacturers)
            {
                Manufactirers.Add(manufacture.Name);
            }
            FullContent = Content = new ObservableCollection<WorkedDocs>(TasksOperations.GetWorkedDocs());
        }

        #endregion

        #region DropFiltersCommand

        public ICommand DropFiltersCommand { get; }

        private bool CanDropFiltersCommandExecute(object p) => FullContent != null;

        private void OnDropFiltersCommandExecuted(object p)
        {
            SelectedHead = null;
            SelectedManufacture = null;
            Content = FullContent;
        }

        #endregion

        public WorkedDocsReportVM() : base()
        {
            GenerateReportCommand = new LambdaCommand(OnGenerateReportCommandExecuted, CanGenerateReportCommandExecute);
            DropFiltersCommand = new LambdaCommand(OnDropFiltersCommandExecuted, CanDropFiltersCommandExecute);
        }
    }
}

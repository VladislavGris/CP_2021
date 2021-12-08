using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Units;
using CP_2021.Models.DBModels;
using CP_2021.Models.ProcedureResuts.Plan;
using CP_2021.ViewModels.Base;

namespace CP_2021.ViewModels
{
    class ViewReportViewModel : ViewModelBase
    {
        #region Task

        private TaskReport _task;

        public TaskReport Task
        {
            get => _task;
            set => Set(ref _task, value);
        }

        #endregion

        public ViewReportViewModel() { }

        public ViewReportViewModel(TaskReport task)
        {
            Task = task;
        }
    }
}

using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Models.ProcedureResuts;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CP_2021.ViewModels.Reports
{
    internal class ByActVM : BaseReport<ByAct>
    {
        private readonly string _byActProc = "exec ByAct @dateFrom = {0}, @dateTo = {1}";

        #region DateFrom

        private DateTime _dateFrom;

        public DateTime DateFrom
        {
            get => _dateFrom;
            set => Set(ref _dateFrom, value);
        }

        #endregion

        #region DateTo

        private DateTime _dateTo;

        public DateTime DateTo
        {
            get => _dateTo;
            set => Set(ref _dateTo, value);
        }

        #endregion

        #region GenerateReportCommand

        public ICommand GenerateReportCommand { get; }

        protected bool CanGenerateReportCommandExecute(object p) => true;

        protected virtual void OnGenerateReportCommandExecuted(object p)
        {
            var heads = ApplicationUnitSingleton.GetInstance().dbUnit.HeadTasks.Get().OrderBy(t => t.Task);
            HeadTasks = new ObservableCollection<string>();
            foreach (var head in heads)
            {
                HeadTasks.Add(head.Task);
            }
            FullContent = Content = new ObservableCollection<ByAct>(ApplicationUnitSingleton.GetInstance().dbUnit.ByAct.GetWithRawSql(_byActProc,DateFrom,DateTo));
        }

        #endregion

        public ByActVM() : base()
        {
            GenerateReportCommand = new LambdaCommand(OnGenerateReportCommandExecuted, CanGenerateReportCommandExecute);

            DateFrom = DateTo = DateTime.Now;
        }
    }
}

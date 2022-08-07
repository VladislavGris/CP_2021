using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Models.ProcedureResuts.PaymentReports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CP_2021.ViewModels.Reports.Payment
{
    internal class FirstPaymentVM : BaseReport<PaymentFirstPart>
    {
        private readonly string _firstPaymentCommand = "exec PaymentFirstPart @dateFrom = {0}, @dateTo = {1}";

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

        public new ICommand GenerateReportCommand { get; }

        protected bool CanGenerateReportCommandExecute(object p) => true;

        override protected void OnGenerateReportCommandExecuted(object p)
        {
            var heads = ApplicationUnitSingleton.GetInstance().dbUnit.HeadTasks.Get().OrderBy(t => t.Task);
            HeadTasks = new ObservableCollection<string>();
            foreach (var head in heads)
            {
                HeadTasks.Add(head.Task);
            }
            FullContent = Content = new ObservableCollection<PaymentFirstPart>(ApplicationUnitSingleton.GetInstance().dbUnit.FirstPayment.GetWithRawSql(_firstPaymentCommand, DateFrom, DateTo));
        }

        #endregion

        public FirstPaymentVM() : base()
        {
            GenerateReportCommand = new LambdaCommand(OnGenerateReportCommandExecuted, CanGenerateReportCommandExecute);

            DateFrom = DateTo = DateTime.Now;
        }
    }
}

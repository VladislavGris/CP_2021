using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Units;
using CP_2021.Models.DBModels;
using CP_2021.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CP_2021.ViewModels
{
    class SendReportViewModel : ViewModelBase
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

        #region User

        private UserDB _user;

        public UserDB User
        {
            get => _user;
            set => Set(ref _user, value);
        }

        #endregion

        #region Report

        private ReportDB _report;

        public ReportDB Report
        {
            get => _report;
            set => Set(ref _report, value);
        }

        #endregion

        #endregion

        #region Команды

        #region SubmitCommand

        public ICommand SubmitCommand { get; }

        private bool CanSubmitCommandExecute(object p) => Report.Description!= null && Report.Description!="";

        private void OnSubmitCommandExecuted(object p)
        {
            Report.State = true;
            Unit.Commit();
            ((Window)p).Close();
        }

        #endregion

        #endregion

        public SendReportViewModel() { }

        public SendReportViewModel(ApplicationUnit unit, UserDB user, ReportDB report)
        {
            #region Команды

            SubmitCommand = new LambdaCommand(OnSubmitCommandExecuted, CanSubmitCommandExecute);

            #endregion
            Unit = unit;
            User = user;
            Report = report;
        }
    }
}

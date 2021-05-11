using CP_2021.Infrastructure.Units;
using CP_2021.Models.DBModels;
using CP_2021.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.ViewModels
{
    class ViewReportViewModel : ViewModelBase
    {

        #region Свойтсва

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

        public ViewReportViewModel() { }

        public ViewReportViewModel(ApplicationUnit unit, UserDB user, ReportDB report)
        {
            Report = report;
            Unit = unit;
            User = user;
        }
    }
}

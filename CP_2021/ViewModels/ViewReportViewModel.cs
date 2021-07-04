using CP_2021.Infrastructure.Singletons;
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
        private ApplicationUnit Unit;

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

        public ViewReportViewModel(ReportDB report)
        {
            Report = report;
            Unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            User = UserDataSingleton.GetInstance().user;
        }
    }
}

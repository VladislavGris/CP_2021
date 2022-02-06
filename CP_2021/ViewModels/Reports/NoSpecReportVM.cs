using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.ViewEntities;
using System.Collections.ObjectModel;

namespace CP_2021.ViewModels.Reports
{
    internal class NoSpecReportVM : BaseReport<NoSpec>
    {
        protected override void OnGenerateReportCommandExecuted(object p)
        {
            base.OnGenerateReportCommandExecuted(p);
            FullContent = Content = new ObservableCollection<NoSpec>(TasksOperations.GetNoSpec());
        }

        public NoSpecReportVM() : base() { }
    }
}

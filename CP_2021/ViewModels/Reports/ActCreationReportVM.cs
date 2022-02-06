using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.ViewEntities;
using System.Collections.ObjectModel;

namespace CP_2021.ViewModels.Reports
{
    internal class ActCreationReportVM : BaseReport<ActCreation>
    {
        protected override void OnGenerateReportCommandExecuted(object p)
        {
            base.OnGenerateReportCommandExecuted(p);
            FullContent = Content = new ObservableCollection<ActCreation>(TasksOperations.GetActCreation());

        }

        public ActCreationReportVM() : base() { }
    }
}

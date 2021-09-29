using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Models.ViewEntities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CP_2021.ViewModels.Reports
{
    internal class ActFormVM : BaseReport<ActForm>
    {
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
            FullContent = Content = new ObservableCollection<ActForm>(ApplicationUnitSingleton.GetInstance().dbUnit.ActForm.Get());
        }

        #endregion

        public ActFormVM() : base()
        {
            GenerateReportCommand = new LambdaCommand(OnGenerateReportCommandExecuted, CanGenerateReportCommandExecute);
        }
    }
}

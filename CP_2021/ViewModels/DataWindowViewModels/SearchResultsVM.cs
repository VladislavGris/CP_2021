using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Search;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Utils.CustomEventArgs;
using CP_2021.Models.ProcedureResuts;
using CP_2021.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace CP_2021.ViewModels.DataWindowViewModels
{
    internal class SearchResultsVM : ViewModelBase
    {
        private SearchManager _searchManager = new SearchManager();
        private readonly string _executeProcedure = "exec Search @parm = {0}";

        #region SearchString

        private string _searchString;

        public string SearchString
        {
            get => _searchString;
            set => Set(ref _searchString, value);
        }

        #endregion

        #region SearchResults

        private ObservableCollection<SearchProcResult> _searchResults;

        public ObservableCollection<SearchProcResult> SearchResults
        {
            get => _searchResults;
            set => Set(ref _searchResults, value);
        }

        #endregion

        #region SelectedRow

        private SearchProcResult _selectedRow;

        public SearchProcResult SelectedRow
        {
            get => _selectedRow;
            set => Set(ref _selectedRow, value);
        }

        #endregion

        #region SearchCommand

        public ICommand SearchCommand { get; }

        private bool CanSearchCommandExecute(object p) => true;

        private void OnSearchCommandExecuted(object p)
        {
            if (SearchString != null)
            {
                SearchResults = new ObservableCollection<SearchProcResult>(ApplicationUnitSingleton.GetInstance().dbUnit.SearchResults.GetWithRawSql(_executeProcedure, "%" + SearchString + "%"));
                MessageBox.Show("Количество совпадений: " + SearchResults.Count);
            }
        }

        #endregion

        #region GotoTaskCommand

        public ICommand GotoTaskCommand { get; }

        private bool CanGotoTaskCommandExecute(object p) => true;

        private void OnGotoTaskCommandExecuted(object p)
        {
            TaskIdEventArgs args = new TaskIdEventArgs() { Id = SelectedRow.Id };
            OnSendTaskIdToReportVM(args);
        }

        #endregion

        #region Events

        public event EventHandler<TaskIdEventArgs> SendTaskIdToReportVM;

        protected virtual void OnSendTaskIdToReportVM(TaskIdEventArgs e)
        {
            EventHandler<TaskIdEventArgs> handler = SendTaskIdToReportVM;
            handler?.Invoke(this, e);
        }

        #endregion

        public SearchResultsVM() {
            SearchCommand = new LambdaCommand(OnSearchCommandExecuted, CanSearchCommandExecute);
            GotoTaskCommand = new LambdaCommand(OnGotoTaskCommandExecuted, CanGotoTaskCommandExecute);
        }
    }
}

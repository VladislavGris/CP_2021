using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Search;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Utils.CustomEventArgs;
using CP_2021.Infrastructure.Utils.DB;
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

        #region SelectedIndex

        private int _selectedIndex = 0;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value);
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
            try
            {
                if (SearchString != null)
                {
                    switch (SelectedIndex)
                    {
                        case 0:
                            SearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchProductionTask($"\"{SearchString}*\""));
                            break;
                        case 1:
                            SearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchAct($"\"{SearchString}*\""));
                            break;
                        case 2:
                            SearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchComplectation($"\"{SearchString}*\""));
                            break;
                        case 3:
                            SearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchGiving($"\"{SearchString}*\""));
                            break;
                        case 4:
                            SearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchIn_Production($"\"{SearchString}*\""));
                            break;
                        case 5:
                            SearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchLaborCosts($"\"{SearchString}*\""));
                            break;
                        case 6:
                            SearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchManufacture($"\"{SearchString}*\""));
                            break;
                        case 7:
                            SearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchPayment($"\"{SearchString}*\""));
                            break;
                        default:
                            break;
                    }
                    MessageBox.Show("Количество совпадений: " + SearchResults.Count);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
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

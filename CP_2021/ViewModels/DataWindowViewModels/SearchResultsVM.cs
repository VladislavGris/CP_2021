using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Search;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Utils.CustomEventArgs;
using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.ProcedureResuts;
using CP_2021.Models.ViewEntities;
using CP_2021.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CP_2021.ViewModels.DataWindowViewModels
{
    internal class SearchResultsVM : ViewModelBase
    {
        private SearchManager _searchManager = new SearchManager();

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

        #region FullSearchResults

        private ObservableCollection<SearchProcResult> _fullSearchResults;

        public ObservableCollection<SearchProcResult> FullSearchResults
        {
            get => _fullSearchResults;
            set => Set(ref _fullSearchResults, value);
        }

        #endregion

        #region FilteredSearchResults

        private ObservableCollection<SearchProcResult> _filteredSearchResults;

        public ObservableCollection<SearchProcResult> FilteredSearchResults
        {
            get => _filteredSearchResults;
            set => Set(ref _filteredSearchResults, value);
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

        #region RootTasks

        private ObservableCollection<string> _rootTasks;

        public ObservableCollection<string> RootTasks
        {
            get => _rootTasks;
            set => Set(ref _rootTasks, value);
        }

        #endregion

        #region SelectedRootTask

        private string _selectedRootTask;

        public string SelectedRootTask
        {
            get => _selectedRootTask;
            set => Set(ref _selectedRootTask, value);
        }

        #endregion

        #region RootSubTasks

        private ObservableCollection<string> _rootSubTasks;

        public ObservableCollection<string> RootSubTasks
        {
            get => _rootSubTasks;
            set => Set(ref _rootSubTasks, value);
        }

        #endregion

        #region SelectedRootSubTask

        private string _selectedRootSubTask;

        public string SelectedRootSubTask
        {
            get => _selectedRootSubTask;
            set => Set(ref _selectedRootSubTask, value);
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
                            FullSearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchProductionTask($"N'%{SearchString}%'"));
                            break;
                        case 1:
                            FullSearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchAct($"N'%{SearchString}%'"));
                            break;
                        case 2:
                            FullSearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchComplectation($"N'%{SearchString}%'"));
                            break;
                        case 3:
                            FullSearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchGiving($"N'%{SearchString}%'"));
                            break;
                        case 4:
                            FullSearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchIn_Production($"N'%{SearchString}%'"));
                            break;
                        case 5:
                            FullSearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchLaborCosts($"N'%{SearchString}%'"));
                            break;
                        case 6:
                            FullSearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchManufacture($"N'%{SearchString}%'"));
                            break;
                        case 7:
                            FullSearchResults = new ObservableCollection<SearchProcResult>(TasksOperations.SearchPayment($"N'%{SearchString}%'"));
                            break;
                        default:
                            break;
                    }
                    FilteredSearchResults = FullSearchResults;
                    SelectedRootTask = null;
                    SelectedRootSubTask = null;

                    RootSubTasks = new ObservableCollection<string>();
                    RootTasks = new ObservableCollection<string>();

                    var heads = TasksOperations.GetHeadTasks();
                    foreach (var head in heads)
                    {
                        RootTasks.Add(head.Task);
                    }
                    MessageBox.Show("Количество совпадений: " + FullSearchResults.Count);
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

        #region RootTaskChangedCommand

        public ICommand RootTaskChangedCommand { get; }

        private bool CanRootTaskChangedCommandExecute(object p) => true;

        private void OnRootTaskChangedCommandExecuted(object p)
        {
            var tasks = TasksOperations.GetSubTasksByProjectName(SelectedRootTask);
            RootSubTasks = new ObservableCollection<string>();
            SelectedRootSubTask = null;
            foreach (var task in tasks)
            {
                RootSubTasks.Add(task.Name);
            }
            ApplyFilters();
        }

        #endregion

        #region RootSubTaskChangedCommand

        public ICommand RootSubTaskChangedCommand { get; }

        private bool CanRootSubTaskChangedCommandExecute(object p) => !string.IsNullOrEmpty(SelectedRootTask);

        private void OnRootSubTaskChangedCommandExecuted(object p)
        {
            ApplyFilters();
        }

        #endregion

        #region DropFiltersCommand

        public ICommand DropFiltersCommand { get; }

        private bool CanDropFiltersCommandExecute(object p) => true;

        private void OnDropFiltersCommandExecuted(object p)
        {
            SelectedRootTask = null;
            SelectedRootSubTask = null;
            FilteredSearchResults = FullSearchResults;
        }

        #endregion

        private void ApplyFilters()
        {
            FilteredSearchResults = FullSearchResults;
            if (!string.IsNullOrEmpty(SelectedRootTask))
            {
                FilteredSearchResults = new ObservableCollection<SearchProcResult>(FilteredSearchResults.Where(r=>r.RootTask == SelectedRootTask));
            }
            if(!string.IsNullOrEmpty(SelectedRootSubTask))
            {
                FilteredSearchResults = new ObservableCollection<SearchProcResult>(FilteredSearchResults.Where(r=>r.RootSubTask == SelectedRootSubTask));
            }
        }

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
            RootTaskChangedCommand = new LambdaCommand(OnRootTaskChangedCommandExecuted, CanRootTaskChangedCommandExecute);
            RootSubTaskChangedCommand = new LambdaCommand(OnRootSubTaskChangedCommandExecuted, CanRootSubTaskChangedCommandExecute);
            DropFiltersCommand = new LambdaCommand(OnDropFiltersCommandExecuted, CanDropFiltersCommandExecute);
        }

        public SearchResultsVM(string searchString) : this()
        {
            SearchString = searchString;
        }
    }
}

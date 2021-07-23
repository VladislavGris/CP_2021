﻿using CP_2021.Data;
using CP_2021.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CP_2021.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Common.Wpf.Data;
using CP_2021.Models.Classes;
using System.Windows.Input;
using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Units;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;
using CP_2021.Infrastructure.Search;
using CP_2021.Infrastructure.Search.SearchStrategies;
using CP_2021.Infrastructure.Exceptions;
using CP_2021.Infrastructure.Singletons;

namespace CP_2021.ViewModels
{
    class ProductionPlanControlViewModel : ViewModelBase
    {

        #region Свойства

        private ApplicationUnit Unit;

        #region User

        private UserDB _user;

        public UserDB User
        {
            get => _user;
            set => Set(ref _user, value);
        }

        #endregion

        #region TaskToCopy

        private ProductionTask _taskToCopy;

        public ProductionTask TaskToCopy
        {
            get => _taskToCopy;
            set => Set(ref _taskToCopy, value);
        }

        #endregion

        #region ProductionTasks
        private List<ProductionTaskDB> _productionTasks;

        public List<ProductionTaskDB> ProductionTasks
        {
            get => _productionTasks;
            set => Set(ref _productionTasks, value);
        }

        #endregion

        #region SelectedTask
        private ProductionTask _selectedTask;

        public ProductionTask SelectedTask
        {
            get => _selectedTask;
            set => Set(ref _selectedTask, value);
        }

        #endregion

        #region Model

        private TreeGridModel _model;

        public TreeGridModel Model
        {
            get => _model;
            set => Set(ref _model, value);
        }

        #endregion

        #region SearchString

        private string _searchString;

        public string SearchString
        {
            get => _searchString;
            set => Set(ref _searchString, value);
        }

        #endregion

        #region SelectedSearchIndex

        private int _selectedSearchIndex = 0;

        public int SelectedSearchIndex
        {
            get => _selectedSearchIndex;
            set => Set(ref _selectedSearchIndex, value);
        }

        #endregion

        #region SearchResults

        private List<ProductionTask> _searchResults;

        public List<ProductionTask> SearchResults
        {
            get => _searchResults;
            set => Set(ref _searchResults, value);
        }

        #endregion

        #region SearchResultString

        private string _searchResultString;

        public string SearchResultString
        {
            get => _searchResultString;
            set => Set(ref _searchResultString, value);
        }

        #endregion

        #region SearchComboBoxContent

        private List<string> _searchComboBoxContent = new List<string> { "Изделие", "Распорядительный документ", "Количество", "Стоимость по спецификации", "Приходный документ", "Желаемая дата", "Реальная дата", "Ведомость комплектации", "Дата комплектации", "Процент получения", "Номер МСЛ", "Сборка", "Электромонтаж", "Дата выдачи", "Дата готовности(п)", "Дата готовности", "Предприятие изготовитель", "Номер письма", "Номер спецификации", "Накладная", "Отчет", "Накладная возврата", "Дата поступления на склад", "Номер и дата акта расходования", "Примечания" };

        public List<string> SearchComboBoxContent
        {
            get => _searchComboBoxContent;
            set => Set(ref _searchComboBoxContent, value);
        }

        #endregion

        private SearchManager searchManager;

        #endregion

        #region Команды

        #region ExpandAllCommand

        public ICommand ExpandAllCommand { get; }

        private bool CanExpandAllCommandExecute(object p) => Model!=null;

        private void OnExpandAllCommandExecuted(object p)
        {
            Model.ExpandAll();
        }

        #endregion

        #region RollUpAllCommand

        public ICommand RollUpAllCommand { get; }

        private bool CanRollUpAllCommandExecute(object p) => Model != null;

        private void OnRollUpAllCommandExecuted(object p)
        {
            Model.CollapseAll();
        }

        #endregion
        //TODO: AddProductionTask completed
        #region AddProductionTaskCommand

        public ICommand AddProductionTaskCommand { get; }

        private bool CanAddProductionTaskCommandExecute(object p) =>true;

        private void OnAddProductionTaskCommandExecuted(object p)
        {
            if (SelectedTask?.Task.MyParent.Parent != null)
            {
                SelectedTask = SelectedTask.AddAtTheSameLevel();
            }
            else
            {
                SelectedTask = SelectedTask.AddEmptyRootToModel(Model);
            }
        }

        #endregion

        #region RowEditingEndingCommand

        public ICommand RowEditingEndingCommand { get; }

        private bool CanRowEditingEndingCommandExecute(object p) => true;

        private void OnRowEditingEndingCommandExecuted(object p)
        {
            Unit.Commit();
        }

        #endregion
        //TODO: AddChildCommand completed
        #region AddChildCommand

        public ICommand AddChildCommand { get; }

        private bool CanAddChildCommandExecute(object p) => SelectedTask != null;

        private void OnAddChildCommandExecuted(object p)
        {
            SelectedTask.IsExpanded = true;
            SelectedTask = SelectedTask.AddEmptyChild(); 
        }

        #endregion
        //TODO: DeleteProductionTask completed
        #region DeleteProductionTaskCommand

        public ICommand DeleteProductionTaskCommand { get; }

        private bool CanDeleteProductionTaskCommandExecute(object p) => SelectedTask != null;

        private void OnDeleteProductionTaskCommandExecuted(object p)
        {
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить элемент {SelectedTask.Task.Name}?", "Удаление", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    ProductionTask parent = (ProductionTask)SelectedTask.Parent;
                    SelectedTask.Remove();
                    if (parent == null)
                    {
                        if (Model.Count != 0)
                            SelectedTask = (ProductionTask)Model.Last();
                        else
                            SelectedTask = null;
                    }
                    else
                    {
                        if (parent.Children.Count != 0)
                            SelectedTask = (ProductionTask)parent.Children.Last();
                        else
                            SelectedTask = parent;
                    }
                    if (parent != null)
                    {
                        parent.CheckTaskHasChildren();
                    }
                    break;
            }
        }

        #endregion
        //TODO: LevelUpCommand completed
        #region LevelUpCommand

        public ICommand LevelUpCommand { get; }

        private bool CanLevelUpCommandExecute(object p) => SelectedTask?.Parent != null;

        private void OnLevelUpCommandExecuted(object p)
        {
            ProductionTask parent = (ProductionTask)SelectedTask.Parent;
            ProductionTask task = ProductionTask.InitTask(SelectedTask.Task);
            SelectedTask.IsExpanded = false;
            SelectedTask.DownOrderBelow();
            parent.Children.Remove(SelectedTask);
            task.Task.MyParent.LineOrder = parent.Task.MyParent.LineOrder + 1;
            if (parent.Parent == null)
            {
                task.Task.MyParent.Parent = null;
                Model.Insert(task.Task.MyParent.LineOrder - 1, task);
            }
            else
            {
                task.Task.MyParent.Parent = ((ProductionTask)parent.Parent).Task;
                parent.Parent.Children.Insert(task.Task.MyParent.LineOrder - 1, task);
            }
            task.UpOrderBelow();
            parent.CheckTaskHasChildren();
            Unit.Commit();
            SelectedTask = task;
        }

        #endregion
        //TODO: LevelDownCommand completed
        #region LevelDownCommand

        public ICommand LevelDownCommand { get; }

        private bool CanLevelDownCommandExecute(object p) => SelectedTask != null;

        private void OnLevelDownCommandExecuted(object p)
        {
            int selectedTaskOrder = SelectedTask.Task.MyParent.LineOrder;
            ProductionTaskDB dbTask = new ProductionTaskDB("Новое изделие");
            ProductionTask task = new ProductionTask(dbTask);
            ProductionTask downTask = ProductionTask.InitTask(SelectedTask.Task);
            ProductionTask parent = (ProductionTask)SelectedTask.Parent;

            if (SelectedTask.Parent != null)
            {
                dbTask.MyParent = new HierarchyDB(SelectedTask.Task.MyParent.Parent, dbTask);
                SelectedTask.Task.MyParent.Parent = dbTask;
                parent.Children.Insert(selectedTaskOrder - 1, task);
                parent.Children.Remove(SelectedTask);
            }
            else
            {
                dbTask.MyParent = new HierarchyDB(dbTask);
                SelectedTask.Task.MyParent = new HierarchyDB(dbTask, SelectedTask.Task);
                Model.Insert(selectedTaskOrder - 1, task);
                Model.Remove(SelectedTask);
            }
            task.Task.MyParent.LineOrder = selectedTaskOrder;
            downTask.Task.MyParent.LineOrder = 1;
            task.Children.Add(downTask);
            task.HasChildren = true;
            task.IsExpanded = true;
            Unit.Tasks.Insert(dbTask);
            Unit.Commit();
            SelectedTask = downTask;
        }

        #endregion

        #region UpdateModelCommand

        public ICommand UpdateModelCommand { get; }

        private bool CanUpdateModelCommandExecute(object p) => true;

        private void OnUpdateModelCommandExecuted(object p)
        {
            Unit.Refresh();
            ProductionTasks = Unit.Tasks.Get().ToList();
            Model = ProductionTask.InitModel(ProductionTasks);
        }

        #endregion

        #region CopyTaskCommand

        public ICommand CopyTaskCommand { get; }

        private bool CanCopyTaskCommandExecute(object p) => true;

        private void OnCopyTaskCommandExecuted(object p)
        {
            TaskToCopy = SelectedTask.Clone();
        }

        #endregion

        #region PasteTaskCommand

        public ICommand PasteTaskCommand { get; }

        private bool CanPasteTaskCommandExecute(object p) => TaskToCopy != null;

        private void OnPasteTaskCommandExecuted(object p)
        {
            ProductionTaskDB dbTask = TaskToCopy.Task.Clone();
            ProductionTask task = new ProductionTask(dbTask);
            SelectedTask.UpOrderBelow();

            if(SelectedTask.Parent == null)
            {
                dbTask.MyParent = new HierarchyDB(dbTask);
                Model.Insert(SelectedTask.Task.MyParent.LineOrder, task);
            }
            else
            {
                dbTask.MyParent = new HierarchyDB(SelectedTask.Task.MyParent.Parent, dbTask);
                ((ProductionTask)SelectedTask.Parent).Children.Insert(SelectedTask.Task.MyParent.LineOrder, task);
            }
            dbTask.MyParent.LineOrder = SelectedTask.Task.MyParent.LineOrder + 1;

            Unit.Tasks.Insert(dbTask);
            Unit.Commit();

            if (TaskToCopy.HasChildren)
            {
                task.HasChildren = true;
                foreach(ProductionTask child in TaskToCopy.Children)
                {
                    task.AddChild(child);
                }
            }
        }

        #endregion

        #region SearchCommands
        #region ShowSearchGridCommand

        public ICommand ShowSearchGridCommand { get; }

        private bool CanShowSearchGridCommandExecute(object p) => true;

        private void OnShowSearchGridCommandExecuted(object p)
        {
            if (((Grid)p).RowDefinitions.ElementAt(2).Height != new GridLength(0))
            {
                ((Grid)p).RowDefinitions.ElementAt(2).Height = new GridLength(0);
            }
            else
            {
                ((Grid)p).RowDefinitions.ElementAt(2).Height = new GridLength(40);
            }
        }

        #endregion

        #region SearchCommand

        public ICommand SearchCommand { get; }

        private bool CanSearchCommandExecute(object p) => SearchString != null;

        private void OnSearchCommandExecuted(object p)
        {
            SearchResultString = null;
            SearchResults = new List<ProductionTask>();
            FieldNames fieldName = (FieldNames)SelectedSearchIndex;
            try
            {
                switch (fieldName)
                {
                    case FieldNames.Name:
                        searchManager.SetSearchStrategy(new NameSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.ManagDoc:
                        searchManager.SetSearchStrategy(new ManagDocSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.Count:
                        searchManager.SetSearchStrategy(new CountSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.SpecificationCost:
                        searchManager.SetSearchStrategy(new SpecificationCostSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.IncDoc:
                        searchManager.SetSearchStrategy(new IncDocSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.VishDate:
                        searchManager.SetSearchStrategy(new VishDateSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.RealDate:
                        searchManager.SetSearchStrategy(new RealDateSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.Complectation:
                        searchManager.SetSearchStrategy(new ComplectationSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.ComplectationDate:
                        searchManager.SetSearchStrategy(new ComplectationDateSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.Percent:
                        searchManager.SetSearchStrategy(new PercentSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.MSLNumber:
                        searchManager.SetSearchStrategy(new MSLNumberSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.Executor:
                        searchManager.SetSearchStrategy(new FirstExecutorSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.Executor2:
                        searchManager.SetSearchStrategy(new SecondExecutorSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.GivingDate:
                        searchManager.SetSearchStrategy(new GivingDateSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.ProjectedDate:
                        searchManager.SetSearchStrategy(new ProjectedDateSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.ReadyDate:
                        searchManager.SetSearchStrategy(new CompletionDateSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.Manufacture:
                        searchManager.SetSearchStrategy(new ManufactureSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.LetterNum:
                        searchManager.SetSearchStrategy(new LetterNumSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.SpecNum:
                        searchManager.SetSearchStrategy(new SpecNumSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.Bill:
                        searchManager.SetSearchStrategy(new BillSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.Report:
                        searchManager.SetSearchStrategy(new ReportSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.ReturnReport:
                        searchManager.SetSearchStrategy(new ReturnReportSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.ReceivingDate:
                        searchManager.SetSearchStrategy(new ReceivingDateSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.ExpendNum:
                        searchManager.SetSearchStrategy(new ExpendNumSearchStrategy(Model, SearchString));
                        break;
                    case FieldNames.Note:
                        searchManager.SetSearchStrategy(new NoteSearchStrategy(Model, SearchString));
                        break;
                    default:
                        SearchResultString = "Параметр для поиска не задан";
                        break;

                }
                searchManager.ExecuteSearchStrategy();
                SearchResults = searchManager.GetSearchResults();
            }
            catch (Exception ex)
            {
                if (ex is IncorrectDateFormatException or IncorrectSearchValueException)
                {
                    SearchResultString = ex.Message;
                }
            }

            if (SearchResultString == null)
            {
                if (SearchResults.Count == 0)
                {
                    SearchResultString = "Совпадений не найдено";
                }
                else
                {
                    SearchResultString = $"Количество совпадений: {SearchResults.Count}";
                    SelectedTask = SearchResults.First();
                    ((DataGrid)p).UpdateLayout();
                    ((DataGrid)p).ScrollIntoView(SelectedTask);
                }
            }
        }

        #endregion

        #region MoveNextResultCommand

        public ICommand MoveNextResultCommand { get; }

        private bool CanMoveNextResultCommandExecute(object p) => SelectedTask != null && SearchResults != null && SearchResults.Count != 0 && SelectedTask != SearchResults.Last();

        private void OnMoveNextResultCommandExecuted(object p)
        {
            if (SearchResults.Contains(SelectedTask))
            {
                int resultIndex = SearchResults.IndexOf(SelectedTask);
                SelectedTask = SearchResults.ElementAt(resultIndex + 1);
            }
            else
            {
                SelectedTask = SearchResults.Last();
            }
            ((DataGrid)p).UpdateLayout();
            ((DataGrid)p).ScrollIntoView(SelectedTask);
        }

        #endregion

        #region MovePreviousResultCommand

        public ICommand MovePreviousResultCommand { get; }

        private bool CanMovePreviousResultCommandExecute(object p) => SelectedTask != null && SearchResults != null && SearchResults.Count != 0 && SelectedTask != SearchResults.First();

        private void OnMovePreviousResultCommandExecuted(object p)
        {
            if (SearchResults.Contains(SelectedTask))
            {
                int resultIndex = SearchResults.IndexOf(SelectedTask);
                SelectedTask = SearchResults.ElementAt(resultIndex - 1);
            }
            else
            {
                SelectedTask = SearchResults.First();
            }
            ((DataGrid)p).UpdateLayout();
            ((DataGrid)p).ScrollIntoView(SelectedTask);
        }

        #endregion
        #endregion

        //TODO: В случае необходимости реализовать функции и добавить столбец IsExpanded в БД
        #region OnCollapsingCommand

        public ICommand OnCollapsingCommand { get; }

        private bool CanOnCollapsingCommandExecute(object p) => true;

        private void OnOnCollapsingCommandExecuted(object p)
        {
            if(p is ProductionTask)
            {
                ProductionTask task = (ProductionTask)p;
                Debug.WriteLine(task.Task.Name);
            }
            Debug.WriteLine("Collapsed");
        }

        #endregion
        #region OnExpandingCommand

        public ICommand OnExpandingCommand { get; }

        private bool CanOnExpandingCommandExecute(object p) => true;

        private void OnOnExpandingCommandExecuted(object p)
        {
            if (p is ProductionTask)
            {
                ProductionTask task = (ProductionTask)p;
                Debug.WriteLine(task.Task.Name);
            }
            Debug.WriteLine("Expanded");
        }

        #endregion

        #endregion

        public ProductionPlanControlViewModel()
        {
            #region Команды

            ExpandAllCommand = new LambdaCommand(OnExpandAllCommandExecuted, CanExpandAllCommandExecute);
            RollUpAllCommand = new LambdaCommand(OnRollUpAllCommandExecuted, CanRollUpAllCommandExecute);
            AddProductionTaskCommand = new LambdaCommand(OnAddProductionTaskCommandExecuted, CanAddProductionTaskCommandExecute);
            AddChildCommand = new LambdaCommand(OnAddChildCommandExecuted, CanAddChildCommandExecute);
            DeleteProductionTaskCommand = new LambdaCommand(OnDeleteProductionTaskCommandExecuted, CanDeleteProductionTaskCommandExecute);
            RowEditingEndingCommand = new LambdaCommand(OnRowEditingEndingCommandExecuted, CanRowEditingEndingCommandExecute);
            LevelUpCommand = new LambdaCommand(OnLevelUpCommandExecuted, CanLevelUpCommandExecute);
            LevelDownCommand = new LambdaCommand(OnLevelDownCommandExecuted, CanLevelDownCommandExecute);
            UpdateModelCommand = new LambdaCommand(OnUpdateModelCommandExecuted, CanUpdateModelCommandExecute);
            CopyTaskCommand = new LambdaCommand(OnCopyTaskCommandExecuted, CanCopyTaskCommandExecute);
            PasteTaskCommand = new LambdaCommand(OnPasteTaskCommandExecuted, CanPasteTaskCommandExecute);
            SearchCommand = new LambdaCommand(OnSearchCommandExecuted, CanSearchCommandExecute);
            ShowSearchGridCommand = new LambdaCommand(OnShowSearchGridCommandExecuted, CanShowSearchGridCommandExecute);
            MoveNextResultCommand = new LambdaCommand(OnMoveNextResultCommandExecuted, CanMoveNextResultCommandExecute);
            MovePreviousResultCommand = new LambdaCommand(OnMovePreviousResultCommandExecuted, CanMovePreviousResultCommandExecute);
            OnCollapsingCommand = new LambdaCommand(OnOnCollapsingCommandExecuted, CanOnCollapsingCommandExecute);
            OnExpandingCommand = new LambdaCommand(OnOnExpandingCommandExecuted, CanOnExpandingCommandExecute);

            #endregion

            User = UserDataSingleton.GetInstance().user;
            Unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            ProductionTasks = Unit.Tasks.Get().ToList();
            //TODO: InitModel ready
            Model = ProductionTask.InitModel(ProductionTasks);
            searchManager = new SearchManager();
        }
    }
}

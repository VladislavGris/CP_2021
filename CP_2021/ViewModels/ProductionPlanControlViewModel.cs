using Common.Wpf.Data;
using CP_2021.Data;
using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Exceptions;
using CP_2021.Infrastructure.Search;
using CP_2021.Infrastructure.Search.SearchStrategies;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.UndoRedo;
using CP_2021.Infrastructure.Units;
using CP_2021.Infrastructure.Utils.CustomEventArgs;
using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.Classes;
using CP_2021.Models.DBModels;
using CP_2021.Models.ProcedureResuts.Plan;
using CP_2021.ViewModels.Base;
using CP_2021.ViewModels.DataWindowViewModels;
using CP_2021.Views.Windows;
using CP_2021.Views.Windows.DataWindows;
using log4net;
using log4net.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CP_2021.ViewModels
{
    class ProductionPlanControlViewModel : ViewModelBase
    {

        #region Свойства

        private ApplicationUnit Unit;
        private UndoRedoManager _undoManager;
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        #region DataTable

        private DataGrid _dataTable;

        public DataGrid DataTable
        {
            get => _dataTable;
            set => Set(ref _dataTable, value);
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

        #region FontSizes

        private List<int> _fontSizes;

        public List<int> FontSizes
        {
            get => _fontSizes;
            set => Set(ref _fontSizes, value);
        }

        #endregion

        #region FontFamilies

        private List<string> _fontFamilies;

        public List<string> FontFamilies
        {
            get => _fontFamilies;
            set => Set(ref _fontFamilies, value);
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

        #region UpdatingMessage

        private string _updatingMessage;

        public string UpdatingMessage
        {
            get => _updatingMessage;
            set => Set(ref _updatingMessage, value);
        }

        #endregion

        private SearchManager searchManager;

        #endregion

        #region Команды

        #region ExpandAllCommand

        public ICommand ExpandAllCommand { get; }

        private bool CanExpandAllCommandExecute(object p) => /*true;*/ false;

        private void OnExpandAllCommandExecuted(object p)
        {
            try
            {
                foreach(ProductionTaskDB task in Unit.Tasks.Get()){
                    task.Expanded = true;
                }
                Unit.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка. Обновите базу");
                _log.Error("UNKNOWN | ProductionPlanControlViewModel::ExpandAllCommand | " + ex.GetType().Name + " | " + ex.Message);
            }
            Update();
        }

        #endregion
        #region RollUpAllCommand

        public ICommand RollUpAllCommand { get; }

        private bool CanRollUpAllCommandExecute(object p) => /*true;*/ false;

        private void OnRollUpAllCommandExecuted(object p)
        {
            try
            {
                foreach (ProductionTaskDB task in Unit.Tasks.Get())
                {
                    task.Expanded = false;
                }
                Unit.Commit();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка. Обновите базу");
                _log.Error("UNKNOWN | ProductionPlanControlViewModel::RollUpAllCommand | " + ex.GetType().Name + " | " + ex.Message);
            }
            Update();
        }

        #endregion
        #region AddProductionTaskCommand

        public ICommand AddProductionTaskCommand { get; }

        private bool CanAddProductionTaskCommandExecute(object p) =>true;

        private void OnAddProductionTaskCommandExecuted(object p)
        {
            // Пустая БД
            if(Model.Count == 0)
            {
                Task_Hierarchy_Formatting insertedTask = TasksOperations.InsertEmptyTask(null, 1);
                if(insertedTask == null)
                {
                    MessageBox.Show("При вставке нового значения произошла ошибка");
                    return;
                }
                Model.Add(new ProductionTask(insertedTask));
                return;
            }
            if (SelectedTask == null)
            {
                MessageBox.Show("Не выбран элемент для добавления");
                return;
            }
            
            // Добавить задачу на тот же уровень под выбранную задачу
            Task_Hierarchy_Formatting task = TasksOperations.InsertEmptyTask(SelectedTask.data.ParentId, SelectedTask.data.LineOrder+1);
            ProductionTask pTask = new ProductionTask(task);
            // Корневая задача или дочерняя
            if (SelectedTask.Parent == null)
            {
                SelectedTask.DownTasksModel(Model);
                Model.Insert(task.LineOrder - 1, pTask);
            }
            else
            {
                SelectedTask.DownTasksChildren((ProductionTask)SelectedTask.Parent);
                SelectedTask.Parent.Children.Insert(task.LineOrder - 1, pTask);
            }
            SelectedTask = pTask;
        }

        #endregion
        #region AddChildCommand

        public ICommand AddChildCommand { get; }

        private bool CanAddChildCommandExecute(object p) => SelectedTask != null;

        private void OnAddChildCommandExecuted(object p)
        {
            int line;
            // Добавляется первый дочерний элемент или нет
            if (SelectedTask.data.ChildrenCount == 0)
                line = 1;
            else
                line = SelectedTask.data.ChildrenCount + 1;
            // Добавить задачу как дочернюю к SelectedItem в БД
            Task_Hierarchy_Formatting task = TasksOperations.InsertEmptyTask(SelectedTask.data.Id, line);
            ProductionTask pTask = new ProductionTask(task);

            SelectedTask.data.ChildrenCount += 1;
            SelectedTask.HasChildren = true;
            if (SelectedTask.IsExpanded)
            {
                SelectedTask.Children.Add(pTask);
                SelectedTask = pTask;
            }
        }

        #endregion
        #region RowEditingEndingCommand

        public ICommand RowEditingEndingCommand { get; }

        private bool CanRowEditingEndingCommandExecute(object p) => true;

        private void OnRowEditingEndingCommandExecuted(object p)
        {
            try
            {
                Unit.Commit();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                MessageBox.Show("Редактируемая строка была удалена. Обновите базу");
                _log.Warn("ProductionPlanControlViewModel::RowEditingEndingCommand | " + ex.GetType().Name + " | " + ex.Message);
            }catch(Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка. Обновите базу");
                _log.Error("UNKNOWN | ProductionPlanControlViewModel::RowEditingEndingCommand | " + ex.GetType().Name + " | " + ex.Message);
            }
            //Update();
        }

        #endregion
        #region DeleteProductionTaskCommand

        public ICommand DeleteProductionTaskCommand { get; }

        private bool CanDeleteProductionTaskCommandExecute(object p) => SelectedTask != null;

        private void OnDeleteProductionTaskCommandExecuted(object p)
        {
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить элемент {SelectedTask.Task.Name}?", "Удаление", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    ProductionTaskDB dbTask = Unit.Tasks.Get().Where(t => t.Id == SelectedTask.Task.Id).SingleOrDefault();
                    if (dbTask != null)
                    {
                        try
                        {
                            dbTask.UpOrderBelow();
                            dbTask.Remove();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Неизвестная ошибка");
                            _log.Error("ProductionPlanControlViewModel::DeleteProductionTaskCommand " + ex.Message);
                        }
            }
                    else
                    {
                        MessageBox.Show("Выбранный элемент уже был удален");
                    }
                    break;
            }
            Update();
        }

        #endregion
        #region LevelUpCommand

        public ICommand LevelUpCommand { get; }

        private bool CanLevelUpCommandExecute(object p) => SelectedTask?.Parent != null && SelectedTask != null;

        private void OnLevelUpCommandExecuted(object p)
        {
            try
            {
                ProductionTaskDB dbTask = Unit.Tasks.Get().Where(t => t.Id == SelectedTask.Task.Id).SingleOrDefault();
                if (dbTask == null)
                {
                    MessageBox.Show("Выделенная строка была удалена");
                }
                else
                {
                    dbTask.UpOrderBelow();
                    if (dbTask.MyParent.Parent.MyParent.Parent == null)
                    {
                        int lineOrder = dbTask.MyParent.Parent.MyParent.LineOrder + 1;
                        dbTask.MyParent = new HierarchyDB(dbTask);
                        dbTask.MyParent.LineOrder = lineOrder;
                        Unit.Commit();
                        dbTask.DownTaskBelow();
                    }
                    else
                    {
                        int lineOrder = dbTask.MyParent.Parent.MyParent.LineOrder + 1;
                        dbTask.MyParent = new HierarchyDB(dbTask.MyParent.Parent.MyParent.Parent, dbTask);
                        dbTask.MyParent.LineOrder = lineOrder;
                        Unit.Commit();
                        dbTask.DownTaskBelow();
                    }
                    Unit.Commit();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("ProductionPlanControlViewModel::LevelUpCommand " + ex.Message);
            }
            Update();
        }

        #endregion
        #region LevelDownCommand

        public ICommand LevelDownCommand { get; }

        private bool CanLevelDownCommandExecute(object p) => /*SelectedTask != null && SelectedTask.Task.MyParent.LineOrder != 1;*/ true;

        private void OnLevelDownCommandExecuted(object p)
        {
            ProductionTaskDB dbTask = Unit.Tasks.Get().Where(t=>t.Id == SelectedTask.Task.Id).SingleOrDefault();
            try
            {
                if (dbTask == null)
                {
                    MessageBox.Show("Выделенная строка была удалена");
                }
                else
                {
                    dbTask.UpOrderBelow();
                    ProductionTaskDB parent = Unit.Tasks.Get().Where(t => t.MyParent.ParentId == dbTask.MyParent.ParentId && t.MyParent.LineOrder == dbTask.MyParent.LineOrder - 1).Single();
                    parent.Expanded = true;
                    dbTask.MyParent = new HierarchyDB(parent, dbTask);
                    dbTask.MyParent.LineOrder = 1;
                    Unit.Commit();
                    dbTask.DownTaskBelow();
                    Unit.Commit();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("ProductionPlanControlViewModel::LevelDownCommand " + ex.Message);
            }
            Update();
            SelectedTask = ProductionTask.FindByTask(Model, dbTask);
        }

        #endregion
        #region UpdateModelCommand

        public ICommand UpdateModelCommand { get; }

        private bool CanUpdateModelCommandExecute(object p) => true;

        private void OnUpdateModelCommandExecuted(object p)
        {
            Model = ProductionTask.InitRootsModel();
        }

        #endregion
        #region CopyTaskCommand

        public ICommand CopyTaskCommand { get; }

        private bool CanCopyTaskCommandExecute(object p) => SelectedTask != null;

        private void OnCopyTaskCommandExecuted(object p)
        {
            try
            {
                TaskToCopy = ProductionTask.InitTask(SelectedTask.Task);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка. Обновите базу");
                _log.Error("UNKNOWN | ProductionPlanControlViewModel::CopyTaskCommand | " + ex.GetType().Name + " | " + ex.Message);
            }
            
        }

        #endregion
        #region CutTaskCommand

        public ICommand CutTaskCommand { get; }

        private bool CanCutTaskCommandExecute(object p) => SelectedTask!=null;

        private void OnCutTaskCommandExecuted(object p)
        {
            try
            {
                TaskToCopy = ProductionTask.InitTask(SelectedTask.Task);
                ProductionTaskDB dbTask = Unit.Tasks.Get().Where(t => t.Id == SelectedTask.Task.Id).SingleOrDefault();
                if (dbTask != null)
                {
                    try
                    {
                        dbTask.UpOrderBelow();
                        dbTask.Remove();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Неизвестная ошибка");
                        _log.Error("ProductionPlanControlViewModel::DeleteProductionTaskCommand " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Выбранный элемент уже был удален");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка. Обновите базу");
                _log.Error("UNKNOWN | ProductionPlanControlViewModel::CutTaskCommand | " + ex.GetType().Name + " | " + ex.Message);
            }
            Update();
        }

        #endregion
        #region PasteTaskCommand

        public ICommand PasteTaskCommand { get; }

        private bool CanPasteTaskCommandExecute(object p) => TaskToCopy != null;

        private void OnPasteTaskCommandExecuted(object p)
        {
            ProductionTaskDB dbTask = TaskToCopy.Task.Clone();
            try
            {
                ProductionTask task = new ProductionTask(dbTask);
                SelectedTask.Task.DownTaskBelow();

                if (SelectedTask.Parent == null)
                {
                    dbTask.MyParent = new HierarchyDB(dbTask);
                }
                else
                {
                    dbTask.MyParent = new HierarchyDB(SelectedTask.Task.MyParent.Parent, dbTask);
                }
                dbTask.MyParent.LineOrder = SelectedTask.Task.MyParent.LineOrder + 1;

                Unit.Tasks.Insert(dbTask);
                Unit.Commit();

                if (TaskToCopy.HasChildren)
                {
                    foreach (ProductionTask child in TaskToCopy.Children)
                    {
                        task.AddChild(child);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка. Обновите базу");
                _log.Error("UNKNOWN | ProductionPlanControlViewModel::PasteTaskCommand | " + ex.GetType().Name + " | " + ex.Message);
            }
            Update();
            SelectedTask = ProductionTask.FindByTask(Model, dbTask);
        }

        #endregion
        #region SearchCommands

        #region SearchCommand

        public ICommand SearchCommand { get; }

        private bool CanSearchCommandExecute(object p) => SearchString != null;

        private void OnSearchCommandExecuted(object p)
        {
            Update();
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
                if(SearchResults.Count != 0) 
                {
                    MessageBox.Show("Поиск завершен. Количество совпадений: " + SearchResults.Count);
                }
                else
                {
                    MessageBox.Show("Поиск завершен. Совпадений не найдено");
                }
            }
            catch (Exception ex)
            {
                if (ex is IncorrectDateFormatException or IncorrectSearchValueException)
                {
                    SearchResultString = ex.Message;
                    MessageBox.Show(ex.Message);
                }
            }

            if (SearchResultString == null)
            {
                if (SearchResults.Count != 0)
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
                if (SelectedTask.Equals(SearchResults.Last()))
                {
                    MessageBox.Show("Конец результатов поиска");
                }
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
        #region UpTaskCommand

        public ICommand UpTaskCommand { get; }

        private bool CanUpTaskCommandExecute(object p) => /*SelectedTask?.Task.MyParent.LineOrder != 1 && SelectedTask!=null;*/ true;

        private void OnUpTaskCommandExecuted(object p)
        {
            ProductionTaskDB taskToUp = Unit.Tasks.Get().Where(t => t.Id == SelectedTask.Task.Id).SingleOrDefault();
            try
            {
                ProductionTaskDB taskToDown = Unit.Tasks.Get().Where(t => t.MyParent.ParentId == taskToUp.MyParent.ParentId && t.MyParent.LineOrder == taskToUp.MyParent.LineOrder - 1).SingleOrDefault();
                if (taskToUp == null)
                {
                    MessageBox.Show("Выбранная строка была удалена");
                }
                else
                {
                    taskToUp.MyParent.LineOrder--;
                    taskToDown.MyParent.LineOrder++;
                    Unit.Commit();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("ProductionPlanControlViewModel::UpTaskCommand " + ex.Message);
            }
            Update();
            SelectedTask = ProductionTask.FindByTask(Model, taskToUp);
        }

        #endregion
        #region DownTaskCommand

        public ICommand DownTaskCommand { get; }

        private bool CanDownTaskCommandExecute(object p) => true;
        //{
        //    if (SelectedTask != null)
        //    {
        //        int selecetedTaskLineOrder = SelectedTask.Task.MyParent.LineOrder;
        //        int maxLineOrderByParent = Unit.Tasks.Get().Where(t => t.MyParent.Parent == SelectedTask.Task.MyParent.Parent).Max(t => t.MyParent.LineOrder);
        //        Debug.WriteLine("selected = " + selecetedTaskLineOrder + "; max = " + maxLineOrderByParent);
        //        return selecetedTaskLineOrder != maxLineOrderByParent;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        private void OnDownTaskCommandExecuted(object p)
        {
            ProductionTaskDB taskToDown = Unit.Tasks.Get().Where(t => t.Id == SelectedTask.Task.Id).SingleOrDefault();
            try
            {
                ProductionTaskDB taskToUp = Unit.Tasks.Get().Where(t => t.MyParent.ParentId == taskToDown.MyParent.ParentId && t.MyParent.LineOrder == taskToDown.MyParent.LineOrder + 1).SingleOrDefault();
                if (taskToDown == null)
                {
                    MessageBox.Show("Выбранная строка была удалена");
                }
                else
                {
                    taskToUp.MyParent.LineOrder--;
                    taskToDown.MyParent.LineOrder++;
                    Unit.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("ProductionPlanControlViewModel::DownTaskCommand " + ex.Message);
            }
            Update();
            SelectedTask = ProductionTask.FindByTask(Model, taskToDown);
        }

        #endregion
        #region UndoCommand

        public ICommand UndoCommand { get; }

        private bool CanUndoCommandExecute(object p) => _undoManager.UndoStackEmpty();

        private void OnUndoCommandExecuted(object p)
        {
            try
            {
                _undoManager.ExecuteUndoCommand();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка. Обновите базу");
                _log.Error("UNKNOWN | ProductionPlanControlViewModel::UndoCommand | " + ex.GetType().Name + " | " + ex.Message);
            }
        }

        #endregion
        #region RedoCommand

        public ICommand RedoCommand { get; }

        private bool CanRedoCommandExecute(object p) => _undoManager.RedoStackEmpty();

        private void OnRedoCommandExecuted(object p)
        {
            try
            {
                _undoManager.ExecuteRedoCommand();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка. Обновите базу");
                _log.Error("UNKNOWN | ProductionPlanControlViewModel::RedoCommand | " + ex.GetType().Name + " | " + ex.Message);
            } 
        }

        #endregion
        #region SetBoldCommand

        public ICommand SetBoldCommand { get; }

        private bool CanSetBoldCommandExecute(object p) => SelectedTask!=null;

        private void OnSetBoldCommandExecuted(object p)
        {
            ProductionTaskDB task = Unit.Tasks.Get().Where(t=>t.Id == SelectedTask.Task.Id).FirstOrDefault();
            if (task == null)
            {
                MessageBox.Show("Выбранная строка была удалена");
            }
            else
            {
                task.Formatting.IsBold = task.Formatting.IsBold == true ? false : true;
            }
            Unit.Commit();
            Update();
            SelectedTask = ProductionTask.FindByTask(Model, task);
        }

        #endregion
        #region SetItalicCommand

        public ICommand SetItalicCommand { get; }

        private bool CanSetItalicCommandExecute(object p) => SelectedTask != null;

        private void OnSetItalicCommandExecuted(object p)
        {
            ProductionTaskDB task = Unit.Tasks.Get().Where(t => t.Id == SelectedTask.Task.Id).FirstOrDefault();
            if (task == null)
            {
                MessageBox.Show("Выбранная строка была удалена");
            }
            else
            {
                task.Formatting.IsItalic = task.Formatting.IsItalic == true ? false : true;
            }
            Unit.Commit();
            Update();
            SelectedTask = ProductionTask.FindByTask(Model, task);
        }

        #endregion
        #region SetUnderlineCommand

        public ICommand SetUnderlineCommand { get; }

        private bool CanSetUnderlineCommandExecute(object p) => SelectedTask != null;

        private void OnSetUnderlineCommandExecuted(object p)
        {
            ProductionTaskDB task = Unit.Tasks.Get().Where(t => t.Id == SelectedTask.Task.Id).FirstOrDefault();
            if (task == null)
            {
                MessageBox.Show("Выбранная строка была удалена");
            }
            else
            {
                task.Formatting.IsUnderline = task.Formatting.IsUnderline == true ? false : true;
            }
            Unit.Commit();
            Update();
            SelectedTask = ProductionTask.FindByTask(Model, task);
        }

        #endregion
        #region OpenPaymentWindowCommand

        public ICommand OpenPaymentWindowCommand { get; }

        private bool CanOpenPaymentWindowCommandExecute(object p) => true;

        private void OnOpenPaymentWindowCommandExecuted(object p)
        {
            DataWindowViewModel vm = new DataWindowViewModel();
            vm.SetEditableTask(SelectedTask.Task);
            PaymentWindow window = new PaymentWindow();
            window.DataContext = vm;
            window.Closed += Window_Closed;
            window.Show();
        }

        #endregion
        #region OpenLaborCostsWindowCommand

        public ICommand OpenLaborCostsWindowCommand { get; }

        private bool CanOpenLaborCostsWindowCommandExecute(object p) => true;

        private void OnOpenLaborCostsWindowCommandExecuted(object p)
        {
            DataWindowViewModel vm = new DataWindowViewModel();
            vm.SetEditableTask(SelectedTask.Task);
            LaborCostsWindow window = new LaborCostsWindow();
            window.DataContext = vm;
            window.Closed += Window_Closed;
            window.Show();
        }

        #endregion
        #region OpenDocumentWindowCommand

        public ICommand OpenDocumentWindowCommand { get; }

        private bool CanOpenDocumentWindowCommandExecute(object p) => true;

        private void OnOpenDocumentWindowCommandExecuted(object p)
        {
            DataWindowViewModel vm = new DataWindowViewModel();
            vm.SetEditableTask(SelectedTask.Task);
            DocumentWindow window = new DocumentWindow();
            window.DataContext = vm;
            window.Closed += Window_Closed;
            window.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //ProductionTaskDB task = Unit.Tasks.Get().Where(t => t.Id == SelectedTask.Task.Id).SingleOrDefault();
            //Update();
            //SelectedTask = ProductionTask.FindByTask(Model,task);
        }

        #endregion
        #region SelectionChangedCommand

        public ICommand SelectionChangedCommand { get; }

        private bool CanSelectionChangedCommandExecute(object p) => SelectedTask!=null;

        private void OnSelectionChangedCommandExecuted(object p)
        {
            MessageBoxResult result = MessageBox.Show("Вы хотите изменить статус?", "Изменение статуса", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    BindingExpression be = ((ComboBox)p).GetBindingExpression(ComboBox.SelectedIndexProperty);
                    be.UpdateSource();
                    switch (SelectedTask.Task.Completion)
                    {
                        case (short)TaskCompletion.VKOnStorage:
                            SelectedTask.Task.Complectation.ComplectationDate = DateTime.Now;
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion
        #region OpenComplectationWindowCommand

        public ICommand OpenComplectationWindowCommand { get; }

        private bool CanOpenComplectationWindowCommandExecute(object p) => true;

        private void OnOpenComplectationWindowCommandExecuted(object p)
        {
            DataWindowViewModel vm = new DataWindowViewModel();
            vm.SetEditableTask(SelectedTask.Task);
            ComplectationWindow window = new ComplectationWindow();
            window.DataContext = vm;
            window.Closed += Window_Closed;
            window.Show();
        }

        #endregion
        #region OpenActWindowCommand

        public ICommand OpenActWindowCommand { get; }

        private bool CanOpenActWindowCommandExecute(object p) => true;

        private void OnOpenActWindowCommandExecuted(object p)
        {
            DataWindowViewModel actVm = new DataWindowViewModel();
            actVm.SetEditableTask(SelectedTask.Task);
            ConsumeActWindow window = new ConsumeActWindow();
            window.Closed += Window_Closed;
            window.DataContext = actVm;
            window.Show();
        }

        #endregion
        #region OpenGivingWindowCommand

        public ICommand OpenGivingWindowCommand { get; }

        private bool CanOpenGivingWindowCommandExecute(object p) => true;

        private void OnOpenGivingWindowCommandExecuted(object p)
        {
            DataWindowViewModel vm = new DataWindowViewModel();
            vm.SetEditableTask(SelectedTask.Task);
            GivingWindow window = new GivingWindow();
            window.DataContext = vm;
            window.Closed += Window_Closed;
            window.Show();
        }

        #endregion
        #region OpenManufactureWindowCommand

        public ICommand OpenManufactureWindowCommand { get; }

        private bool CanOpenManufactureWindowCommandExecute(object p) => true;

        private void OnOpenManufactureWindowCommandExecuted(object p)
        {
            DataWindowViewModel vm = new DataWindowViewModel();
            vm.SetEditableTask(SelectedTask.Task);
            ManufactureWindow window = new ManufactureWindow();
            window.DataContext = vm;
            window.Closed += Window_Closed;
            window.Show();
        }

        #endregion
        #region OpenInProductionWindowCommand

        public ICommand OpenInProductionWindowCommand { get; }

        private bool CanOpenInProductionWindowCommandExecute(object p) => true;

        private void OnOpenInProductionWindowCommandExecuted(object p)
        {
            DataWindowViewModel vm = new DataWindowViewModel();
            vm.SetEditableTask(SelectedTask.Task);
            InProductionWindow window = new InProductionWindow();
            window.DataContext = vm;
            window.Closed += Window_Closed;
            window.Show();
        }

        #endregion
        #region OpenSearchWindowCommand

        public ICommand OpenSearchWindowCommand { get; }

        private bool CanOpenSearchWindowCommandExecute(object p) => true;

        private void OnOpenSearchWindowCommandExecuted(object p)
        {
            SearchWindow window = new SearchWindow();
            ((SearchResultsVM)(window.DataContext)).SendTaskIdToReportVM += SetSelectedTaskFromReport;
            window.Show();
        }

        #endregion
        #region OnCollapsingCommand

        public ICommand OnCollapsingCommand { get; }

        private bool CanOnCollapsingCommandExecute(object p) => true;

        private void OnOnCollapsingCommandExecuted(object p)
        {
            // TODO: Добавить сохранение сворачивания в БД
            if (p is ProductionTask)
            {
                ((ProductionTask)p).UnloadChildren();
            }
        }

        #endregion
        #region OnExpandingCommand

        public ICommand OnExpandingCommand { get; }

        private bool CanOnExpandingCommandExecute(object p) => true;

        private void OnOnExpandingCommandExecuted(object p)
        {
            // TODO: Добавить сохранение разворачивания в БД
            if (p is ProductionTask)
            {
                ((ProductionTask)p).LoadChildren();
            }
        }

        #endregion
        #region GetDatagrid

        public ICommand GetDatagrid { get; }

        private bool CanGetDatagridExecute(object p) => true;

        private void OnGetDatagridExecuted(object p)
        {
            DataTable = (DataGrid)p;
        }

        #endregion
        #endregion

        #region Методы

        private void Update()
        {
            try
            {
                ApplicationUnitSingleton.RecreateUnit();
                Unit = ApplicationUnitSingleton.GetInstance().dbUnit;
                ProductionTasks = Unit.Tasks.Get().ToList();
                Model = ProductionTask.InitModel(ProductionTasks);
                _undoManager = new UndoRedoManager();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка. Обновите базу");
                _log.Error("UNKNOWN | ProductionPlanControlViewModel::UpdateModelCommand | " + ex.GetType().Name + " | " + ex.Message);
            }
        }

        #endregion

        public void SetSelectedTaskFromReport(object sender, TaskIdEventArgs e)
        {
            SelectedTask = ProductionTask.FindByTask(Model, ApplicationUnitSingleton.GetInstance().dbUnit.Tasks.GetByID(e.Id));
            SelectedTask.ExpandFromChildToParent();
            DataTable.UpdateLayout();
            DataTable.ScrollIntoView(SelectedTask);
        }

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
            CutTaskCommand = new LambdaCommand(OnCutTaskCommandExecuted, CanCutTaskCommandExecute);
            PasteTaskCommand = new LambdaCommand(OnPasteTaskCommandExecuted, CanPasteTaskCommandExecute);
            SearchCommand = new LambdaCommand(OnSearchCommandExecuted, CanSearchCommandExecute);
            MoveNextResultCommand = new LambdaCommand(OnMoveNextResultCommandExecuted, CanMoveNextResultCommandExecute);
            MovePreviousResultCommand = new LambdaCommand(OnMovePreviousResultCommandExecuted, CanMovePreviousResultCommandExecute);
            OnCollapsingCommand = new LambdaCommand(OnOnCollapsingCommandExecuted, CanOnCollapsingCommandExecute);
            OnExpandingCommand = new LambdaCommand(OnOnExpandingCommandExecuted, CanOnExpandingCommandExecute);
            UpTaskCommand = new LambdaCommand(OnUpTaskCommandExecuted, CanUpTaskCommandExecute);
            DownTaskCommand = new LambdaCommand(OnDownTaskCommandExecuted, CanDownTaskCommandExecute);
            UndoCommand = new LambdaCommand(OnUndoCommandExecuted, CanUndoCommandExecute);
            RedoCommand = new LambdaCommand(OnRedoCommandExecuted, CanRedoCommandExecute);
            SetBoldCommand = new LambdaCommand(OnSetBoldCommandExecuted, CanSetBoldCommandExecute);
            SetItalicCommand = new LambdaCommand(OnSetItalicCommandExecuted, CanSetItalicCommandExecute);
            SetUnderlineCommand = new LambdaCommand(OnSetUnderlineCommandExecuted, CanSetUnderlineCommandExecute);
            OpenPaymentWindowCommand = new LambdaCommand(OnOpenPaymentWindowCommandExecuted, CanOpenPaymentWindowCommandExecute);
            OpenLaborCostsWindowCommand = new LambdaCommand(OnOpenLaborCostsWindowCommandExecuted, CanOpenLaborCostsWindowCommandExecute);
            OpenDocumentWindowCommand = new LambdaCommand(OnOpenDocumentWindowCommandExecuted, CanOpenDocumentWindowCommandExecute);
            OpenComplectationWindowCommand = new LambdaCommand(OnOpenComplectationWindowCommandExecuted, CanOpenComplectationWindowCommandExecute);
            OpenGivingWindowCommand = new LambdaCommand(OnOpenGivingWindowCommandExecuted, CanOpenGivingWindowCommandExecute);
            OpenManufactureWindowCommand = new LambdaCommand(OnOpenManufactureWindowCommandExecuted, CanOpenManufactureWindowCommandExecute);
            OpenInProductionWindowCommand = new LambdaCommand(OnOpenInProductionWindowCommandExecuted, CanOpenInProductionWindowCommandExecute);
            SelectionChangedCommand = new LambdaCommand(OnSelectionChangedCommandExecuted, CanSelectionChangedCommandExecute);
            GetDatagrid = new LambdaCommand(OnGetDatagridExecuted, CanGetDatagridExecute);
            OpenActWindowCommand = new LambdaCommand(OnOpenActWindowCommandExecuted, CanOpenActWindowCommandExecute);
            OpenSearchWindowCommand = new LambdaCommand(OnOpenSearchWindowCommandExecuted, CanOpenSearchWindowCommandExecute);
            #endregion

            FontSizes = new List<int> { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22 };
            FontFamilies = new List<string> { "Arial", "Calibre", "Times New Roman" };
            User = UserDataSingleton.GetInstance().user;
            //Unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            //ProductionTasks = Unit.Tasks.Get().ToList();
            //Model = ProductionTask.InitModel(ProductionTasks);
            Model = ProductionTask.InitRootsModel();
            searchManager = new SearchManager();
            //_undoManager = new UndoRedoManager();

            var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            string filepath = Directory.GetCurrentDirectory() + "\\Data\\Configs\\log4net.config";
            XmlConfigurator.Configure(logRepo, new FileInfo(filepath));
        }
    }
}

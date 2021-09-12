using CP_2021.Data;
using CP_2021.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Collections.ObjectModel;
using CP_2021.Infrastructure.UndoRedo;
using CP_2021.Infrastructure.UndoRedo.UndoCommands;
using System.Threading;
using CP_2021.Infrastructure.Threading;
using log4net;
using log4net.Config;
using System.Reflection;
using CP_2021.ViewModels.DataWindowViewModels;
using CP_2021.Views.Windows.DataWindows;
using System.Windows.Data;
using CP_2021.Infrastructure.Utils;
using CP_2021.Models.Hierarchy;
using System.ComponentModel;

namespace CP_2021.ViewModels
{
    class ProductionPlanControlViewModel : ViewModelBase
    {

        #region Свойства

        private ApplicationUnit Unit;
        private UndoRedoManager _undoManager;
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static BackgroundWorker bgWorker;

        #region StatusMessage

        private string _statusMessage;

        public string StatusMessage
        {
            get => _statusMessage;
            set => Set(ref _statusMessage, value);
        }

        #endregion
        #region Data

        private DataGridHierarchialData _data;

        public DataGridHierarchialData Data
        {
            get => _data;
            set => Set(ref _data, value);
        }

        #endregion

        #region SelectedModel

        private DataGridHierarchialDataModel _selectedModel;

        public DataGridHierarchialDataModel SelectedModel
        {
            get => _selectedModel;
            set => Set(ref _selectedModel, value);
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

        #region ModelToCopy

        private DataGridHierarchialDataModel _modelToCopy;

        public DataGridHierarchialDataModel ModelToCopy
        {
            get => _modelToCopy;
            set => Set(ref _modelToCopy, value);
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

        private bool CanExpandAllCommandExecute(object p) => Data!=null;

        private void OnExpandAllCommandExecuted(object p)
        {
            try
            {
                ApplicationUnitSingleton.RecreateUnit();
                ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
                foreach (ProductionTaskDB task in unit.Tasks.Get()){
                    task.Expanded = true;
                }
                unit.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка. Обновите базу");
                _log.Error("UNKNOWN | ProductionPlanControlViewModel::ExpandAllCommand | " + ex.GetType().Name + " | " + ex.Message);
            }
            InitData();
        }

        #endregion

        #region RollUpAllCommand

        public ICommand RollUpAllCommand { get; }

        private bool CanRollUpAllCommandExecute(object p) => Data != null;

        private void OnRollUpAllCommandExecuted(object p)
        {
            try
            {
                ApplicationUnitSingleton.RecreateUnit();
                ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
                foreach (ProductionTaskDB task in unit.Tasks.Get())
                {
                    task.Expanded = false;
                }
                unit.Commit();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка. Обновите базу");
                _log.Error("UNKNOWN | ProductionPlanControlViewModel::RollUpAllCommand | " + ex.GetType().Name + " | " + ex.Message);
            }
            InitData();
        }

        #endregion

        #region AddProductionTaskCommand

        public ICommand AddProductionTaskCommand { get; }

        private bool CanAddProductionTaskCommandExecute(object p) =>true;

        private void OnAddProductionTaskCommandExecuted(object p)
        {
            ProductionTaskDB dbTask = new ProductionTaskDB("Новое изделие");
            ApplicationUnitSingleton.RecreateUnit();
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            //Если база данных пустая
            try
            {
                if (unit.Tasks.Get().Count() == 0)
                {
                    dbTask.MyParent = new HierarchyDB(dbTask);
                    dbTask.MyParent.LineOrder = 1;
                }
                else
                {
                    //Команда не выполняется без выбранной задачи
                    if (SelectedModel == null)
                    {
                        MessageBox.Show("Не выбран элемент для добавления");
                        return;
                    }
                    //Добавление головного изделия, иначе изделие в Children к Parent
                    if (((ProductionTaskDB)SelectedModel.Data).MyParent.Parent != null)
                    {
                        ProductionTaskDB parent = unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).MyParent.ParentId).Single();
                        parent.Expanded = true;
                        dbTask.MyParent = new HierarchyDB(parent, dbTask);
                        dbTask.MyParent.LineOrder = unit.Tasks.Get().Where(t => t.MyParent.ParentId == parent.Id).Max(t => t.MyParent.LineOrder) + 1;
                    }
                    else
                    {
                        dbTask.MyParent = new HierarchyDB(dbTask);
                        //Выборка элемента с Id выбранного элемента
                        ProductionTaskDB upperTask = unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).Single();
                        dbTask.MyParent.LineOrder = upperTask.MyParent.LineOrder + 1;
                        dbTask.DownTaskBelow();
                    }
                }
                unit.Tasks.Insert(dbTask);
                unit.Commit();
            }
            catch(InvalidOperationException ex)
            {
                MessageBox.Show("Выбранный вами элемент был удален");
                _log.Warn("ProductionPlanControlViewModel::AddProductionTaskCommand " + ex.ToString());
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("ProductionPlanControlViewModel::AddProductionTaskCommand " + ex.ToString());
            }
            InitData();
        }

        #endregion

        #region RowEditingEndingCommand

        public ICommand RowEditingEndingCommand { get; }

        private bool CanRowEditingEndingCommandExecute(object p) => true;

        private void OnRowEditingEndingCommandExecuted(object p)
        {
            try
            {
                ApplicationUnitSingleton.GetInstance().dbUnit.Tasks.Update((ProductionTaskDB)(SelectedModel.Data));
                ApplicationUnitSingleton.GetInstance().dbUnit.Commit();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                MessageBox.Show("Редактируемая строка была удалена. Обновите базу");
                _log.Warn("ProductionPlanControlViewModel::RowEditingEndingCommand | " + ex.ToString());
            }catch(Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка. Обновите базу");
                _log.Error("UNKNOWN | ProductionPlanControlViewModel::RowEditingEndingCommand | " + ex.ToString());
            }
        }

        #endregion

        #region AddChildCommand

        public ICommand AddChildCommand { get; }

        private bool CanAddChildCommandExecute(object p) => SelectedModel != null;

        private void OnAddChildCommandExecuted(object p)
        {
            ProductionTaskDB dbTask = new ProductionTaskDB("Новое изделие");
            try
            {
                ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
                ProductionTaskDB parent = unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).Single();
                parent.Expanded = true;
                dbTask.MyParent = new HierarchyDB(parent, dbTask);

                if (unit.Tasks.Get().Where(t => t.MyParent.ParentId == parent.Id).Count() != 0)
                {
                    dbTask.MyParent.LineOrder = unit.Tasks.Get().Where(t => t.MyParent.ParentId == parent.Id).Max(t => t.MyParent.LineOrder) + 1;
                }
                else
                {
                    dbTask.MyParent.LineOrder = 1;
                }

                unit.Tasks.Insert(dbTask);
                unit.Commit();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("ProductionPlanControlViewModel::OnAddChildCommandExecuted " + ex.Message);
            }
            InitData();
        }

        #endregion

        #region DeleteProductionTaskCommand

        public ICommand DeleteProductionTaskCommand { get; }

        private bool CanDeleteProductionTaskCommandExecute(object p) => SelectedModel != null;

        private void OnDeleteProductionTaskCommandExecuted(object p)
        {
            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить элемент {((ProductionTaskDB)SelectedModel.Data).Name}?", "Удаление", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    ApplicationUnitSingleton.RecreateUnit();
                    ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
                    ProductionTaskDB dbTask = unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).SingleOrDefault();
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
            InitData();
        }

        #endregion

        #region LevelUpCommand

        public ICommand LevelUpCommand { get; }

        private bool CanLevelUpCommandExecute(object p) => SelectedModel != null && ((ProductionTaskDB)SelectedModel.Data).MyParent.Parent != null;

        private void OnLevelUpCommandExecuted(object p)
        {
            try
            {
                ApplicationUnitSingleton.RecreateUnit();
                ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
                ProductionTaskDB dbTask = unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).SingleOrDefault();
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
                        unit.Commit();
                        dbTask.DownTaskBelow();
                    }
                    else
                    {
                        int lineOrder = dbTask.MyParent.Parent.MyParent.LineOrder + 1;
                        dbTask.MyParent = new HierarchyDB(dbTask.MyParent.Parent.MyParent.Parent, dbTask);
                        dbTask.MyParent.LineOrder = lineOrder;
                        unit.Commit();
                        dbTask.DownTaskBelow();
                    }
                    unit.Commit();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("ProductionPlanControlViewModel::LevelUpCommand " + ex.Message);
            }
            InitData();
        }

        #endregion

        #region LevelDownCommand

        public ICommand LevelDownCommand { get; }

        private bool CanLevelDownCommandExecute(object p) => SelectedModel != null && ((ProductionTaskDB)SelectedModel.Data).MyParent.LineOrder != 1;

        private void OnLevelDownCommandExecuted(object p)
        {
            ApplicationUnitSingleton.RecreateUnit();
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            ProductionTaskDB dbTask = unit.Tasks.Get().Where(t=>t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).SingleOrDefault();
            try
            {
                if (dbTask == null)
                {
                    MessageBox.Show("Выделенная строка была удалена");
                }
                else
                {
                    dbTask.UpOrderBelow();
                    ProductionTaskDB parent = unit.Tasks.Get().Where(t => t.MyParent.ParentId == dbTask.MyParent.ParentId && t.MyParent.LineOrder == dbTask.MyParent.LineOrder - 1).Single();
                    parent.Expanded = true;
                    dbTask.MyParent = new HierarchyDB(parent, dbTask);
                    dbTask.MyParent.LineOrder = 1;
                    unit.Commit();
                    dbTask.DownTaskBelow();
                    unit.Commit();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("ProductionPlanControlViewModel::LevelDownCommand " + ex.Message);
            }
            InitData();
        }

        #endregion

        #region UpdateModelCommand

        public ICommand UpdateModelCommand { get; }

        private bool CanUpdateModelCommandExecute(object p) => true;

        private void OnUpdateModelCommandExecuted(object p)
        {
            ApplicationUnitSingleton.RecreateUnit();
            InitData();
        }

        #endregion

        #region CopyTaskCommand

        public ICommand CopyTaskCommand { get; }

        private bool CanCopyTaskCommandExecute(object p) => SelectedModel != null;

        private void OnCopyTaskCommandExecuted(object p)
        {
            try
            {
                ModelToCopy = SelectedModel;
                //TaskToCopy = ProductionTask.InitTask(((ProductionTaskDB)SelectedModel.Data));
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

        private bool CanCutTaskCommandExecute(object p) => SelectedModel!=null;

        private void OnCutTaskCommandExecuted(object p)
        {
            try
            {
                ModelToCopy = SelectedModel;
                ApplicationUnitSingleton.RecreateUnit();
                ProductionTaskDB dbTask = ApplicationUnitSingleton.GetInstance().dbUnit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).SingleOrDefault();
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
            InitData();
        }

        #endregion

        #region PasteTaskCommand

        public ICommand PasteTaskCommand { get; }

        private bool CanPasteTaskCommandExecute(object p) => ModelToCopy != null;

        private void OnPasteTaskCommandExecuted(object p)
        {
            ProductionTaskDB dbTask = ((ProductionTaskDB)ModelToCopy.Data).Clone();
            try
            {
                ApplicationUnitSingleton.RecreateUnit();
                ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
                ((ProductionTaskDB)SelectedModel.Data).DownTaskBelow();

                if (((ProductionTaskDB)SelectedModel.Data).MyParent.Parent == null)
                {
                    dbTask.MyParent = new HierarchyDB(dbTask);
                }
                else
                {
                    dbTask.MyParent = new HierarchyDB(((ProductionTaskDB)SelectedModel.Data).MyParent.Parent, dbTask);
                }
                dbTask.MyParent.LineOrder = ((ProductionTaskDB)SelectedModel.Data).MyParent.LineOrder;

                unit.Tasks.Insert(dbTask);
                unit.Commit();

                if (ModelToCopy.HasChildren)
                {
                    foreach (DataGridHierarchialDataModel child in ModelToCopy.Children)
                    {
                        dbTask.AddChildTask(child);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка. Обновите базу");
                _log.Error("UNKNOWN | ProductionPlanControlViewModel::PasteTaskCommand | " + ex.ToString());
            }
            InitData();
        }

        #endregion

        #region SearchCommands

        #region SearchCommand

        public ICommand SearchCommand { get; }

        private bool CanSearchCommandExecute(object p) => SearchString != null;

        private void OnSearchCommandExecuted(object p)
        {
            //Update();
            //SearchResultString = null;
            //SearchResults = new List<ProductionTask>();
            //FieldNames fieldName = (FieldNames)SelectedSearchIndex;
            //try
            //{
            //    switch (fieldName)
            //    {
            //        case FieldNames.Name:
            //            searchManager.SetSearchStrategy(new NameSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.ManagDoc:
            //            searchManager.SetSearchStrategy(new ManagDocSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.Count:
            //            searchManager.SetSearchStrategy(new CountSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.SpecificationCost:
            //            searchManager.SetSearchStrategy(new SpecificationCostSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.IncDoc:
            //            searchManager.SetSearchStrategy(new IncDocSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.VishDate:
            //            searchManager.SetSearchStrategy(new VishDateSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.RealDate:
            //            searchManager.SetSearchStrategy(new RealDateSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.Complectation:
            //            searchManager.SetSearchStrategy(new ComplectationSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.ComplectationDate:
            //            searchManager.SetSearchStrategy(new ComplectationDateSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.Percent:
            //            searchManager.SetSearchStrategy(new PercentSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.MSLNumber:
            //            searchManager.SetSearchStrategy(new MSLNumberSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.Executor:
            //            searchManager.SetSearchStrategy(new FirstExecutorSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.Executor2:
            //            searchManager.SetSearchStrategy(new SecondExecutorSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.GivingDate:
            //            searchManager.SetSearchStrategy(new GivingDateSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.ProjectedDate:
            //            searchManager.SetSearchStrategy(new ProjectedDateSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.ReadyDate:
            //            searchManager.SetSearchStrategy(new CompletionDateSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.Manufacture:
            //            searchManager.SetSearchStrategy(new ManufactureSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.LetterNum:
            //            searchManager.SetSearchStrategy(new LetterNumSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.SpecNum:
            //            searchManager.SetSearchStrategy(new SpecNumSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.Bill:
            //            searchManager.SetSearchStrategy(new BillSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.Report:
            //            searchManager.SetSearchStrategy(new ReportSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.ReturnReport:
            //            searchManager.SetSearchStrategy(new ReturnReportSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.ReceivingDate:
            //            searchManager.SetSearchStrategy(new ReceivingDateSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.ExpendNum:
            //            searchManager.SetSearchStrategy(new ExpendNumSearchStrategy(Model, SearchString));
            //            break;
            //        case FieldNames.Note:
            //            searchManager.SetSearchStrategy(new NoteSearchStrategy(Model, SearchString));
            //            break;
            //        default:
            //            SearchResultString = "Параметр для поиска не задан";
            //            break;

            //    }
            //    searchManager.ExecuteSearchStrategy();
            //    SearchResults = searchManager.GetSearchResults();
            //    if(SearchResults.Count != 0) 
            //    {
            //        MessageBox.Show("Поиск завершен. Количество совпадений: " + SearchResults.Count);
            //    }
            //    else
            //    {
            //        MessageBox.Show("Поиск завершен. Совпадений не найдено");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (ex is IncorrectDateFormatException or IncorrectSearchValueException)
            //    {
            //        SearchResultString = ex.Message;
            //        MessageBox.Show(ex.Message);
            //    }
            //}

            //if (SearchResultString == null)
            //{
            //    if (SearchResults.Count != 0)
            //    {
            //        SearchResultString = $"Количество совпадений: {SearchResults.Count}";
            //        SelectedTask = SearchResults.First();
            //        ((DataGrid)p).UpdateLayout();
            //        ((DataGrid)p).ScrollIntoView(SelectedTask);
            //    }
            //}
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

        private bool CanUpTaskCommandExecute(object p) => SelectedModel != null && ((ProductionTaskDB)SelectedModel.Data).MyParent.LineOrder != 1;

        private void OnUpTaskCommandExecuted(object p)
        {
            ApplicationUnitSingleton.RecreateUnit();
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            ProductionTaskDB taskToUp = unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).SingleOrDefault();
            try
            {
                ProductionTaskDB taskToDown = unit.Tasks.Get().Where(t => t.MyParent.ParentId == taskToUp.MyParent.ParentId && t.MyParent.LineOrder == taskToUp.MyParent.LineOrder - 1).SingleOrDefault();
                if (taskToUp == null)
                {
                    MessageBox.Show("Выбранная строка была удалена");
                }
                else
                {
                    taskToUp.MyParent.LineOrder--;
                    taskToDown.MyParent.LineOrder++;
                    unit.Commit();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("ProductionPlanControlViewModel::UpTaskCommand " + ex.Message);
            }
            InitData();
        }

        #endregion

        #region DownTaskCommand

        public ICommand DownTaskCommand { get; }

        private bool CanDownTaskCommandExecute(object p)
        {
            if (SelectedModel != null)
            {
                
                int selecetedTaskLineOrder = ((ProductionTaskDB)SelectedModel.Data).MyParent.LineOrder;
                int maxLineOrderByParent = ApplicationUnitSingleton.GetInstance().dbUnit.Tasks.Get().Where(t => t.MyParent.ParentId == ((ProductionTaskDB)SelectedModel.Data).MyParent.ParentId).Max(t => t.MyParent.LineOrder);
                return selecetedTaskLineOrder != maxLineOrderByParent;
            }
            else
            {
                return false;
            }
        }

        private void OnDownTaskCommandExecuted(object p)
        {
            ApplicationUnitSingleton.RecreateUnit();
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            ProductionTaskDB taskToDown = unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).SingleOrDefault();
            try
            {
                ProductionTaskDB taskToUp = unit.Tasks.Get().Where(t => t.MyParent.ParentId == taskToDown.MyParent.ParentId && t.MyParent.LineOrder == taskToDown.MyParent.LineOrder + 1).SingleOrDefault();
                if (taskToDown == null)
                {
                    MessageBox.Show("Выбранная строка была удалена");
                }
                else
                {
                    taskToUp.MyParent.LineOrder--;
                    taskToDown.MyParent.LineOrder++;
                    unit.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("ProductionPlanControlViewModel::DownTaskCommand " + ex.Message);
            }
            InitData();
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

        private bool CanSetBoldCommandExecute(object p) => SelectedModel!=null;

        private void OnSetBoldCommandExecuted(object p)
        {
            ApplicationUnitSingleton.RecreateUnit();
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            ProductionTaskDB task = unit.Tasks.Get().Where(t=>t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).FirstOrDefault();
            if (task == null)
            {
                MessageBox.Show("Выбранная строка была удалена");
            }
            else
            {
                task.Formatting.IsBold = task.Formatting.IsBold == true ? false : true;
            }
            unit.Commit();
            InitData();
        }

        #endregion

        #region SetItalicCommand

        public ICommand SetItalicCommand { get; }

        private bool CanSetItalicCommandExecute(object p) => SelectedModel != null;

        private void OnSetItalicCommandExecuted(object p)
        {
            ApplicationUnitSingleton.RecreateUnit();
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            ProductionTaskDB task = unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).FirstOrDefault();
            if (task == null)
            {
                MessageBox.Show("Выбранная строка была удалена");
            }
            else
            {
                task.Formatting.IsItalic = task.Formatting.IsItalic == true ? false : true;
            }
            unit.Commit();
            InitData();
        }

        #endregion

        #region SetUnderlineCommand

        public ICommand SetUnderlineCommand { get; }

        private bool CanSetUnderlineCommandExecute(object p) => SelectedModel != null;

        private void OnSetUnderlineCommandExecuted(object p)
        {
            ApplicationUnitSingleton.RecreateUnit();
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            ProductionTaskDB task = unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).FirstOrDefault();
            if (task == null)
            {
                MessageBox.Show("Выбранная строка была удалена");
            }
            else
            {
                task.Formatting.IsUnderline = task.Formatting.IsUnderline == true ? false : true;
            }
            unit.Commit();
            InitData();
        }

        #endregion

        #region OpenPaymentWindowCommand

        public ICommand OpenPaymentWindowCommand { get; }

        private bool CanOpenPaymentWindowCommandExecute(object p) => true;

        private void OnOpenPaymentWindowCommandExecuted(object p)
        {
            try
            {
                //Unit.Refresh();
                using(ApplicationContext context = new ApplicationContext())
                {
                    ProductionTaskDB task = context.Production_Plan.Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).SingleOrDefault();
                    //ProductionTaskDB task = Unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).SingleOrDefault();
                    if (task == null)
                    {
                        MessageBox.Show("Изделие было удалено");
                        Update();
                    }
                    if (task.EditingBy == "default")
                    {
                        task.EditingBy = UserDataSingleton.GetInstance().user.Login;
                        context.SaveChanges();
                        DataWindowViewModel paymentVM = new DataWindowViewModel();
                        paymentVM.SetEditableTask(((ProductionTaskDB)SelectedModel.Data));
                        PaymentWindow paymentWindow = new PaymentWindow();
                        paymentWindow.Closed += Window_Closed;
                        paymentWindow.DataContext = paymentVM;
                        paymentWindow.Show();
                    }
                    else
                    {
                        UserDB user = Unit.DBUsers.Get().Where(u => u.Login == task.EditingBy).SingleOrDefault();
                        if (user == null)
                        {
                            MessageBox.Show($"Строка уже редактируется");
                        }
                        else
                        {
                            MessageBox.Show($"Строка уже редактируется пользователем {user.Surname} {user.Name}");
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("OpenPaymentWindowCommand " + ex.Message);
            }
        }

        #endregion

        #region OpenLaborCostsWindowCommand

        public ICommand OpenLaborCostsWindowCommand { get; }

        private bool CanOpenLaborCostsWindowCommandExecute(object p) => true;

        private void OnOpenLaborCostsWindowCommandExecuted(object p)
        {
            try
            {
                Unit.Refresh();
                ProductionTaskDB task = Unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).SingleOrDefault();
                if (task == null)
                {
                    MessageBox.Show("Изделие было удалено");
                    Update();
                }
                if (task.EditingBy == "default")
                {
                    task.EditingBy = UserDataSingleton.GetInstance().user.Login;
                    Unit.Commit();
                    DataWindowViewModel laborVm = new DataWindowViewModel();
                    laborVm.SetEditableTask(((ProductionTaskDB)SelectedModel.Data));
                    LaborCostsWindow laborWindow = new LaborCostsWindow();
                    laborWindow.Closed += Window_Closed;
                    laborWindow.DataContext = laborVm;
                    laborWindow.Show();
                }
                else
                {
                    UserDB user = Unit.DBUsers.Get().Where(u => u.Login == task.EditingBy).SingleOrDefault();
                    if (user == null)
                    {
                        MessageBox.Show($"Строка уже редактируется");
                    }
                    else
                    {
                        MessageBox.Show($"Строка уже редактируется пользователем {user.Surname} {user.Name}");
                    }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("OpenLaborCostsWindowCommand " + ex.Message);
            }
        }

        #endregion

        #region OpenDocumentWindowCommand

        public ICommand OpenDocumentWindowCommand { get; }

        private bool CanOpenDocumentWindowCommandExecute(object p) => true;

        private void OnOpenDocumentWindowCommandExecuted(object p)
        {
            try
            {
                Unit.Refresh();
                ProductionTaskDB task = Unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).SingleOrDefault();
                if (task == null)
                {
                    MessageBox.Show("Изделие было удалено");
                    Update();
                }
                if (task.EditingBy == "default")
                {
                    task.EditingBy = UserDataSingleton.GetInstance().user.Login;
                    Unit.Commit();
                    DataWindowViewModel laborVm = new DataWindowViewModel();
                    laborVm.SetEditableTask(((ProductionTaskDB)SelectedModel.Data));
                    DocumentWindow window = new DocumentWindow();
                    window.Closed += Window_Closed;
                    window.DataContext = laborVm;
                    window.Show();
                }
                else
                {
                    UserDB user = Unit.DBUsers.Get().Where(u => u.Login == task.EditingBy).SingleOrDefault();
                    if (user == null)
                    {
                        MessageBox.Show($"Строка уже редактируется");
                    }
                    else
                    {
                        MessageBox.Show($"Строка уже редактируется пользователем {user.Surname} {user.Name}");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("OpenDocumentWindowCommand " + ex.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //ProductionTaskDB task = Unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).SingleOrDefault();
            //Update();
            //SelectedTask = ProductionTask.FindByTask(Model,task);
        }

        #endregion

        #region SelectionChangedCommand

        public ICommand SelectionChangedCommand { get; }

        private bool CanSelectionChangedCommandExecute(object p) => SelectedModel!=null;

        private void OnSelectionChangedCommandExecuted(object p)
        {
            MessageBoxResult result = MessageBox.Show("Вы хотите изменить статус?", "Изменение статуса", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    BindingExpression be = ((ComboBox)p).GetBindingExpression(ComboBox.SelectedIndexProperty);
                    be.UpdateSource();
                    switch (((ProductionTaskDB)SelectedModel.Data).Completion)
                    {
                        case (short)TaskCompletion.VKOnStorage:
                            ((ProductionTaskDB)SelectedModel.Data).Complectation.ComplectationDate = DateTime.Now;
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
            try
            {
                Unit.Refresh();
                ProductionTaskDB task = Unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).SingleOrDefault();
                if (task == null)
                {
                    MessageBox.Show("Изделие было удалено");
                    Update();
                }
                if (task.EditingBy == "default")
                {
                    task.EditingBy = UserDataSingleton.GetInstance().user.Login;
                    Unit.Commit();
                    DataWindowViewModel laborVm = new DataWindowViewModel();
                    laborVm.SetEditableTask(((ProductionTaskDB)SelectedModel.Data));
                    ComplectationWindow window = new ComplectationWindow();
                    window.Closed += Window_Closed;
                    window.DataContext = laborVm;
                    window.Show();
                }
                else
                {
                    UserDB user = Unit.DBUsers.Get().Where(u => u.Login == task.EditingBy).SingleOrDefault();
                    if (user == null)
                    {
                        MessageBox.Show($"Строка уже редактируется");
                    }
                    else
                    {
                        MessageBox.Show($"Строка уже редактируется пользователем {user.Surname} {user.Name}");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("OpenComplectationWindowCommand " + ex.Message);
            }
        }

        #endregion

        #region OpenGivingWindowCommand

        public ICommand OpenGivingWindowCommand { get; }

        private bool CanOpenGivingWindowCommandExecute(object p) => true;

        private void OnOpenGivingWindowCommandExecuted(object p)
        {
            try
            {
                Unit.Refresh();
                ProductionTaskDB task = Unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).SingleOrDefault();
                if (task == null)
                {
                    MessageBox.Show("Изделие было удалено");
                    Update();
                }
                if (task.EditingBy == "default")
                {
                    task.EditingBy = UserDataSingleton.GetInstance().user.Login;
                    Unit.Commit();
                    DataWindowViewModel laborVm = new DataWindowViewModel();
                    laborVm.SetEditableTask(((ProductionTaskDB)SelectedModel.Data));
                    GivingWindow window = new GivingWindow();
                    window.Closed += Window_Closed;
                    window.DataContext = laborVm;
                    window.Show();
                }
                else
                {
                    UserDB user = Unit.DBUsers.Get().Where(u => u.Login == task.EditingBy).SingleOrDefault();
                    if (user == null)
                    {
                        MessageBox.Show($"Строка уже редактируется");
                    }
                    else
                    {
                        MessageBox.Show($"Строка уже редактируется пользователем {user.Surname} {user.Name}");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("OpenGivingWindowCommand " + ex.Message);
            }
        }

        #endregion

        #region OpenManufactureWindowCommand

        public ICommand OpenManufactureWindowCommand { get; }

        private bool CanOpenManufactureWindowCommandExecute(object p) => true;

        private void OnOpenManufactureWindowCommandExecuted(object p)
        {
            try
            {
                Unit.Refresh();
                ProductionTaskDB task = Unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).SingleOrDefault();
                if (task == null)
                {
                    MessageBox.Show("Изделие было удалено");
                    Update();
                }
                if (task.EditingBy == "default")
                {
                    task.EditingBy = UserDataSingleton.GetInstance().user.Login;
                    Unit.Commit();
                    DataWindowViewModel laborVm = new DataWindowViewModel();
                    laborVm.SetEditableTask(((ProductionTaskDB)SelectedModel.Data));
                    ManufactureWindow window = new ManufactureWindow();
                    window.Closed += Window_Closed;
                    window.DataContext = laborVm;
                    window.Show();
                }
                else
                {
                    UserDB user = Unit.DBUsers.Get().Where(u => u.Login == task.EditingBy).SingleOrDefault();
                    if (user == null)
                    {
                        MessageBox.Show($"Строка уже редактируется");
                    }
                    else
                    {
                        MessageBox.Show($"Строка уже редактируется пользователем {user.Surname} {user.Name}");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("OpenManufactureWindowCommand " + ex.Message);
            }
        }

        #endregion

        #region OpenInProductionWindowCommand

        public ICommand OpenInProductionWindowCommand { get; }

        private bool CanOpenInProductionWindowCommandExecute(object p) => true;

        private void OnOpenInProductionWindowCommandExecuted(object p)
        {
            try
            {
                Unit.Refresh();
                ProductionTaskDB task = Unit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)SelectedModel.Data).Id).SingleOrDefault();
                if (task == null)
                {
                    MessageBox.Show("Изделие было удалено");
                    Update();
                }
                if (task.EditingBy == "default")
                {
                    task.EditingBy = UserDataSingleton.GetInstance().user.Login;
                    Unit.Commit();
                    DataWindowViewModel laborVm = new DataWindowViewModel();
                    laborVm.SetEditableTask(((ProductionTaskDB)SelectedModel.Data));
                    InProductionWindow window = new InProductionWindow();
                    window.Closed += Window_Closed;
                    window.DataContext = laborVm;
                    window.Show();
                }
                else
                {
                    UserDB user = Unit.DBUsers.Get().Where(u => u.Login == task.EditingBy).SingleOrDefault();
                    if (user == null)
                    {
                        MessageBox.Show($"Строка уже редактируется");
                    }
                    else
                    {
                        MessageBox.Show($"Строка уже редактируется пользователем {user.Surname} {user.Name}");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка");
                _log.Error("OpenInProductionWindowCommand " + ex.Message);
            }
        }

        #endregion

        #region OnCollapsingCommand

        public ICommand OnCollapsingCommand { get; }

        private bool CanOnCollapsingCommandExecute(object p) => true;

        private void OnOnCollapsingCommandExecuted(object p)
        {
            try
            {
                if (p is DataGridHierarchialDataModel)
                {
                    ApplicationUnitSingleton.RecreateUnit();
                    ApplicationUnitSingleton.GetInstance().dbUnit.Tasks.Get().Where(t => t.Id == ((ProductionTaskDB)((DataGridHierarchialDataModel)p).Data).Id).First().Expanded = false;
                    ApplicationUnitSingleton.GetInstance().dbUnit.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка. Обновите базу");
                _log.Error("UNKNOWN | ProductionPlanControlViewModel::OnCollapsingCommand | " + ex.GetType().Name + " | " + ex.Message);
            }
        }

        #endregion
        #region OnExpandingCommand

        public ICommand OnExpandingCommand { get; }

        private bool CanOnExpandingCommandExecute(object p) => true;

        private void OnOnExpandingCommandExecuted(object p)
        {
            //try
            //{
            //    if (p is DataGridHierarchialDataModel)
            //    {
            //        ApplicationUnitSingleton.RecreateUnit();
            //        ApplicationUnitSingleton.GetInstance().dbUnit.Tasks.Get().Where(t=>t.Id == ((ProductionTaskDB)((DataGridHierarchialDataModel)p).Data).Id).First().Expanded = true;
            //        ApplicationUnitSingleton.GetInstance().dbUnit.Commit();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Неизвестная ошибка. Обновите базу");
            //    _log.Error("UNKNOWN | ProductionPlanControlViewModel::OnExpandingCommand | " + ex.GetType().Name + " | " + ex.Message);
            //}
        }

        #endregion

        #endregion

        #region Методы

        private void Update()
        {
        }
        private void InitData()
        {

            Data = new DataGridHierarchialData();
            AsyncDBUnit unit = AsyncUnitSingleton.GetInstance().dbUnit;
            
            var tasks = unit.Tasks.Items.ToList();
            
            Debug.WriteLine("Before load: " + DateTime.Now.ToLongTimeString());
            Debug.WriteLine(DateTime.Now.ToLongTimeString() + " Data loop start");
            foreach (ProductionTaskDB root in tasks.Where(t => t.MyParent.Parent == null).OrderBy(t=>t.MyParent.LineOrder))
            {
                DataGridHierarchialDataModel rootModel = new DataGridHierarchialDataModel() { Data = root, DataManager = Data };
                rootModel.IsExpanded = root.Expanded;
                //Thread thread = new Thread(new ThreadStart(rootModel.AddChildren));
                //thread.Start();
                Debug.WriteLine(DateTime.Now.ToLongTimeString() + " New thread started");
                rootModel.AddChildren();
                rootModel.IsVisible = true;
                Data.RawData.Add(rootModel);
            }
            Debug.WriteLine(DateTime.Now.ToLongTimeString() + " Data loop end");
            Data.Initialize();
            Debug.WriteLine(DateTime.Now.ToLongTimeString() + " Data initialized");
        }

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
            #endregion

            FontSizes = new List<int> { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22 };
            FontFamilies = new List<string> { "Arial", "Calibre", "Times New Roman" };
            User = UserDataSingleton.GetInstance().user;
            searchManager = new SearchManager();
            _undoManager = new UndoRedoManager();

            var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            string filepath = Directory.GetCurrentDirectory() + "\\Data\\Configs\\log4net.config";
            XmlConfigurator.Configure(logRepo, new FileInfo(filepath));
            bgWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            bgWorker.DoWork += LoadData;
            bgWorker.RunWorkerCompleted += InitializeData;
            bgWorker.ProgressChanged += ReportProgress;
            bgWorker.RunWorkerAsync();
        }

        private void ReportProgress(object sender, ProgressChangedEventArgs e)
        {
            StatusMessage = "Загрузка данных: " + e.ProgressPercentage.ToString() + "%";
        }

        private void InitializeData(object sender, RunWorkerCompletedEventArgs e)
        {
            Data.Initialize();
            StatusMessage = "Данные загружены";
        }

        private int activeThreadCount = 0;

        private void LoadData(object sender, DoWorkEventArgs e)
        {
            Data = new DataGridHierarchialData();
            AsyncDBUnit unit = AsyncUnitSingleton.GetInstance().dbUnit;

            var tasks = unit.Tasks._set.ToList().Where(t => t.MyParent.Parent == null).OrderBy(t => t.MyParent.LineOrder);

            Debug.WriteLine("Before load: " + DateTime.Now.ToLongTimeString());
            Debug.WriteLine(DateTime.Now.ToLongTimeString() + " Data loop start");
            int count = unit.Tasks.Items.Count();
            int currentItem = 1;
            foreach (ProductionTaskDB root in tasks)
            {
                DataGridHierarchialDataModel rootModel = new DataGridHierarchialDataModel() { Data = root, DataManager = Data };
                rootModel.IsExpanded = root.Expanded;
                activeThreadCount++;

                //ThreadPool.QueueUserWorkItem((object state)=> { rootModel.AddChildren(); });
                //Debug.WriteLine("New Thread");
                rootModel.AddChildren();
                rootModel.IsVisible = true;
                Data.RawData.Add(rootModel);
                bgWorker.ReportProgress((int)(((float)currentItem/count)*100));
                currentItem++;
            }
            Debug.WriteLine("Loop end. Active Thread count = " + activeThreadCount);
            //while (activeThreadCount != 0)
            //{
            //    Thread.Sleep(50);
            //    Debug.WriteLine(activeThreadCount);
            //}
        }
    }
}

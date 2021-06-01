using CP_2021.Data;
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

namespace CP_2021.ViewModels
{
    class ProductionPlanControlViewModel : ViewModelBase
    {

        #region Свойства

        #region UnitOfWork

        private ApplicationUnit _unit;

        public ApplicationUnit Unit
        {
            get => _unit;
            set => Set(ref _unit, value);
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
        // TODO: Сделать листом
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
        #region ErrorMessage

        private string _errorMessage;

        public string ErrorMessage
        {
            get => _errorMessage;
            set => Set(ref _errorMessage, value);
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

        #endregion

        #region Команды

        #region ExpandAllCommand

        public ICommand ExpandAllCommand { get; }

        private bool CanExpandAllCommandExecute(object p) => Model!=null;

        private void OnExpandAllCommandExecuted(object p)
        {
            foreach(ProductionTask t in Model)
            {
                Expand(t);
            }
        }

        #endregion

        #region RollUpAllCommand

        public ICommand RollUpAllCommand { get; }

        private bool CanRollUpAllCommandExecute(object p) => Model != null;

        private void OnRollUpAllCommandExecuted(object p)
        {
            foreach (ProductionTask t in Model)
            {
                RollUp(t);
            }
        }

        #endregion

        #region AddProductionTaskCommand

        public ICommand AddProductionTaskCommand { get; }

        private bool CanAddProductionTaskCommandExecute(object p) =>true;

        private void OnAddProductionTaskCommandExecuted(object p)
        {
            ProductionTaskDB dbTask = new ProductionTaskDB("Новое изделие");
            ProductionTask task = new ProductionTask(dbTask);
            if (SelectedTask?.Task.MyParent != null)
            {
                dbTask.MyParent = new HierarchyDB(SelectedTask.Task.MyParent.Parent, dbTask);
                SelectedTask.Parent.Children.Add(task);
            }
            else
            {
                Model.Add(task);
            }
            Unit.Tasks.Insert(dbTask);
            Unit.Commit();
            SelectedTask = task;
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

        #region AddChildCommand

        public ICommand AddChildCommand { get; }

        private bool CanAddChildCommandExecute(object p) => SelectedTask != null;

        private void OnAddChildCommandExecuted(object p)
        {
            ProductionTaskDB dbTask = new ProductionTaskDB("Новое изделие");
            dbTask.MyParent = new HierarchyDB(SelectedTask.Task, dbTask);
            ProductionTask task = new ProductionTask(dbTask);
            Unit.Tasks.Insert(dbTask);
            Unit.Commit();
            SelectedTask.Children.Add(task);
            SelectedTask.HasChildren = true;
            SelectedTask.IsExpanded = true;
            SelectedTask = task;
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
                    ProductionTask parent = (ProductionTask)SelectedTask.Parent;
                    SelectedTask.Remove(Unit);
                    Unit.Commit();
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
                    if (parent?.Children.Count == 0)
                    {
                        parent.HasChildren = false;
                        parent.IsExpanded = false;
                    }
                    break;
            }
        }

        #endregion

        #region LevelUpCommand

        public ICommand LevelUpCommand { get; }

        private bool CanLevelUpCommandExecute(object p) => SelectedTask != null && SelectedTask?.Task.MyParent != null;

        private void OnLevelUpCommandExecuted(object p)
        {
            ProductionTask parent = (ProductionTask)SelectedTask.Parent;
            ProductionTask task = ProductionTask.InitTask(SelectedTask.Task);
            SelectedTask.IsExpanded = false;
            parent.Children.Remove(SelectedTask);
            if (parent.Parent == null)
            {
                Model.Add(task);
                task.Task.MyParent = null;
            }
            else
            {
                parent.Parent.Children.Add(task);
                task.Task.MyParent.Parent = ((ProductionTask)parent.Parent).Task;
            }
            if(parent.Children.Count == 0)
            {
                parent.IsExpanded = false;
                parent.HasChildren = false;
            }
            Unit.Commit();
            SelectedTask = task;
        }

        #endregion

        #region LevelDownCommand

        public ICommand LevelDownCommand { get; }

        private bool CanLevelDownCommandExecute(object p) => SelectedTask != null;

        private void OnLevelDownCommandExecuted(object p)
        {
            ProductionTaskDB dbTask = new ProductionTaskDB("Новое изделие");
            ProductionTask task = new ProductionTask(dbTask);
            ProductionTask downTask = ProductionTask.InitTask(SelectedTask.Task);
            ProductionTask parent = (ProductionTask)SelectedTask.Parent;

            if (SelectedTask.Task.MyParent != null)
            {
                dbTask.MyParent = new HierarchyDB(SelectedTask.Task.MyParent.Parent, dbTask);
                SelectedTask.Task.MyParent.Parent = dbTask;
                parent.Children.Add(task);
                parent.Children.Remove(SelectedTask);
            }
            else
            {
                SelectedTask.Task.MyParent = new HierarchyDB(dbTask, SelectedTask.Task);
                Model.Add(task);
                Model.Remove(SelectedTask);
            }
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
            TaskToCopy.AddTasksToDatabase(Unit, Model, (ProductionTask)SelectedTask.Parent);
        }

        #endregion

        #region ShowSearchGridCommand

        public ICommand ShowSearchGridCommand { get; }

        private bool CanShowSearchGridCommandExecute(object p) => true;

        private void OnShowSearchGridCommandExecuted(object p)
        {
            if(((Grid)p).RowDefinitions.ElementAt(2).Height != new GridLength(0))
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
            SearchResults = new List<ProductionTask>();
            switch (SelectedSearchIndex)
            {
                case 0:
                    foreach(ProductionTask root in Model)
                    {
                        SearchByName(root);
                    }
                    break;
                case 1:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByManagDoc(root);
                    }
                    break;
                case 2:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByCount(root);
                    }
                    break;
                case 3:
                    foreach (ProductionTask root in Model)
                    {
                        SearchBySpecificationCost(root);
                    }
                    break;
                case 4:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByIncDoc(root);
                    }
                    break;
                case 5:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByVishDate(root);
                    }
                    break;
                case 6:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByRealDate(root);
                    }
                    break;
                case 7:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByComplectation(root);
                    }
                    break;
                case 8:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByComplectationDate(root);
                    }
                    break;
                case 9:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByPercent(root);
                    }
                    break;
                case 10:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByMSLNumber(root);
                    }
                    break;
                case 11:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByExecutor(root);
                    }
                    break;
                case 12:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByExecutor2(root);
                    }
                    break;
                case 13:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByGivingDate(root);
                    }
                    break;
                case 14:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByProjectedDate(root);
                    }
                    break;
                case 15:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByComplectationDate(root);
                    }
                    break;
                case 16:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByManufacture(root);
                    }
                    break;
                case 17:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByLetterNum(root);
                    }
                    break;
                case 18:
                    foreach (ProductionTask root in Model)
                    {
                        SearchBySpecNum(root);
                    }
                    break;
                case 19:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByBill(root);
                    }
                    break;
                case 20:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByReport(root);
                    }
                    break;
                case 21:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByReturnReport(root);
                    }
                    break;
                case 22:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByReceivingDate(root);
                    }
                    break;
                case 23:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByExpendNum(root);
                    }
                    break;
                case 24:
                    foreach (ProductionTask root in Model)
                    {
                        SearchByNote(root);
                    }
                    break;
                default:
                    SearchResultString = "Параметр для поиска не задан";
                    break;

            }
            if(SearchResults.Count == 0)
            {
                if(ErrorMessage == null)
                {
                    SearchResultString = "Совпадений не найдено";
                }
                else
                {
                    SearchResultString = ErrorMessage;
                }
            }
            else
            {
                SearchResultString = $"Количество совпадений: {SearchResults.Count}";
                SelectedTask = SearchResults.First();
                ((DataGrid)p).UpdateLayout();
                ((DataGrid)p).ScrollIntoView(SelectedTask);
            }
        }

        #endregion

        #region MoveNextResultCommand

        public ICommand MoveNextResultCommand { get; }

        private bool CanMoveNextResultCommandExecute(object p) => SelectedTask != null && SearchResults != null && SearchResults.Count!=0 && SelectedTask != SearchResults.Last();

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

        #region Методы

        private void Expand(ProductionTask task)
        {
            task.IsExpanded = true;
            if (task.HasChildren)
            {
                foreach(ProductionTask t in task.Children)
                {
                    Expand(t);
                }
            }
        }

        private void RollUp(ProductionTask task)
        {
            task.IsExpanded = false;
            if (task.HasChildren)
            {
                foreach (ProductionTask t in task.Children)
                {
                    RollUp(t);
                }
            }
        }

        #region SearchFunctions

        private void SearchByName(ProductionTask task)
        {
            if (task.Task.Name.ToLower().Contains(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchByName(child);
                }
            }
        }

        private void SearchByManagDoc(ProductionTask task)
        {
            if (task.Task.ManagDoc != null && task.Task.ManagDoc.ToLower().Contains(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchByManagDoc(child);
                }
            }
        }

        private void SearchByCount(ProductionTask task)
        {
            ErrorMessage = null;
            int count;
            if(Int32.TryParse(SearchString, out count))
            {
                if (task.Task.Count == count)
                {
                    SearchResults.Add(task);
                }
                if (task.HasChildren)
                {
                    foreach (ProductionTask child in task.Children)
                    {
                        SearchByCount(child);
                    }
                }
            }
            else
            {
                ErrorMessage = "Поисковые данные заданы неверно";
            }
            
        }

        private void SearchBySpecificationCost(ProductionTask task)
        {
            if (task.Task.SpecCost.ToLower().Equals(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchBySpecificationCost(child);
                }
            }
        }

        private void SearchByIncDoc(ProductionTask task)
        {
            if (task.Task.IncDoc!=null&&task.Task.IncDoc.ToLower().Contains(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchByIncDoc(child);
                }
            }
        }

        private void SearchByVishDate(ProductionTask task)
        {
            ErrorMessage = null;
            DateTime vishDate;
            if(DateTime.TryParse(SearchString, out vishDate))
            {
                if (task.Task.VishDate!=null && task.Task.VishDate.Equals(vishDate))
                {
                    SearchResults.Add(task);
                }
                if (task.HasChildren)
                {
                    foreach (ProductionTask child in task.Children)
                    {
                        SearchByVishDate(child);
                    }
                }
            }
            else
            {
                ErrorMessage = "Поисковые данные заданы неверно. Формат даты: дд.мм.гггг";
            }
            
        }

        private void SearchByRealDate(ProductionTask task)
        {
            ErrorMessage = null;
            DateTime vishDate;
            if (DateTime.TryParse(SearchString, out vishDate))
            {
                if (task.Task.RealDate != null && task.Task.RealDate.Equals(vishDate))
                {
                    SearchResults.Add(task);
                }
                if (task.HasChildren)
                {
                    foreach (ProductionTask child in task.Children)
                    {
                        SearchByRealDate(child);
                    }
                }
            }
            else
            {
                ErrorMessage = "Поисковые данные заданы неверно. Формат даты: дд.мм.гггг";
            }

        }

        private void SearchByComplectation(ProductionTask task)
        {
            if (task.Task.Complectation.Complectation!=null&&task.Task.Complectation.Complectation.ToLower().Contains(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchByComplectation(child);
                }
            }
        }

        private void SearchByComplectationDate(ProductionTask task)
        {
            ErrorMessage = null;
            DateTime vishDate;
            if (DateTime.TryParse(SearchString, out vishDate))
            {
                if (task.Task.Complectation.ComplectationDate != null && task.Task.Complectation.ComplectationDate.Equals(vishDate))
                {
                    SearchResults.Add(task);
                }
                if (task.HasChildren)
                {
                    foreach (ProductionTask child in task.Children)
                    {
                        SearchByComplectationDate(child);
                    }
                }
            }
            else
            {
                ErrorMessage = "Поисковые данные заданы неверно. Формат даты: дд.мм.гггг";
            }

        }

        private void SearchByPercent(ProductionTask task)
        {
            ErrorMessage = null;
            float count;
            if (float.TryParse(SearchString, out count))
            {
                if (task.Task.Complectation.Percentage == count)
                {
                    SearchResults.Add(task);
                }
                if (task.HasChildren)
                {
                    foreach (ProductionTask child in task.Children)
                    {
                        SearchByPercent(child);
                    }
                }
            }
            else
            {
                ErrorMessage = "Поисковые данные заданы неверно";
            }

        }

        private void SearchByMSLNumber(ProductionTask task)
        {
            if (task.Task.InProduction.Number != null && task.Task.InProduction.Number.ToLower().Contains(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchByMSLNumber(child);
                }
            }
        }

        private void SearchByExecutor(ProductionTask task)
        {
            if (task.Task.InProduction.ExecutorName != null && task.Task.InProduction.ExecutorName.ToLower().Contains(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchByExecutor(child);
                }
            }
        }

        private void SearchByExecutor2(ProductionTask task)
        {
            if (task.Task.InProduction.ExecutorName2 != null && task.Task.InProduction.ExecutorName2.ToLower().Contains(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchByExecutor2(child);
                }
            }
        }

        private void SearchByGivingDate(ProductionTask task)
        {
            ErrorMessage = null;
            DateTime vishDate;
            if (DateTime.TryParse(SearchString, out vishDate))
            {
                if (task.Task.InProduction.GivingDate != null && task.Task.InProduction.GivingDate.Equals(vishDate))
                {
                    SearchResults.Add(task);
                }
                if (task.HasChildren)
                {
                    foreach (ProductionTask child in task.Children)
                    {
                        SearchByGivingDate(child);
                    }
                }
            }
            else
            {
                ErrorMessage = "Поисковые данные заданы неверно. Формат даты: дд.мм.гггг";
            }

        }

        private void SearchByProjectedDate(ProductionTask task)
        {
            ErrorMessage = null;
            DateTime vishDate;
            if (DateTime.TryParse(SearchString, out vishDate))
            {
                if (task.Task.InProduction.ProjectedDate != null && task.Task.InProduction.ProjectedDate.Equals(vishDate))
                {
                    SearchResults.Add(task);
                }
                if (task.HasChildren)
                {
                    foreach (ProductionTask child in task.Children)
                    {
                        SearchByProjectedDate(child);
                    }
                }
            }
            else
            {
                ErrorMessage = "Поисковые данные заданы неверно. Формат даты: дд.мм.гггг";
            }

        }

        private void SearchByCompletionDate(ProductionTask task)
        {
            ErrorMessage = null;
            DateTime vishDate;
            if (DateTime.TryParse(SearchString, out vishDate))
            {
                if (task.Task.InProduction.CompletionDate != null && task.Task.InProduction.CompletionDate.Equals(vishDate))
                {
                    SearchResults.Add(task);
                }
                if (task.HasChildren)
                {
                    foreach (ProductionTask child in task.Children)
                    {
                        SearchByCompletionDate(child);
                    }
                }
            }
            else
            {
                ErrorMessage = "Поисковые данные заданы неверно. Формат даты: дд.мм.гггг";
            }

        }

        private void SearchByManufacture(ProductionTask task)
        {
            if (task.Task.Manufacture.Name != null && task.Task.Manufacture.Name.ToLower().Contains(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchByManufacture(child);
                }
            }
        }

        private void SearchByLetterNum(ProductionTask task)
        {
            if (task.Task.Manufacture.LetterNum != null && task.Task.Manufacture.LetterNum.ToLower().Contains(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchByLetterNum(child);
                }
            }
        }

        private void SearchBySpecNum(ProductionTask task)
        {
            if (task.Task.Manufacture.SpecNum != null && task.Task.Manufacture.SpecNum.ToLower().Contains(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchBySpecNum(child);
                }
            }
        }

        private void SearchByBill(ProductionTask task)
        {
            if (task.Task.Giving.Bill != null && task.Task.Giving.Bill.ToLower().Contains(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchByBill(child);
                }
            }
        }

        private void SearchByReport(ProductionTask task)
        {
            if (task.Task.Giving.Report != null && task.Task.Giving.Report.ToLower().Contains(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchByReport(child);
                }
            }
        }

        private void SearchByReturnReport(ProductionTask task)
        {
            if (task.Task.Giving.ReturnReport != null && task.Task.Giving.ReturnReport.ToLower().Contains(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchByReturnReport(child);
                }
            }
        }

        private void SearchByReceivingDate(ProductionTask task)
        {
            ErrorMessage = null;
            DateTime vishDate;
            if (DateTime.TryParse(SearchString, out vishDate))
            {
                if (task.Task.Giving.ReceivingDate != null && task.Task.Giving.ReceivingDate.Equals(vishDate))
                {
                    SearchResults.Add(task);
                }
                if (task.HasChildren)
                {
                    foreach (ProductionTask child in task.Children)
                    {
                        SearchByReceivingDate(child);
                    }
                }
            }
            else
            {
                ErrorMessage = "Поисковые данные заданы неверно. Формат даты: дд.мм.гггг";
            }

        }

        private void SearchByExpendNum(ProductionTask task)
        {
            if (task.Task.ExpendNum != null && task.Task.ExpendNum.ToLower().Contains(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchByExpendNum(child);
                }
            }
        }

        private void SearchByNote(ProductionTask task)
        {
            if (task.Task.Note != null && task.Task.Note.ToLower().Contains(SearchString.ToLower()))
            {
                SearchResults.Add(task);
            }
            if (task.HasChildren)
            {
                foreach (ProductionTask child in task.Children)
                {
                    SearchByNote(child);
                }
            }
        }

        #endregion

        #endregion

        public ProductionPlanControlViewModel()
        {
        }

        public ProductionPlanControlViewModel(ApplicationUnit unit, UserDB user)
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

            #endregion

            User = user;
            Unit = unit;
            ProductionTasks = Unit.Tasks.Get().ToList();
            Model = ProductionTask.InitModel(ProductionTasks);
        }
    }
}

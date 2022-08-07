using Common.Wpf.Data;
using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
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
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace CP_2021.ViewModels
{
    class ProductionPlanControlViewModel : ViewModelBase
    {

        #region Свойства
        private ApplicationUnit Unit;
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Guid? _taskToCopyId = null;
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

        #region Color

        private Color _color;

        public Color Color
        {
            get => _color;
            set => Set(ref _color, value);
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
        #region SelectedFontSize

        private int _selectedFontSize;

        public int SelectedFontSize
        {
            get => _selectedFontSize;
            set => Set(ref _selectedFontSize, value);
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
        #endregion
        #region Команды

        private void ShowError(Exception ex)
        {
            MessageBox.Show("Произошла ошибка.Обновите базу.Подробности записаны в лог");
            _log.Error(ex.Message);
        }

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
                    _log.Error("Inserted task in AddProductionTaskCommand while Model.Count = 0 is null");
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

            try
            {
                // Добавить задачу на тот же уровень под выбранную задачу
                Task_Hierarchy_Formatting task = TasksOperations.InsertEmptyTask(SelectedTask.data.ParentId, SelectedTask.data.LineOrder + 1);
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
            catch(Exception ex)
            {
                ShowError(ex);
            }
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

            try
            {
                Task_Hierarchy_Formatting task = TasksOperations.InsertEmptyTask(SelectedTask.data.Id, line);

                if (task == null)
                {
                    _log.Error("In AddChildCommand task after insert empty task was null");
                    MessageBox.Show("При вставке нового значения произошла ошибка");
                    return;
                }
                ProductionTask pTask = new ProductionTask(task);

                SelectedTask.data.ChildrenCount += 1;
                SelectedTask.HasChildren = true;
                if (SelectedTask.IsExpanded)
                {
                    SelectedTask.Children.Add(pTask);
                    SelectedTask = pTask;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
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
                TasksOperations.UpdateProductionPlan(SelectedTask.data.Id, SelectedTask.data.IncDoc, SelectedTask.data.Name, SelectedTask.data.Count, SelectedTask.data.SpecCost, SelectedTask.data.Note, SelectedTask.data.Expanded, SelectedTask.data.Completion, SelectedTask.data.EditingBy);
            }
            catch(Exception ex)
            {
                ShowError(ex);
            }
        }

        #endregion
        #region DeleteProductionTaskCommand

        public ICommand DeleteProductionTaskCommand { get; }

        private bool CanDeleteProductionTaskCommandExecute(object p) => SelectedTask != null;

        private void OnDeleteProductionTaskCommandExecuted(object p)
        {

            MessageBoxResult result = MessageBox.Show($"Вы действительно хотите удалить элемент {SelectedTask.data.Name}?", "Удаление", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        TasksOperations.DropTaskBtId(SelectedTask.data.Id);
                        ProductionTask topTask = null;
                        if (SelectedTask.Parent == null)
                        {
                            if (Model.IndexOf(SelectedTask) != 0)
                                topTask = (ProductionTask)Model.ElementAt(Model.IndexOf(SelectedTask) - 1);
                            SelectedTask.UpTasksModel(Model);
                            Model.Remove(SelectedTask);
                        }
                        else
                        {

                            ProductionTask parent = (ProductionTask)SelectedTask.Parent;
                            if (parent.Children.IndexOf(SelectedTask) != 0)
                                topTask = (ProductionTask)parent.Children.ElementAt(parent.Children.IndexOf(SelectedTask) - 1);
                            SelectedTask.UpTasksChildren(parent);
                            SelectedTask.Parent.Children.Remove(SelectedTask);
                            if (parent.Children.Count == 0)
                            {
                                parent.HasChildren = parent.IsExpanded = false;
                            }
                        }
                        if (topTask != null)
                            SelectedTask = topTask;
                    }
                    catch(Exception ex)
                    {
                        ShowError(ex);
                    }
                    break;
            }
        }

        #endregion
        #region LevelUpCommand

        public ICommand LevelUpCommand { get; }

        private bool CanLevelUpCommandExecute(object p) => SelectedTask?.Parent != null;

        private void OnLevelUpCommandExecuted(object p)
        {
            ProductionTask parent = (ProductionTask)SelectedTask.Parent;
            ProductionTask parentParent = (ProductionTask)SelectedTask.Parent.Parent;
            ProductionTask task = SelectedTask;
            
            try
            {
                parent.Children.Remove(SelectedTask);
                if (parent.Children.Count == 0)
                    parent.IsExpanded = parent.HasChildren = false;

                if (parentParent == null)
                {
                    TasksOperations.LevelUpTask(task.data.Id, task.data.LineOrder, parent.data.Id, parent.data.LineOrder + 1);
                    parent.DownTasksModel(Model);
                    Model.Insert(parent.data.LineOrder, task);
                }
                else
                {
                    TasksOperations.LevelUpTask(task.data.Id, task.data.LineOrder, parent.data.Id, parent.data.LineOrder + 1);
                    parent.DownTasksChildren(parentParent);
                    parentParent.Children.Insert(parent.data.LineOrder, task);
                }

                task.IsExpanded = false;
                task.UpTasksChildren(parent);
                task.data.LineOrder = parent.data.LineOrder + 1;
            }
            catch(Exception ex)
            {
                ShowError(ex);
            }
            
        }

        #endregion
        #region LevelDownCommand

        public ICommand LevelDownCommand { get; }

        private bool CanLevelDownCommandExecute(object p) => SelectedTask != null && SelectedTask?.data.LineOrder != 1;

        private void OnLevelDownCommandExecuted(object p)
        {
            ProductionTask task = SelectedTask;
            ProductionTask parent = (ProductionTask)SelectedTask.Parent;
            ProductionTask newParent = null;
            try
            {
                if (parent == null)
                {
                    newParent = (ProductionTask)Model.ElementAt(task.data.LineOrder - 2);
                    TasksOperations.LevelDownTask(task.data.Id, task.data.LineOrder, null, newParent.data.Id);
                    task.UpTasksModel(Model);
                    Model.Remove(SelectedTask);
                    
                }
                else
                {
                    newParent = (ProductionTask)parent.Children.ElementAt(task.data.LineOrder - 2);
                    TasksOperations.LevelDownTask(task.data.Id, task.data.LineOrder, parent.data.Id, newParent.data.Id);
                    task.UpTasksChildren(parent);
                    parent.Children.Remove(SelectedTask);
                }
                if (!newParent.IsExpanded)
                {
                    newParent.IsExpanded = true;
                    newParent.LoadChildren();
                }
                else
                {
                    newParent.Children.Add(task);
                    if (newParent.Children.Count == 0)
                        task.data.LineOrder = 1;
                    else
                        task.data.LineOrder = newParent.Children.Cast<ProductionTask>().Max(t => t.data.LineOrder) + 1;
                }
                newParent.HasChildren = true;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ShowError(ex);
            }
            catch(Exception ex)
            {
                ShowError(ex);
            }
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
        #region Copy, Paste
        #region CopyTaskCommand

        public ICommand CopyTaskCommand { get; }

        private bool CanCopyTaskCommandExecute(object p) => true;

        private void OnCopyTaskCommandExecuted(object p)
        {
            Debug.WriteLine("Copy");
            if(SelectedTask?.data != null)
            {
                _taskToCopyId = SelectedTask.data.Id;
            }
        }

        #endregion
        #region PasteTaskCommand

        public ICommand PasteTaskCommand { get; }

        private bool CanPasteTaskCommandExecute(object p) => _taskToCopyId.HasValue;

        private void OnPasteTaskCommandExecuted(object p)
        {
            try
            {
                ProductionTask parent = (ProductionTask)SelectedTask.Parent;
                if(parent == null)
                {
                    var task = TasksOperations.PasteTask(_taskToCopyId.Value, null, SelectedTask.data.LineOrder + 1);
                    SelectedTask.DownTasksModel(Model);
                    Model.Insert(SelectedTask.data.LineOrder, new ProductionTask(task));
                }
                else
                {
                    var task = TasksOperations.PasteTask(_taskToCopyId.Value, parent.data.Id, SelectedTask.data.LineOrder + 1);
                    SelectedTask.DownTasksChildren(parent);
                    parent.Children.Insert(SelectedTask.data.LineOrder, new ProductionTask(task));
                }
            }catch(Exception ex)
            {
                ShowError(ex);
            }
        }

        #endregion
        #endregion
        #region UpTaskCommand

        public ICommand UpTaskCommand { get; }

        private bool CanUpTaskCommandExecute(object p) => SelectedTask != null && SelectedTask.data.LineOrder != 1;

        private void OnUpTaskCommandExecuted(object p)
        {
            SelectedTask.Collapse();
            int taskToUpOrder = SelectedTask.data.LineOrder;
            Guid taskToUpId = SelectedTask.data.Id;
            Guid taskToDownId;

            ProductionTask parent = (ProductionTask)SelectedTask.Parent;
            ProductionTask taskToUp = SelectedTask;
            ProductionTask taskToDown = null;
            if (parent == null)
                taskToDown = Model.Cast<ProductionTask>().Where(t => t.data.LineOrder == taskToUpOrder - 1).SingleOrDefault();
            else
                taskToDown = parent.Children.Cast<ProductionTask>().Where(t => t.data.LineOrder == taskToUpOrder - 1).SingleOrDefault();
            try
            {
                if (taskToDown != null)
                {
                    taskToDownId = taskToDown.data.Id;

                    TasksOperations.UpTask(taskToUpId, taskToDownId);
                    SelectedTask.data.LineOrder -= 1;
                    taskToDown.data.LineOrder += 1;

                    if (parent == null)
                    {
                        taskToDown.IsExpanded = false;
                        taskToUp.IsExpanded = false;
                        Model.Remove(taskToDown);
                        Model.Remove(taskToUp);
                        Model.Insert(taskToUp.data.LineOrder - 1, taskToUp);
                        Model.Insert(taskToDown.data.LineOrder - 1, taskToDown);
                    }
                    else
                    {
                        parent.Children.Remove(taskToDown);
                        parent.Children.Remove(taskToUp);
                        parent.Children.Insert(taskToUp.data.LineOrder - 1, taskToUp);
                        parent.Children.Insert(taskToDown.data.LineOrder - 1, taskToDown);
                    }
                    SelectedTask = taskToUp;
                    return;
                }

            }
            catch(Exception ex)
            {
                ShowError(ex);
                return;
            }
            
            MessageBox.Show("Не удалось выполнить операцию. Обновите базу");
        }

        #endregion
        #region DownTaskCommand

        public ICommand DownTaskCommand { get; }

        private bool CanDownTaskCommandExecute(object p) => Model.Count != 0 && SelectedTask?.data.LineOrder<(SelectedTask?.Parent == null? Model?.Cast<ProductionTask>().Max(t => t.data.LineOrder) : SelectedTask?.Parent?.Children.Cast<ProductionTask>().Max(t => t.data.LineOrder));

        private void OnDownTaskCommandExecuted(object p)
        {
            SelectedTask.Collapse();
            int taskToDownOrder = SelectedTask.data.LineOrder;
            Guid taskToDownId = SelectedTask.data.Id;
            Guid taskToUpId;

            ProductionTask parent = (ProductionTask)SelectedTask.Parent;
            ProductionTask taskToDown = SelectedTask;
            ProductionTask taskToUp = null;
            if(parent == null)
                taskToUp = Model.Cast<ProductionTask>().Where(t => t.data.LineOrder == taskToDownOrder + 1).SingleOrDefault();
            else
                taskToUp = parent.Children.Cast<ProductionTask>().Where(t => t.data.LineOrder == taskToDownOrder + 1).SingleOrDefault();
            try
            {
                if (taskToUp != null)
                {
                    taskToUpId = taskToUp.data.Id;

                    TasksOperations.DownTask(taskToDownId, taskToUpId);
                    SelectedTask.data.LineOrder += 1;
                    taskToUp.data.LineOrder -= 1;
                    if (parent == null)
                    {
                        taskToUp.IsExpanded = false;
                        taskToDown.IsExpanded = false;
                        Model.Remove(taskToUp);
                        Model.Remove(taskToDown);
                        Model.Insert(taskToUp.data.LineOrder - 1, taskToUp);
                        Model.Insert(taskToDown.data.LineOrder - 1, taskToDown);
                    }
                    else
                    {
                        parent.Children.Remove(taskToUp);
                        parent.Children.Remove(taskToDown);
                        parent.Children.Insert(taskToUp.data.LineOrder - 1, taskToUp);
                        parent.Children.Insert(taskToDown.data.LineOrder - 1, taskToDown);
                    }
                    SelectedTask = taskToDown;
                    return;
                }
            }
            catch(Exception ex)
            {
                ShowError(ex);
                return;
            }
            
            MessageBox.Show("Не удалось выполнить операцию. Обновите базу");
        }

        #endregion
        #region Formatting
        #region SetBoldCommand

        public ICommand SetBoldCommand { get; }

        private bool CanSetBoldCommandExecute(object p) => SelectedTask != null && SelectedTask.data != null;

        private void OnSetBoldCommandExecuted(object p)
        {
            try
            {
                TasksOperations.SetBold(SelectedTask.data.Id, !SelectedTask.data.IsBold);
            }
            catch(Exception ex)
            {
                ShowError(ex);
                return;
            }
            SelectedTask.data.IsBold = !SelectedTask.data.IsBold;
        }

        #endregion
        #region SetFontSize

        public ICommand SetFontSize { get; }

        private bool CanSetFontSizeCommandExecute(object p) => SelectedTask != null && SelectedTask.data != null;

        private void OnSetFontSizeCommandExecuted(object p)
        {
            try
            {
                TasksOperations.SetFontSize(SelectedTask.data.Id, SelectedFontSize);
            }
            catch(Exception ex)
            {
                ShowError(ex);
                return;
            }
            SelectedTask.data.FontSize = SelectedFontSize;
        }

        #endregion
        #region SetCurrentElementFontSize

        public ICommand SetCurrentElementFontSize { get; }

        private bool CanSetCurrentElementFontSizeCommandExecute(object p) => SelectedTask != null && SelectedTask.data != null;

        private void OnSetCurrentElementFontSizeCommandExecuted(object p)
        {
            SelectedFontSize = SelectedTask.data.FontSize;
        }

        #endregion
        #endregion
        #region Windows
        public void GetTaskIdFromReport(object sender, WindowEventArgs e)
        {
            var result = TasksOperations.GetTaskById(e.taskId);
            if (result != null)
            {
                var task = Model.FlatModel.Cast<ProductionTask>().Where(t => t.data.Id == result.Id).FirstOrDefault();
                if(task!= null)
                {
                    switch (e.dataWindow)
                    {
                        case DataWindow.Complectation:
                            task.data.Rack = result.Rack;
                            task.data.Shelf = result.Shelf;
                            task.data.Percentage = result.Percentage;
                            task.data.Complectation = result.Complectation;
                            break;
                        case DataWindow.ConsumeAct:
                            task.data.ActNumber = result.ActNumber;
                            task.data.ActCreation = result.ActCreation;
                            task.data.ActDate = result.ActDate;
                            task.data.ByAct = result.ByAct;
                            task.data.IsItalic = result.IsItalic;
                            break;
                        case DataWindow.Document:
                            task.data.ManagDoc = result.ManagDoc;
                            break;
                        case DataWindow.Giving:
                            task.data.State = result.State;
                            break;
                        case DataWindow.InProduction:
                            task.data.GivingDate = result.GivingDate;
                            break;
                        case DataWindow.Manufacture:
                            task.data.M_Name = result.M_Name;
                            break;
                        case DataWindow.TimedGiving:
                            task.data.IsTimedGiving = result.IsTimedGiving;
                            task.data.IsSKBCheck = result.IsSKBCheck;
                            task.data.IsOECStorage = result.IsOECStorage;
                            break;
                        default:
                            break;
                    }
                }
                
            }
            
        }
        #region OpenPaymentWindowCommand

        public ICommand OpenPaymentWindowCommand { get; }

        private bool CanOpenPaymentWindowCommandExecute(object p) => true;

        private void OnOpenPaymentWindowCommandExecuted(object p)
        {
            PaymentWindow window = new PaymentWindow();
            var entity = TasksOperations.GetPaymentData(SelectedTask.data.Id);
            if (entity != null)
            {
                PaymentWindowVM vw = new PaymentWindowVM(entity, SelectedTask, window);
                vw.SendIdToPlan += GetTaskIdFromReport;
                window.DataContext = vw;
                window.Show();
            }
        }

        #endregion
        #region OpenLaborCostsWindowCommand

        public ICommand OpenLaborCostsWindowCommand { get; }

        private bool CanOpenLaborCostsWindowCommandExecute(object p) => true;

        private void OnOpenLaborCostsWindowCommandExecuted(object p)
        {
            LaborCostsWindow window = new LaborCostsWindow();
            var entity = TasksOperations.GetLaborCostsData(SelectedTask.data.Id);
            if (entity != null)
            {
                LaborCostsWindowVM vw = new LaborCostsWindowVM(entity, SelectedTask, window);
                vw.SendIdToPlan += GetTaskIdFromReport;
                window.DataContext = vw;
                window.Show();
            }
        }

        #endregion
        #region OpenDocumentWindowCommand

        public ICommand OpenDocumentWindowCommand { get; }

        private bool CanOpenDocumentWindowCommandExecute(object p) => true;

        private void OnOpenDocumentWindowCommandExecuted(object p)
        {
            DocumentWindow window = new DocumentWindow();
            var entity = TasksOperations.GetDocumentationData(SelectedTask.data.Id);
            if (entity != null)
            {
                DocumentWindowVM vw = new DocumentWindowVM(entity, SelectedTask, window);
                vw.SendIdToPlan += GetTaskIdFromReport;
                window.DataContext = vw;
                window.Show();
            }
        }

        #endregion
        #region OpenComplectationWindowCommand

        public ICommand OpenComplectationWindowCommand { get; }

        private bool CanOpenComplectationWindowCommandExecute(object p) => true;

        private void OnOpenComplectationWindowCommandExecuted(object p)
        {
            ComplectationWindow window = new ComplectationWindow();
            var entity = TasksOperations.GetComplectationWindowEntity(SelectedTask.data.Id);
            if(entity != null)
            {
                ComplectationWindowVM vm = new ComplectationWindowVM(entity, SelectedTask, window);
                vm.SendIdToPlan += GetTaskIdFromReport;
                window.DataContext = vm;
                //window.Closed += Window_Closed;
                window.Show();
            }
            else
            {
                MessageBox.Show("Не удалось загрузить данные. Обновите базу");
            }
            
        }

        #endregion
        #region OpenActWindowCommand

        public ICommand OpenActWindowCommand { get; }

        private bool CanOpenActWindowCommandExecute(object p) => true;

        private void OnOpenActWindowCommandExecuted(object p)
        {
            ConsumeActWindow window = new ConsumeActWindow();
            var entity = TasksOperations.GetConsumeActWindowEntity(SelectedTask.data.Id);
            if(entity != null)
            {
                ConsumeActWindowVM vm = new ConsumeActWindowVM(entity, SelectedTask, window);
                vm.SendIdToPlan += GetTaskIdFromReport;
                window.DataContext = vm;
                //window.Closed += Window_Closed;
                window.Show();
            }
            else
            {
                MessageBox.Show("Не удалось загрузить данные. Обновите базу");
            }
            
        }

        #endregion
        #region OpenGivingWindowCommand

        public ICommand OpenGivingWindowCommand { get; }

        private bool CanOpenGivingWindowCommandExecute(object p) => true;

        private void OnOpenGivingWindowCommandExecuted(object p)
        {
            GivingWindow window = new GivingWindow();
            var entity = TasksOperations.GetGivingData(SelectedTask.data.Id);
            if (entity != null)
            {
                GivingWindowVM vm = new GivingWindowVM(entity, SelectedTask, window);
                vm.SendIdToPlan += GetTaskIdFromReport;
                window.DataContext = vm;
                window.Show();
            }
            else
            {
                MessageBox.Show("Не удалось загрузить данные. Обновите базу");
            }
        }

        #endregion
        #region OpenManufactureWindowCommand

        public ICommand OpenManufactureWindowCommand { get; }

        private bool CanOpenManufactureWindowCommandExecute(object p) => true;

        private void OnOpenManufactureWindowCommandExecuted(object p)
        {
             ManufactureWindow window = new ManufactureWindow();
            var entity = TasksOperations.GetManufactureData(SelectedTask.data.Id);
            if (entity != null)
            {
                ManufactureWindowVM vm = new ManufactureWindowVM(entity, SelectedTask, window);
                vm.SendIdToPlan += GetTaskIdFromReport;
                window.DataContext = vm;
                window.Show();
            }
            else
            {
                MessageBox.Show("Не удалось загрузить данные. Обновите базу");
            }
        }

        #endregion
        #region OpenInProductionWindowCommand

        public ICommand OpenInProductionWindowCommand { get; }

        private bool CanOpenInProductionWindowCommandExecute(object p) => true;

        private void OnOpenInProductionWindowCommandExecuted(object p)
        {
            InProductionWindow window = new InProductionWindow();
            var entity = TasksOperations.GetInProductionData(SelectedTask.data.Id);
            if (entity != null)
            {
                InProductionWindowVM vm = new InProductionWindowVM(entity, SelectedTask, window);
                vm.SendIdToPlan += GetTaskIdFromReport;
                window.DataContext = vm;
                window.Show();
            }
            else
            {
                MessageBox.Show("Не удалось загрузить данные. Обновите базу");
            }
        }

        #endregion
        #region OpenSearchWindowCommand

        public ICommand OpenSearchWindowCommand { get; }

        private bool CanOpenSearchWindowCommandExecute(object p) => true;

        private void OnOpenSearchWindowCommandExecuted(object p)
        {
            SearchWindow window = new SearchWindow();
            if(SelectedTask == null)
            {
                ((SearchResultsVM)(window.DataContext)).SendTaskIdToReportVM += SetSelectedTaskFromReport;
            }
            else
            {
                SearchResultsVM searchResultsVM = new SearchResultsVM(SelectedTask.Task.Name);
            }
            window.Show();
        }

        #endregion
        #region OpenSearchWindowFromShortcutCommand
        public ICommand OpenSearchWindowFromShortcutCommand { get; }
        private bool CanOpenSearchWindowFromShortcutCommandExecute(object p) => SelectedTask != null;
        private void OnOpenSearchWindowFromShortcutCommandExecuted(object p)
        {
            SearchWindow window = new SearchWindow();
            SearchResultsVM searchVm = new SearchResultsVM(SelectedTask.data.SpecNum);
            window.DataContext = searchVm;
            window.Show();
        }
        #endregion
        #region OpenTimedGivingWindowCommand

        public ICommand OpenTimedGivingWindowCommand { get; }

        private bool CanOpenTimedGivingWindowCommandExecute(object p) => true;

        private void OnOpenTimedGivingWindowCommandExecuted(object p)
        {
            TimedGivingWindow window = new TimedGivingWindow();
            var entity = TasksOperations.GetTimedGivingData(SelectedTask.data.Id);
            if (entity != null)
            {
                TimedGivingWindowVM vw = new TimedGivingWindowVM(entity, SelectedTask, window);
                vw.SendIdToPlan += GetTaskIdFromReport;
                window.DataContext = vw;
                window.Show();
            }
        }

        #endregion
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
                    break;
                default:
                    break;
            }
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
            if (p is ProductionTask && p == SelectedTask && ((ProductionTask)p).Children.Count == 0)
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
        #region ExportTaskCommand

        public ICommand ExportTaskCommand { get; }

        private bool CanExportTaskCommandExecute(object p) => SelectedTask?.data!=null;

        private void OnExportTaskCommandExecuted(object p)
        {
            XMLOperations.ExportTaskById(SelectedTask.data.Id);
        }

        #endregion
        #region ImportTaskCommand

        public ICommand ImportTaskCommand { get; }

        private bool CanImportTaskCommandExecute(object p) => true;

        private void OnImportTaskCommandExecuted(object p)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML files (*.xml)|*.xml";
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var p1 = dialog.FileName.Replace(@"\\", @"\");
                    var taskId = XMLOperations.ImportFromXML(dialog.FileName, SelectedTask.data.ParentId, SelectedTask.data.LineOrder + 1);
                    
                    //if(taskId != null)
                    //{
                        
                    //    var task = TasksOperations.GetTaskById(taskId.Value);
                    //    ProductionTask pTask = new ProductionTask(task);
                    //    if (SelectedTask.Parent == null)
                    //    {
                    //        SelectedTask.DownTasksModel(Model);
                    //        Model.Insert(SelectedTask.data.LineOrder, pTask);
                    //    }
                    //    else
                    //    {
                    //        SelectedTask.DownTasksChildren((ProductionTask)SelectedTask.Parent);
                    //        SelectedTask.Parent.Children.Insert(SelectedTask.data.LineOrder, pTask);
                    //    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Не удалось импортировать данные");
                    //}
                }catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        #endregion

        #region ColorSelectionChanged

        public ICommand ColorSelectionChanged { get; }

        private bool CanColorSelectionChangedExecute(object p) => SelectedTask != null;

        private void OnColorSelectionChangedExecuted(object p)
        {
            if(SelectedTask != null && SelectedTask.data!=null)
            {
                TasksOperations.SetColor(SelectedTask.data.Id, SelectedTask.data.Color);
            }
            else
            {
                MessageBox.Show("Не удалось выполнить операцию. Строка не выбрана или данные отсутствуют");
            }
            
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
                Model = ProductionTask.InitModel(Unit.Tasks.Get().ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка. Обновите базу");
                _log.Error("UNKNOWN | ProductionPlanControlViewModel::UpdateModelCommand | " + ex.GetType().Name + " | " + ex.Message);
            }
        }

        private bool ExpandTask(Guid id)
        {
            var task = Model.FlatModel.Cast<ProductionTask>().Where(t => t.data.Id == id).FirstOrDefault();
            if (task != null)
            {
                if (task.IsExpanded == false)
                {
                    task.IsExpanded = true;
                    task.LoadChildren();
                }
                    
                return true;
            }
            return false;
        }

        #endregion
        public void SetSelectedTaskFromReport(object sender, TaskIdEventArgs e)
        {
            var parents = TasksOperations.GetAllParents(e.Id);
            ProductionTask task = Model.FlatModel.Cast<ProductionTask>().Where(t => t.data.Id == e.Id).FirstOrDefault();
            if (task != null)
            {
                SelectedTask = task;
            }
            else
            {
                int parentCount = parents.Count();
                for(int i = 0; i < parentCount; i++)
                {
                    var parent = parents.ElementAt(i);
                    if (parent.Position != 1)
                    {
                        var ltask = Model.FlatModel.Cast<ProductionTask>().Where(t => t.data.Id == parent.Id).FirstOrDefault();
                        if (!ExpandTask(parent.Id))
                        {
                            MessageBox.Show("Ошибка при попытке перехода к задаче.");
                            return;
                        }
                    }
                }
                task = Model.FlatModel.Cast<ProductionTask>().Where(t => t.data.Id == e.Id).FirstOrDefault();
                if(task == null)
                {
                    MessageBox.Show("Не удалось найти задачу");
                    return;
                }
                SelectedTask = task;
            }

            DataTable.UpdateLayout();
            DataTable.ScrollIntoView(SelectedTask);   
        }

        public ProductionPlanControlViewModel()
        {
            #region Команды
            AddProductionTaskCommand = new LambdaCommand(OnAddProductionTaskCommandExecuted, CanAddProductionTaskCommandExecute);
            AddChildCommand = new LambdaCommand(OnAddChildCommandExecuted, CanAddChildCommandExecute);
            DeleteProductionTaskCommand = new LambdaCommand(OnDeleteProductionTaskCommandExecuted, CanDeleteProductionTaskCommandExecute);
            RowEditingEndingCommand = new LambdaCommand(OnRowEditingEndingCommandExecuted, CanRowEditingEndingCommandExecute);
            LevelUpCommand = new LambdaCommand(OnLevelUpCommandExecuted, CanLevelUpCommandExecute);
            LevelDownCommand = new LambdaCommand(OnLevelDownCommandExecuted, CanLevelDownCommandExecute);
            UpdateModelCommand = new LambdaCommand(OnUpdateModelCommandExecuted, CanUpdateModelCommandExecute);
            CopyTaskCommand = new LambdaCommand(OnCopyTaskCommandExecuted, CanCopyTaskCommandExecute);
            PasteTaskCommand = new LambdaCommand(OnPasteTaskCommandExecuted, CanPasteTaskCommandExecute);
            OnCollapsingCommand = new LambdaCommand(OnOnCollapsingCommandExecuted, CanOnCollapsingCommandExecute);
            OnExpandingCommand = new LambdaCommand(OnOnExpandingCommandExecuted, CanOnExpandingCommandExecute);
            UpTaskCommand = new LambdaCommand(OnUpTaskCommandExecuted, CanUpTaskCommandExecute);
            DownTaskCommand = new LambdaCommand(OnDownTaskCommandExecuted, CanDownTaskCommandExecute);
            SetBoldCommand = new LambdaCommand(OnSetBoldCommandExecuted, CanSetBoldCommandExecute);
            OpenPaymentWindowCommand = new LambdaCommand(OnOpenPaymentWindowCommandExecuted, CanOpenPaymentWindowCommandExecute);
            OpenLaborCostsWindowCommand = new LambdaCommand(OnOpenLaborCostsWindowCommandExecuted, CanOpenLaborCostsWindowCommandExecute);
            OpenDocumentWindowCommand = new LambdaCommand(OnOpenDocumentWindowCommandExecuted, CanOpenDocumentWindowCommandExecute);
            OpenComplectationWindowCommand = new LambdaCommand(OnOpenComplectationWindowCommandExecuted, CanOpenComplectationWindowCommandExecute);
            OpenGivingWindowCommand = new LambdaCommand(OnOpenGivingWindowCommandExecuted, CanOpenGivingWindowCommandExecute);
            OpenManufactureWindowCommand = new LambdaCommand(OnOpenManufactureWindowCommandExecuted, CanOpenManufactureWindowCommandExecute);
            OpenInProductionWindowCommand = new LambdaCommand(OnOpenInProductionWindowCommandExecuted, CanOpenInProductionWindowCommandExecute);
            SelectionChangedCommand = new LambdaCommand(OnSelectionChangedCommandExecuted, CanSelectionChangedCommandExecute);
            OpenTimedGivingWindowCommand = new LambdaCommand(OnOpenTimedGivingWindowCommandExecuted, CanOpenTimedGivingWindowCommandExecute);
            GetDatagrid = new LambdaCommand(OnGetDatagridExecuted, CanGetDatagridExecute);
            OpenActWindowCommand = new LambdaCommand(OnOpenActWindowCommandExecuted, CanOpenActWindowCommandExecute);
            OpenSearchWindowCommand = new LambdaCommand(OnOpenSearchWindowCommandExecuted, CanOpenSearchWindowCommandExecute);
            ExportTaskCommand = new LambdaCommand(OnExportTaskCommandExecuted, CanExportTaskCommandExecute);
            ImportTaskCommand = new LambdaCommand(OnImportTaskCommandExecuted, CanImportTaskCommandExecute);
            SetFontSize = new LambdaCommand(OnSetFontSizeCommandExecuted, CanSetFontSizeCommandExecute);
            SetCurrentElementFontSize = new LambdaCommand(OnSetCurrentElementFontSizeCommandExecuted, CanSetCurrentElementFontSizeCommandExecute);
            ColorSelectionChanged = new LambdaCommand(OnColorSelectionChangedExecuted, CanColorSelectionChangedExecute);
            OpenSearchWindowFromShortcutCommand = new LambdaCommand(OnOpenSearchWindowFromShortcutCommandExecuted, CanOpenSearchWindowFromShortcutCommandExecute);
            #endregion

            FontSizes = new List<int> {10, 12, 14, 16, 18, 20, 22 };
            FontFamilies = new List<string> { "Arial", "Calibre", "Times New Roman" };
            User = UserDataSingleton.GetInstance().user;

            //Model = ProductionTask.InitModel(ProductionTasks);
            //Process.Start("explorer.exe", "G:\\4_Sem\\ПЗ.docx");
            //Process.Start("explorer.exe", "G:\\4_Sem\\CP_2021\\CP_2021\\bin\\Debug\\net5.0-windows\\Report.pdf");

            Model = ProductionTask.InitRootsModel();

            var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            string filepath = Directory.GetCurrentDirectory() + "\\Data\\Configs\\log4net.config";
            XmlConfigurator.Configure(logRepo, new FileInfo(filepath));
        }
    }
}

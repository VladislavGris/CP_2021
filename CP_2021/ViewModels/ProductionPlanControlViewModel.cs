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
        // TODO: Возможно стоит убрать
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
            ProductionTaskDB dbTask = new ProductionTaskDB("Новая задача");
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
            ProductionTaskDB dbTask = new ProductionTaskDB("Новая задача");
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
            ProductionTask parent = (ProductionTask)SelectedTask.Parent;
            SelectedTask.Remove(Unit);
            Unit.Commit();
            if(parent == null)
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
            ProductionTaskDB dbTask = new ProductionTaskDB("Новая задача");
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
            //TaskToCopy.AddTasksToDatabase(Unit,Model, (ProductionTask)SelectedTask.Parent);
            //Unit.Commit();
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

            #endregion

            User = user;
            Unit = unit;
            ProductionTasks = Unit.Tasks.Get().ToList();
            Model = ProductionTask.InitModel(ProductionTasks);
        }
    }
}

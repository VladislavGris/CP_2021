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

        private bool CanExpandAllCommandExecute(object p) => true;

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

        private bool CanRollUpAllCommandExecute(object p) => true;

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
            ProductionTaskDB dbTask = new ProductionTaskDB("Новая задача", SelectedTask?.Task.ParentId);
            Unit.Tasks.Insert(dbTask);
            Unit.Commit();
            ProductionTasks = Unit.Tasks.Get().ToList();
            if (SelectedTask?.Task.ParentId == null || SelectedTask == null)
            {
                SelectedTask = ProductionTask.AddRoot(Model, dbTask);
            }
            else
            {
                SelectedTask = ((ProductionTask)SelectedTask.Parent).AddChildren(dbTask);
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

        #region AddChildCommand

        public ICommand AddChildCommand { get; }

        private bool CanAddChildCommandExecute(object p) => SelectedTask != null;

        private void OnAddChildCommandExecuted(object p)
        {
            ProductionTaskDB dbTask = new ProductionTaskDB("Новая задача", SelectedTask.Task.Id);
            var guid = Guid.NewGuid();
            Unit.Tasks.Insert(dbTask);
            Unit.Commit();
            ProductionTasks = Unit.Tasks.Get().ToList();
            ProductionTask task = new ProductionTask(dbTask);
            SelectedTask.Children.Add(task);
            task.Parent.HasChildren = true;
            task.Parent.IsExpanded = true;
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
            if (parent!=null)
            {
                SelectedTask = parent;
            }
            else if(Model.Count!=0)
            {
                SelectedTask = (ProductionTask)Model.Last();
            }
            else
            {
                SelectedTask = null;
            }
            Unit.Commit();
            if (parent?.Children.Count == 0)
            {
                parent.HasChildren = false;
                parent.IsExpanded = false;
            }
        }

        #endregion

        #region LevelUpCommand

        public ICommand LevelUpCommand { get; }

        private bool CanLevelUpCommandExecute(object p) => SelectedTask != null && SelectedTask?.Task.ParentId != null;

        private void OnLevelUpCommandExecuted(object p)
        {
            SelectedTask.Task.ParentId = ((ProductionTask)SelectedTask.Parent).Task.ParentId;
            Unit.Tasks.Update(SelectedTask.Task);
            Unit.Commit();

            SelectedTask.IsExpanded = false;
            ProductionTask task = SelectedTask.Clone();
            ProductionTask parent = (ProductionTask)SelectedTask.Parent;
            if(((ProductionTask)SelectedTask.Parent).Task.ParentId == null)
            {
                SelectedTask.Parent.Children.Remove(SelectedTask);
                Model.Add(task);
            }
            else
            {
                SelectedTask.Parent.Children.Remove(SelectedTask);
                SelectedTask.Parent.Parent.Children.Add(task);
            }
            if (parent.Children.Count == 0)
            {
                parent.HasChildren = false;
                parent.IsExpanded = false;
            }
        }

        #endregion

        #region LevelDownCommand

        public ICommand LevelDownCommand { get; }

        private bool CanLevelDownCommandExecute(object p) => SelectedTask != null;

        private void OnLevelDownCommandExecuted(object p)
        {
            ProductionTaskDB dbTask = new ProductionTaskDB("Новая задача", SelectedTask.Task.ParentId);
            Unit.Tasks.Insert(dbTask);
            Unit.Commit();

            SelectedTask.Task.ParentId = dbTask.Id;
            Unit.Tasks.Update(SelectedTask.Task);
            Unit.Commit();

            ProductionTask task = new ProductionTask(dbTask);
            ProductionTask childTask = SelectedTask.Clone();

            if (dbTask.ParentId == null)
            {
                Model.Add(task);
                Model.Remove(SelectedTask);
            }
            else
            {
                SelectedTask.Parent.Children.Add(task);
                SelectedTask.Parent.Children.Remove(SelectedTask);
            }
            task.Children.Add(childTask);
            task.HasChildren = true;
            task.IsExpanded = false;
        }

        #endregion

        #region UpdateModelCommand

        public ICommand UpdateModelCommand { get; }

        private bool CanUpdateModelCommandExecute(object p) => true;

        private void OnUpdateModelCommandExecuted(object p)
        {
            ProductionTasks = Unit.Tasks.Get().ToList();
            InitModel();
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
            TaskToCopy.AddTasksToDatabase(Unit, (ProductionTask)SelectedTask.Parent);
            Unit.Commit();
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

        private void InitModel()
        {
            Model = new TreeGridModel();

            foreach(ProductionTaskDB p in ProductionTasks)
            {
                // Выборка корневых элементов
                if(p.ParentId == null)
                {
                    ProductionTask root = new ProductionTask(p);
                    if (root.TaskHasChildren(ProductionTasks))
                    {
                        root.AddChildren(ProductionTasks);
                        //AddChilderen(root);
                    }
                    Model.Add(root);
                }
            }
        }

        #endregion

        public ProductionPlanControlViewModel()
        {
        }

        public ProductionPlanControlViewModel(ApplicationUnit unit)
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

            Unit = unit;
            ProductionTasks = Unit.Tasks.Get().ToList();
            InitModel();
        }
    }
}

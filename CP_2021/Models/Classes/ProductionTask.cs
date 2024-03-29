﻿using Common.Wpf.Data;
using CP_2021.Data;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Units;
using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.DBModels;
using CP_2021.Models.ProcedureResuts.Plan;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CP_2021.Models.Classes
{
    class ProductionTask : TreeGridElement
    {

        public ProductionTaskDB Task { get; set; }
        public Task_Hierarchy_Formatting data{get;set;}

        public ProductionTask() { }

        public ProductionTask(ProductionTaskDB task)
        {
            Task = task;
        }

        public ProductionTask(Task_Hierarchy_Formatting data)
        {
            this.data = data;
            HasChildren = data.ChildrenCount > 0;
        }

        /// <summary>
        /// Функция получает все задачи верхнего уровня(parentId = NULL) из БД и формирует модель 
        /// </summary>
        /// <returns>Модель, заполненная задачами верхнего уровня</returns>
        public static TreeGridModel InitRootsModel()
        {
            TreeGridModel model = new TreeGridModel();
            var tasks = TasksOperations.GetTasksByParentNULL();
            foreach (Task_Hierarchy_Formatting task in tasks)
            {
                ProductionTask root = new ProductionTask(task);
                if (task.ChildrenCount > 0)
                    root.HasChildren = true;
                model.Add(root);
            }
            Debug.WriteLine($"After Model:{DateTime.Now.TimeOfDay}");
            return model;
        }
        /// <summary>
        /// Функция подгрузки дочерних элеметов для задачи
        /// </summary>
        public void LoadChildren()
        {
            var children = TasksOperations.GetTasksByParent(this.data.Id);

            foreach(Task_Hierarchy_Formatting child in children)
            {
                ProductionTask cTask = new ProductionTask(child);
                if (child.ChildrenCount > 0)
                    cTask.HasChildren = true;
                this.Children.Add(cTask);
            }
        }
        /// <summary>
        /// Функция выгрузки дочерних элементов для задачи
        /// </summary>
        public void UnloadChildren()
        {
            this.Children.Clear();
        }
        /// <summary>
        /// Перемещает на 1 позицию ниже все задачи модели, которые имеют LineOrder больше, чем this
        /// </summary>
        /// <param name="model">Иерархическая модель</param>
        public void DownTasksModel(TreeGridModel model)
        {
            foreach(ProductionTask task in model)
            {
                if (task.data.LineOrder > this.data.LineOrder)
                    task.data.LineOrder++;
            }
        }
        /// <summary>
        /// Перемещает на 1 позицию ниже все задачи подзадачи, которые имеют LineOrder больше, чем this
        /// </summary>
        /// <param name="parentTask">Родительская задача</param>
        public void DownTasksChildren(ProductionTask parentTask)
        {
            foreach(ProductionTask child in parentTask.Children)
            {
                if (child.data.LineOrder > this.data.LineOrder)
                    child.data.LineOrder++;
            }
        }
        /// <summary>
        /// Повышает на 1 позицию выше все задачи модели, которые имеют LineOrder больше, чем this
        /// </summary>
        /// <param name="model">Иерархическая модель</param>
        public void UpTasksModel(TreeGridModel model)
        {
            foreach(ProductionTask task in model)
            {
                if (task.data.LineOrder > this.data.LineOrder)
                    task.data.LineOrder--;
            }
        }
        /// <summary>
        /// Повышает на 1 позицию выше все задачи подзадачи, которые имеют LineOrder больше, чем this
        /// </summary>
        /// <param name="parent"></param>
        public void UpTasksChildren(ProductionTask parent)
        {
            foreach (ProductionTask child in parent.Children)
            {
                if (child.data.LineOrder > this.data.LineOrder)
                    child.data.LineOrder--;
            }
        }

        public static TreeGridModel InitModel(List<ProductionTaskDB> tasks)
        {
            TreeGridModel model = new TreeGridModel();
            IEnumerable<ProductionTaskDB> rootTasks = tasks.Where(t => t.MyParent.Parent == null)
                                                            .OrderBy(t => t.MyParent.LineOrder);
            foreach(ProductionTaskDB task in rootTasks)
            {
                ProductionTask root = new ProductionTask(task);
                root.AddChildrenIfHas();
                model.Add(root);
            }

            ExpandItems(model);

            return model;
        }

        public static void ExpandItems(TreeGridModel model)
        {
            foreach(ProductionTask task in model)
            {
                task.IsExpanded = task.Task.Expanded;
                if (task.HasChildren)
                {
                    foreach(ProductionTask child in task.Children)
                    {
                        child.ExpandChild();
                    }
                }
            }
        }

        public void ExpandChild()
        {
            this.IsExpanded = this.Task.Expanded;
            if (this.HasChildren)
            {
                foreach(ProductionTask child in this.Children)
                {
                    child.ExpandChild();
                }
            }
        }

        private void AddChildrenIfHas()
        {
            if (this.Task.ParentTo != null && this.Task.ParentTo?.Count != 0)
            {
                this.HasChildren = true;
                this.AddChildren();
            }
        }

        private void AddChildren()
        {
            var taskChildren = this.Task.ParentTo.OrderBy(t => t.LineOrder);
            foreach(var child in taskChildren)
            {
                ProductionTask cTask = new ProductionTask(child.Child);
                cTask.AddChildrenIfHas();
                this.Children.Add(cTask);
            }
        }

        public static ProductionTask InitTask(ProductionTaskDB dbTask)
        {
            ProductionTask root = new ProductionTask(dbTask);
            if (dbTask.ParentTo != null && dbTask.ParentTo?.Count != 0)
            {
                root.HasChildren = true;
                root.AddChildren();
            }
            return root;
        }

        public ProductionTask AddEmptyChild()
        {
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            ProductionTaskDB dbTask = new ProductionTaskDB("Новое изделие");
            dbTask.MyParent = new HierarchyDB(this.Task, dbTask);
            if(unit.Tasks.Get().Where(t=>t.MyParent.Parent == this.Task).Count() != 0)
            {
                dbTask.MyParent.LineOrder = unit.Tasks.Get().Where(t => t.MyParent.Parent == this.Task).Max(t => t.MyParent.LineOrder) + 1;
            }
            else
            {
                dbTask.MyParent.LineOrder = 1;
            }
            
            ProductionTask task = new ProductionTask(dbTask);
            unit.Tasks.Insert(dbTask);
            unit.Commit();
            this.Children.Add(task);
            this.HasChildren = true;
            return task;
        }

        public ProductionTask AddAtTheSameLevel()
        {
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            ProductionTaskDB dbTask = new ProductionTaskDB("Новое изделие");
            dbTask.MyParent = new HierarchyDB(this.Task.MyParent.Parent, dbTask);
            dbTask.MyParent.LineOrder = this.Task.MyParent.LineOrder + 1;
            ProductionTask task = new ProductionTask(dbTask);
            task.UpOrderBelow();
            unit.Tasks.Insert(dbTask);
            unit.Commit();
            this.Parent.Children.Insert(this.Parent.Children.IndexOf(this) + 1, task);
            return task;
        }

        public ProductionTask AddEmptyRootToModel(TreeGridModel model)
        {
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            ProductionTaskDB dbTask = new ProductionTaskDB("Новое изделие");
            dbTask.MyParent = new HierarchyDB(dbTask);
            dbTask.MyParent.LineOrder = this.Task.MyParent.LineOrder + 1;
            ProductionTask task = new ProductionTask(dbTask);
            task.UpOrderBelow();
            unit.Tasks.Insert(dbTask);
            unit.Commit();
            model.Insert(dbTask.MyParent.LineOrder - 1, task);
            return task;
        }

        public void Remove(TreeGridModel model)
        {
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            ProductionTaskDB dbTask = unit.Tasks.Get().Where(t => t.Id == this.Task.Id).SingleOrDefault();
            dbTask.UpOrderBelow();
            unit.Tasks.Delete(dbTask);
            unit.Commit();

            this.IsExpanded = false;
            if (dbTask.ParentTo != null)
            {
                while(this.Children.LastOrDefault() != null)
                {
                    ((ProductionTask)this.Children.Last()).Remove(model);
                }
            }

            //TODO: Убрать позже после переписи всех функций с обновлением
            if(this.Parent == null)
            {
                model.Remove(this);
            }
            else
            {
                this.Parent.Children.Remove(this);
            }
        }

        public void AddChild(ProductionTask child)
        {
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;

            ProductionTaskDB dbChild = child.Task.Clone();
            dbChild.MyParent = new HierarchyDB(this.Task, dbChild);
            dbChild.MyParent.LineOrder = child.Task.MyParent.LineOrder;
            ProductionTask childToAdd = new ProductionTask(dbChild);

            //this.Children.Add(childToAdd);
            unit.Tasks.Insert(dbChild);
            unit.Commit();

            if (child.HasChildren)
            {
                foreach(ProductionTask item in child.Children)
                {
                    childToAdd.AddChild(item);
                }
            }
        }

        public void CopyChild(ProductionTask child)
        {
            ProductionTaskDB dbChild = child.Task.Clone();
            dbChild.MyParent = new HierarchyDB(this.Task, dbChild);
            dbChild.MyParent.LineOrder = child.Task.MyParent.LineOrder;
            ProductionTask childToAdd = new ProductionTask(dbChild);

            this.Children.Add(childToAdd);

            if (child.HasChildren)
            {
                childToAdd.HasChildren = true;
                foreach(ProductionTask item in child.Children)
                {
                    childToAdd.CopyChild(item);
                }
            }
        }

        public ProductionTask CloneTask()
        {
            ProductionTaskDB dbTask = this.Task.Clone();
            ProductionTask taskToClone = new ProductionTask(dbTask);

            if(this.Parent == null)
            {
                dbTask.MyParent = new HierarchyDB(dbTask);
            }
            else
            {
                dbTask.MyParent = new HierarchyDB(this.Task.MyParent.Parent, dbTask);
            }
            dbTask.MyParent.LineOrder = this.Task.MyParent.LineOrder;

            if (this.HasChildren)
            {
                taskToClone.HasChildren = true;
                foreach (ProductionTask item in this.Children)
                {
                    taskToClone.CopyChild(item);
                }
            }
            return taskToClone;
        }

        public void CheckTaskHasChildren()
        {
            if (this.Children.Count == 0)
            {
                this.HasChildren = false;
                this.IsExpanded = false;
            }
        }

        public void ExpandFromChildToParent()
        {
            if(this.Parent == null)
            {
                return;
            }
            this.Parent.IsExpanded = true;
            ((ProductionTask)(this.Parent)).ExpandFromChildToParent();
        }
        
        public ProductionTask Clone()
        {
            return (ProductionTask) this.MemberwiseClone();
        }

        public void UpOrderBelow()
        {
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            var tasksByParent = unit.Tasks.Get().Where(t => t.MyParent.Parent == this.Task.MyParent.Parent && t != this.Task).OrderBy(t => t.MyParent.LineOrder);
            foreach(var task in tasksByParent)
            {
                if(task.MyParent.LineOrder >= this.Task.MyParent.LineOrder)
                {
                    task.MyParent.LineOrder++;
                }
            }
        }

        public void DownOrderBelow()
        {
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            var tasksByParent = unit.Tasks.Get().Where(t => t.MyParent.Parent == this.Task.MyParent.Parent).OrderBy(t => t.MyParent.LineOrder);
            foreach (var task in tasksByParent)
            {
                if (task.MyParent.LineOrder > this.Task.MyParent.LineOrder)
                {
                    task.MyParent.LineOrder--;
                }
            }
        }

        public static ProductionTask FindByTask(TreeGridModel model, ProductionTaskDB taskToFind)
        {
            ProductionTask task = model.Cast<ProductionTask>().Where(t=>t.Task.Id == taskToFind.Id).FirstOrDefault();
            if(task == null)
            {
                foreach(ProductionTask root in model)
                {
                    if (root.HasChildren)
                    {
                        task = root.CheckTaskForMathch(taskToFind);
                        if (task != null)
                        {
                            break;
                        }
                    }
                }
            }
            return task;
        }

        public ProductionTask CheckTaskForMathch(ProductionTaskDB taskForMathch)
        {
            ProductionTask task = this.Children.Cast<ProductionTask>().Where(t => t.Task.Id == taskForMathch.Id).FirstOrDefault();
            if (task == null)
            {
                foreach(ProductionTask child in this.Children)
                {
                    if (child.HasChildren)
                    {
                        task = child.CheckTaskForMathch(taskForMathch);
                        if (task != null)
                        {
                            break;
                        }
                    }
                }
            }
            return task;
        }
    }
}

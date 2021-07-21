using Common.Wpf.Data;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Units;
using CP_2021.Models.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.Classes
{
    class ProductionTask : TreeGridElement
    {

        public ProductionTaskDB Task { get; set; }

        public ProductionTask() { }

        public ProductionTask(ProductionTaskDB task)
        {
            this.Task = task;
        }

        public static TreeGridModel InitModel(List<ProductionTaskDB> tasks)
        {
            TreeGridModel model = new TreeGridModel();
            IEnumerable<ProductionTaskDB> rootTasks = tasks.Where(t => t.MyParent.Parent == null)
                                                            .OrderBy(t => t.MyParent.LineOrder);
            foreach(ProductionTaskDB task in rootTasks)
            {
                ProductionTask root = new ProductionTask(task);
                root.AddChildrenIfHas(task);
                model.Add(root);
            }
            return model;
        }

        private void AddChildrenIfHas(ProductionTaskDB task)
        {
            if (task.ParentTo != null && task.ParentTo?.Count != 0)
            {
                this.HasChildren = true;
                this.AddChildren(task);
            }
        }

        private void AddChildren(ProductionTaskDB task)
        {
            var taskChildren = task.ParentTo.OrderBy(t => t.LineOrder);
            foreach(var child in taskChildren)
            {
                ProductionTask cTask = new ProductionTask(child.Child);
                cTask.AddChildrenIfHas(child.Child);
                this.Children.Add(cTask);
            }
        }

        public static ProductionTask InitTask(ProductionTaskDB dbTask)
        {
            ProductionTask root = new ProductionTask(dbTask);
            if (dbTask.ParentTo != null && dbTask.ParentTo?.Count != 0)
            {
                root.HasChildren = true;
                root.AddChildren(dbTask);
            }
            return root;
        }

        public ProductionTask AddEmptyChild()
        {
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            ProductionTaskDB dbTask = new ProductionTaskDB("Новое изделие");
            dbTask.MyParent = new HierarchyDB(this.Task, dbTask);
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

            var tasksByParents = unit.Tasks.Get().Where(t => t.MyParent.Parent == this.Task.MyParent.Parent).OrderBy(t => t.MyParent.LineOrder);
            foreach (var pTask in tasksByParents)
            {
                if(pTask.MyParent.LineOrder >= dbTask.MyParent.LineOrder)
                {
                    pTask.MyParent.LineOrder++;
                }
            }
            ProductionTask task = new ProductionTask(dbTask);
            unit.Tasks.Insert(dbTask);
            unit.Commit();
            //this.Parent.Children.Add(task);
            return task;
        }

        public ProductionTask AddEmptyRootToModel(TreeGridModel model)
        {
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            ProductionTaskDB dbTask = new ProductionTaskDB("Новое изделие");
            ProductionTask task = new ProductionTask(dbTask);
            dbTask.MyParent = new HierarchyDB(dbTask);
            dbTask.MyParent.LineOrder = this.Task.MyParent.LineOrder + 1;
            //model.Add(task);
            var tasksWithNullParent = unit.Tasks.Get().Where(t => t.MyParent.Parent == null).OrderBy(t=>t.MyParent.LineOrder);
            foreach(var pTask in tasksWithNullParent)
            {
                if(pTask.MyParent.LineOrder >= dbTask.MyParent.LineOrder)
                {
                    pTask.MyParent.LineOrder++;
                }
            }
            unit.Tasks.Insert(dbTask);
            unit.Commit();
            return task;
        }

        public void Remove()
        {
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            this.IsExpanded = false;
            if(this.Task.ParentTo != null)
            {
                while(this.Children.LastOrDefault() != null)
                {
                    ((ProductionTask)this.Children.Last()).Remove();
                }
            }
            if(this.Parent == null)
            {
                this.Model.Remove(this);
            }
            else
            {
                this.Parent.Children.Remove(this);
            }
            unit.Tasks.Delete(this.Task);
            unit.Commit();
        }

        public void AddTasksToDatabase(ApplicationUnit unit, TreeGridModel model, ProductionTask parent)
        {
            ProductionTaskDB dbTask = this.Task.Clone();
            ProductionTask task = new ProductionTask(dbTask);
            if(parent == null)
            {
                dbTask.MyParent = new HierarchyDB(dbTask);
                Model.Add(task);
            }
            else
            {
                parent.Children.Add(task);
                dbTask.MyParent = new HierarchyDB(parent.Task, dbTask);
            }
            unit.Tasks.Insert(dbTask);
            unit.Commit();
            if (this.HasChildren)
            {
                task.HasChildren = true;
                foreach(ProductionTask child in this.Children)
                {
                    child.AddTasksToDatabase(unit, model, task);
                }
            }
        }

        public void CheckTaskHasChildren()
        {
            if (this.Children.Count == 0)
            {
                this.HasChildren = false;
                this.IsExpanded = false;
            }
        }
        
        public ProductionTask Clone()
        {
            return (ProductionTask) this.MemberwiseClone();
        }
    }
}

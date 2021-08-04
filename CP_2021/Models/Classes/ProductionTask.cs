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
                root.AddChildrenIfHas();
                model.Add(root);
            }
            return model;
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
            model.Insert(dbTask.MyParent.LineOrder - 1, task);
            task.UpOrderBelow();
            unit.Tasks.Insert(dbTask);
            unit.Commit();
            return task;
        }

        public void Remove(TreeGridModel model)
        {
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            this.DownOrderBelow();
            this.IsExpanded = false;
            if(this.Task.ParentTo != null)
            {
                while(this.Children.LastOrDefault() != null)
                {
                    ((ProductionTask)this.Children.Last()).Remove(model);
                }
            }
            if(this.Parent == null)
            {
                model.Remove(this);
            }
            else
            {
                this.Parent.Children.Remove(this);
            }
            unit.Tasks.Delete(unit.Tasks.Get().Where(t => t.Id == this.Task.Id).First());
            unit.Commit();
        }

        public void AddChild(ProductionTask child)
        {
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;

            ProductionTaskDB dbChild = child.Task.Clone();
            dbChild.MyParent = new HierarchyDB(this.Task, dbChild);
            dbChild.MyParent.LineOrder = child.Task.MyParent.LineOrder;
            ProductionTask childToAdd = new ProductionTask(dbChild);

            this.Children.Add(childToAdd);
            unit.Tasks.Insert(dbChild);
            unit.Commit();

            if (child.HasChildren)
            {
                childToAdd.HasChildren = true;
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
    }
}

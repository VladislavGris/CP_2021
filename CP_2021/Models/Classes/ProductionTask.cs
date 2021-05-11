using Common.Wpf.Data;
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
            foreach(ProductionTaskDB task in tasks)
            {
                if(task.MyParent == null)
                {
                    ProductionTask root = new ProductionTask(task);
                    if(task.ParentTo!=null && task.ParentTo?.Count != 0)
                    {
                        root.HasChildren = true;
                        root.AddChildren(task);
                    }
                    model.Add(root);
                }
            }
            return model;
        }

        public static ProductionTask InitTask(ProductionTaskDB dbTask)
        {
            ProductionTask root = new ProductionTask(dbTask);
            if(dbTask.ParentTo!=null && dbTask.ParentTo?.Count != 0)
            {
                root.HasChildren = true;
                root.AddChildren(dbTask);
            }
            return root;
        }

        public void AddChildren(ProductionTaskDB task)
        {
            foreach(var child in task.ParentTo)
            {
                ProductionTask cTask = new ProductionTask(child.Child);
                if(child.Child.ParentTo?.Count != 0 && child.Child.ParentTo != null)
                {
                    cTask.HasChildren = true;
                    cTask.AddChildren(child.Child);
                }
                this.Children.Add(cTask);
            }
        }

        public void Remove(ApplicationUnit unit)
        {
            this.IsExpanded = false;
            if(this.Task.ParentTo != null)
            {
                while(this.Children.LastOrDefault() != null)
                {
                    ((ProductionTask)this.Children.Last()).Remove(unit);
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
        }

        public void AddTasksToDatabase(ApplicationUnit unit, TreeGridModel model, ProductionTask parent)
        {
            ProductionTaskDB dbTask = this.Task.Clone();
            ProductionTask task = new ProductionTask(dbTask);
            if(parent == null)
            {
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

        public ProductionTask Clone()
        {
            return (ProductionTask) this.MemberwiseClone();
        }
    }
}

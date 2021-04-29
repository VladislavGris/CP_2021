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
                    if(task.ParentTo != null)
                    {
                        root.HasChildren = true;
                        root.AddChildren(task);
                    }
                    model.Add(root);
                }
            }
            return model;
        }

        public bool TaskHasChildren(List<ProductionTaskDB> tasks)
        {
            //foreach (ProductionTaskDB t in tasks)
            //{
            //    if (this.Task.Id == t.ParentId)
            //    {
            //        this.HasChildren = true;
            //        return true;
            //    }
            //}
            //this.HasChildren = false;
            //return false;
            var task = tasks.Where(t => t.Id == this.Task.Id);
            if (task.First().ParentTo != null)
                return this.HasChildren = true;
            return this.HasChildren = false;
        }

        public void AddChildren(List<ProductionTaskDB> tasks)
        {
            foreach (ProductionTaskDB p in tasks) 
            {
                if (this.Task.Id == p.MyParent.ParentId)
                {
                    ProductionTask child = new ProductionTask(p);
                    if (child.TaskHasChildren(tasks))
                    {
                        child.AddChildren(tasks);
                    }
                    this.Children.Add(child);
                }
            }
        }

        public void AddChildren(ProductionTaskDB task)
        {
            foreach(var child in task.ParentTo)
            {
                ProductionTask cTask = new ProductionTask(child.Child);
                if(child.Child.ParentTo != null)
                {
                    cTask.HasChildren = true;
                    cTask.AddChildren(child.Child);
                }
                this.Children.Add(cTask);
            }
        }

        //public ProductionTask AddChildren(ProductionTaskDB child)
        //{
        //    ProductionTask task = new ProductionTask(child);
        //    this.Children.Add(task);
        //    return task;
        //}

        public void Remove(ApplicationUnit unit)
        {
            this.IsExpanded = false;
            if (this.HasChildren)
            {
                while (this.Children.LastOrDefault() != null)
                {
                    ((ProductionTask)this.Children.LastOrDefault()).Remove(unit);
                }
            }
            if (this.Parent == null)
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
                //dbTask.ParentId = null;
                model.Add(task);
            }
            else
            {
                //dbTask.ParentId = parent.Task.Id;
                parent.Children.Add(task);
            }
            unit.Tasks.Insert(dbTask);
            unit.Commit();
            if (this.HasChildren)
            {
                task.HasChildren = true;
                foreach(ProductionTask child in this.Children)
                {
                    child.AddTasksToDatabase(unit,model, task);
                }
            }
        }

        public bool IsRootElement(TreeGridModel model)
        {
            foreach(TreeGridElement t in model)
            {
                if (t.Equals(this))
                {
                    return true;
                }
            }
            return false;
        }

        public ProductionTask Clone()
        {
            return (ProductionTask) this.MemberwiseClone();
        }

        public static ProductionTask AddRoot(TreeGridModel model, ProductionTaskDB task)
        {
            ProductionTask newRoot = new ProductionTask(task);
            model.Add(newRoot);
            return newRoot;
        }
    }
}

using Common.Wpf.Data;
using CP_2021.Infrastructure.Units;
using CP_2021.Models.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public bool TaskHasChildren(List<ProductionTaskDB> tasks)
        {
            foreach (ProductionTaskDB t in tasks)
            {
                if (this.Task.Id == t.ParentId)
                {
                    this.HasChildren = true;
                    return true;
                }
            }
            this.HasChildren = false;
            return false;
        }

        public void AddChildren(List<ProductionTaskDB> tasks)
        {
            foreach (ProductionTaskDB p in tasks)
            {
                if (this.Task.Id == p.ParentId)
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

        public ProductionTask AddChildren(ProductionTaskDB child)
        {
            ProductionTask task = new ProductionTask(child);
            this.Children.Add(task);
            return task;
        }

        public void Remove(ApplicationUnit unit)
        {
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

        public static ProductionTask AddRoot(TreeGridModel model, ProductionTaskDB task)
        {
            ProductionTask newRoot = new ProductionTask(task);
            model.Add(newRoot);
            return newRoot;
        }
    }
}

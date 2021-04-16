﻿using Common.Wpf.Data;
using CP_2021.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void AddChildren(ProductionTaskDB child)
        {
            this.Children.Add(new ProductionTask(child));
        }

        public static void AddRoot(TreeGridModel model, ProductionTaskDB task)
        {
            ProductionTask newRoot = new ProductionTask(task);
            model.Add(newRoot);
        }
    }
}

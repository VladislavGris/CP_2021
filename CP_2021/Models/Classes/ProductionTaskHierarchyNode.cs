using CP_2021.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.Classes
{
    class ProductionTaskHierarchyNode
    {
        public ProductionTaskDB Task { get; set; }
        public ObservableCollection<ProductionTaskHierarchyNode> Children { get; set; }

        public ProductionTaskHierarchyNode(ProductionTaskDB task)
        {
            Task = task;
            Children = new ObservableCollection<ProductionTaskHierarchyNode>();
        }

        public void AddChildren(ProductionTaskHierarchyNode node)
        {
            Children.Add(node);
        }

        public void AddChildren(ProductionTaskDB task)
        {
            Children.Add(new ProductionTaskHierarchyNode(task));
        }

        public bool RemoveChildren(ProductionTaskDB task)
        {
            return Children.Remove(new ProductionTaskHierarchyNode(task));
        }

        public bool RemoveChildrenById(int id)
        {
            foreach(ProductionTaskHierarchyNode node in Children)
            {
                if(node.Task.Id == id)
                {
                    Children.Remove(node);
                    return true;
                }
            }
            return false;
        }

        public static bool TaskHasChildren(ProductionTaskDB task, List<ProductionTaskDB> tasks)
        {
            foreach(ProductionTaskDB t in tasks)
            {
                if(t.ParentId == task.Id)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddChildNodes(List<ProductionTaskDB> tasks)
        {
            foreach(ProductionTaskDB task in tasks)
            {
                if(task.ParentId == this.Task.Id)
                {
                    ProductionTaskHierarchyNode childNode = new ProductionTaskHierarchyNode(task);
                    if (TaskHasChildren(task, tasks))
                    {
                        childNode.AddChildNodes(tasks);
                    }
                    this.AddChildren(childNode);
                }
            }
        }
    }
}

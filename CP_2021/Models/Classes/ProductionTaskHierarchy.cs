using CP_2021.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.Classes
{
    class ProductionTaskHierarchy
    {
        public ObservableCollection<ProductionTaskHierarchyNode> Nodes { get; set; }

        public ProductionTaskHierarchy()
        {
            Nodes = new ObservableCollection<ProductionTaskHierarchyNode>();
        }

        public void AddNode(ProductionTaskHierarchyNode node)
        {
            Nodes.Add(node);
        }

        public bool RemoveNode(ProductionTaskHierarchyNode node)
        {
            return Nodes.Remove(node);
        }

        public void AddNodes(List<ProductionTaskDB> tasks)
        {
            foreach(ProductionTaskDB task in tasks)
            {
                if(task.ParentId == null)
                {
                    ProductionTaskHierarchyNode rootNode = new ProductionTaskHierarchyNode(task);
                    if(ProductionTaskHierarchyNode.TaskHasChildren(task, tasks))
                    {
                        rootNode.AddChildNodes(tasks);
                    }
                    AddNode(rootNode);
                }
            }
        }
    }
}

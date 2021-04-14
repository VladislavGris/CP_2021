using CP_2021.Data;
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

namespace CP_2021.ViewModels
{
    class ProductionPlanControlViewModel : ViewModelBase
    {

        #region Свойства

        #region ProductionTasks

        private List<ProductionTaskDB> _ProductionTasks;

        public List<ProductionTaskDB> ProductionTasks
        {
            get => _ProductionTasks;
            set => Set(ref _ProductionTasks, value);
        }

        #endregion

        #region Model

        private TreeGridModel _Model;

        public TreeGridModel Model
        {
            get => _Model;
            set => Set(ref _Model, value);
        }

        #endregion

        #endregion

        #region Команды

        #endregion

        #region Методы

        private void InitEnities()
        {
            using (ProductionDBContext context = new ProductionDBContext())
            {
                ProductionTasks = context.ProductionTasks.
                                    Include(t => t.Complectation).
                                    Include(t=>t.Giving).
                                    Include(t => t.Manufacture).
                                    Include(t => t.InProduction).ToList();
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
                    ProductionTask root = new ProductionTask
                    {
                        Task = p
                    };
                    if (HasChildren(root))
                    {
                        root.HasChildren = true;
                        AddChilderen(root);
                    }
                    Model.Add(root);
                }
            }
        }

        private bool HasChildren(ProductionTask task)
        {
            foreach(ProductionTaskDB t in ProductionTasks)
            {
                if(task.Task.Id == t.ParentId)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddChilderen(ProductionTask task)
        {
            foreach(ProductionTaskDB p in ProductionTasks)
            {
                if(task.Task.Id == p.ParentId)
                {
                    ProductionTask child = new ProductionTask
                    {
                        Task = p
                    };
                    if (HasChildren(child))
                    {
                        child.HasChildren = true;
                        AddChilderen(child);
                    }
                    task.Children.Add(child);
                }
            }
        }

        #endregion

        public ProductionPlanControlViewModel()
        {
            #region Команды

            #endregion
            InitEnities();
            InitModel();
        }
    }
}

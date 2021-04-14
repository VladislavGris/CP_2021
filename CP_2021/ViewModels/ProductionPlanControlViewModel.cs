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

        #region Givings

        private List<GivingDB> _Givings;

        public List<GivingDB> Givings
        {
            get => _Givings;
            set => Set(ref _Givings, value);
        }

        #endregion

        #region Manufactures

        private List<ManufactureDB> _Manufactures;

        public List<ManufactureDB> Manufactures
        {
            get => _Manufactures;
            set => Set(ref _Manufactures, value);
        }

        #endregion

        #region InProductions

        private List<InProductionDB> _InProductions;

        public List<InProductionDB> InProductions
        {
            get => _InProductions;
            set => Set(ref _InProductions, value);
        }

        #endregion

        #region Complectations

        private List<ComplectationDB> _Complectations;

        public List<ComplectationDB> Complectations
        {
            get => _Complectations;
            set => Set(ref _Complectations, value);
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
                        Id = p.Id,
                        IncDoc = p.IncDoc,
                        ManagDoc = p.ManagDoc,
                        Name = p.Name,
                        DetailCount = p.Count,
                        SpecCost = p.SpecCost,
                        VishDate = p.VishDate,
                        RealDate = p.RealDate,
                        ExpendNum = p.ExpendNum,
                        Note = p.Note,
                        ParentId = p.ParentId,
                        Completion = p.Completion,
                        Complectation = p.Complectation,
                        Giving = p.Giving,
                        InProduction = p.InProduction,
                        Manufacture = p.Manufacture
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
                if(task.Id == t.ParentId)
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
                if(task.Id == p.ParentId)
                {
                    ProductionTask child = new ProductionTask
                    {
                        Id = p.Id,
                        IncDoc = p.IncDoc,
                        ManagDoc = p.ManagDoc,
                        Name = p.Name,
                        DetailCount = p.Count,
                        SpecCost = p.SpecCost,
                        VishDate = p.VishDate,
                        RealDate = p.RealDate,
                        ExpendNum = p.ExpendNum,
                        Note = p.Note,
                        ParentId = p.ParentId,
                        Completion = p.Completion,
                        Complectation = p.Complectation,
                        Giving = p.Giving,
                        InProduction = p.InProduction,
                        Manufacture = p.Manufacture
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

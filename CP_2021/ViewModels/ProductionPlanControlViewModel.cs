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
using System.Windows.Input;
using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Units;

namespace CP_2021.ViewModels
{
    class ProductionPlanControlViewModel : ViewModelBase
    {

        #region Свойства

        #region UnitOfWork

        private ProductionTaskUnitOfWork _unit;

        public ProductionTaskUnitOfWork Unit
        {
            get => _unit;
            set => Set(ref _unit, value);
        }

        #endregion

        #region ProductionTasks

        private List<ProductionTaskDB> _productionTasks;

        public List<ProductionTaskDB> ProductionTasks
        {
            get => _productionTasks;
            set => Set(ref _productionTasks, value);
        }

        #endregion

        #region SelectedTask

        private ProductionTask _selectedTask;

        public ProductionTask SelectedTask
        {
            get => _selectedTask;
            set => Set(ref _selectedTask, value);
        }

        #endregion

        #region Model

        private TreeGridModel _model;

        public TreeGridModel Model
        {
            get => _model;
            set => Set(ref _model, value);
        }

        #endregion

        #endregion

        #region Команды

        #region ExpandAllCommand

        public ICommand ExpandAllCommand { get; }

        private bool CanExpandAllCommandExecute(object p) => true;

        private void OnExpandAllCommandExecuted(object p)
        {
            foreach(ProductionTask t in Model)
            {
                Expand(t);
            }
        }

        #endregion

        #region RollUpAllCommand

        public ICommand RollUpAllCommand { get; }

        private bool CanRollUpAllCommandExecute(object p) => true;

        private void OnRollUpAllCommandExecuted(object p)
        {
            foreach (ProductionTask t in Model)
            {
                RollUp(t);
            }
        }

        #endregion
        #endregion

        #region Методы

        private void Expand(ProductionTask task)
        {
            task.IsExpanded = true;
            if (task.HasChildren)
            {
                foreach(ProductionTask t in task.Children)
                {
                    Expand(t);
                }
            }
        }

        private void RollUp(ProductionTask task)
        {
            task.IsExpanded = false;
            if (task.HasChildren)
            {
                foreach (ProductionTask t in task.Children)
                {
                    RollUp(t);
                }
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
                    ProductionTask root = new ProductionTask(p);
                    if (root.TaskHasChildren(ProductionTasks))
                    {
                        root.AddChildren(ProductionTasks);
                        //AddChilderen(root);
                    }
                    Model.Add(root);
                }
            }
        }

        #endregion

        public ProductionPlanControlViewModel()
        {
            #region Команды

            ExpandAllCommand = new LambdaCommand(OnExpandAllCommandExecuted, CanExpandAllCommandExecute);
            RollUpAllCommand = new LambdaCommand(OnRollUpAllCommandExecuted, CanRollUpAllCommandExecute);

            #endregion
            Unit = new ProductionTaskUnitOfWork(new ApplicationContext());
            ProductionTasks = Unit.Tasks.Get().ToList();
            InitModel();
        }
    }
}

using Common.Wpf.Data;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.UndoRedo.UndoCommands.Base;
using CP_2021.Infrastructure.Units;
using CP_2021.Models.Classes;
using CP_2021.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.UndoRedo.UndoCommands
{
    class AddNewRootCommand : IUndoRedoCommand
    {
        private ProductionTask _task;
        private TreeGridModel _model;
        private ApplicationUnit _unit;

        public AddNewRootCommand(TreeGridModel model, ProductionTask task)
        {
            _model = model;
            _task = task;
            _unit = ApplicationUnitSingleton.GetInstance().dbUnit;
        }

        public void Redo()
        {
            ProductionTaskDB dbTask = _task.Task.Clone();
            ProductionTask task = new ProductionTask(dbTask);
            int taskPosition = _task.Task.MyParent.LineOrder;

            ProductionTask topTask = (ProductionTask)_model.ElementAt(taskPosition - 2);
            topTask.UpOrderBelow();

            dbTask.MyParent = new HierarchyDB(dbTask);
            _model.Insert(taskPosition - 1, task);
            dbTask.MyParent.LineOrder = taskPosition;
            
            _unit.Tasks.Insert(task.Task);
            _unit.Commit();

            if (_task.HasChildren)
            {
                task.HasChildren = true;
                foreach(ProductionTask child in _task.Children)
                {
                    task.AddChild(child);
                }
            }

            _task = task;
        }

        public void Undo()
        {
            _task.Remove(_model);
        }
    }
}

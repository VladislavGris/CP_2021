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
    class RemoveRootCommand : IUndoRedoCommand
    {
        private TreeGridModel _model;
        private ProductionTask _task;
        private ApplicationUnit _unit;

        public RemoveRootCommand(TreeGridModel model, ProductionTask task)
        {
            _model = model;
            _task = task;
            _unit = ApplicationUnitSingleton.GetInstance().dbUnit;
        }

        public void Redo()
        {
            _task.Remove(_model);
        }

        public void Undo()
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
                foreach (ProductionTask child in _task.Children)
                {
                    task.AddChild(child);
                }
            }

            _task = task;
        }
    }
}

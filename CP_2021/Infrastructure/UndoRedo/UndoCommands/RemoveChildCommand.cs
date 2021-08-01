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
    class RemoveChildCommand : IUndoRedoCommand
    {
        private ProductionTask _parent;
        private ProductionTask _task;
        private ApplicationUnit _unit;

        public RemoveChildCommand(ProductionTask parent, ProductionTask task)
        {
            _parent = parent;
            _task = task;
            _unit = ApplicationUnitSingleton.GetInstance().dbUnit;
        }

        public void Redo()
        {
            ProductionTask task = ProductionTask.InitTask(_task.Task);
            _task.Remove(new TreeGridModel());
            _parent.CheckTaskHasChildren();
            _task = task;
        }

        public void Undo()
        {
            ProductionTaskDB dbTask = _task.Task.Clone();
            ProductionTask task = new ProductionTask(dbTask);
            int taskPosition = _task.Task.MyParent.LineOrder;

            if (_parent.Children.Count != 0)
            {
                ProductionTask topTask = new ProductionTask();
                if(taskPosition == 1)
                {
                    topTask = (ProductionTask) _parent.Children[0];
                    topTask.Task.MyParent.LineOrder++;
                }
                else
                {
                    topTask = (ProductionTask)_parent.Children[taskPosition - 2];
                }
                topTask.UpOrderBelow();
            }

            dbTask.MyParent = new HierarchyDB(_parent.Task, dbTask);
            _parent.Children.Insert(taskPosition - 1, task);
            _parent.HasChildren = true;
            dbTask.MyParent.LineOrder = taskPosition;

            _unit.Tasks.Insert(dbTask);
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

using Common.Wpf.Data;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.UndoRedo.UndoCommands.Base;
using CP_2021.Infrastructure.Units;
using CP_2021.Models.Classes;
using CP_2021.Models.DBModels;
using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.UndoRedo.UndoCommands
{
    class LevelDownCommand : IUndoRedoCommand
    {
        private ProductionTask _task;
        private int _baseLineOrder;
        private TreeGridModel _model;
        private ApplicationUnit _unit;

        public LevelDownCommand(TreeGridModel model, ProductionTask task, int level)
        {
            _task = task;
            _model = model;
            _baseLineOrder = level;
            _unit = ApplicationUnitSingleton.GetInstance().dbUnit;
        }

        public void Redo()
        {
            _baseLineOrder = _task.Task.MyParent.LineOrder;

            ProductionTaskDB dbTask = new ProductionTaskDB("Новое изделие");
            ProductionTask task = new ProductionTask(dbTask);

            ProductionTask parent = (ProductionTask)_task.Parent;

            if (parent != null)
            {
                dbTask.MyParent = new HierarchyDB(_task.Task.MyParent.Parent, dbTask);
                _task.Task.MyParent.Parent = dbTask;
                parent.Children.Insert(_baseLineOrder - 1, task);
                parent.Children.Remove(_task);
            }
            else
            {
                dbTask.MyParent = new HierarchyDB(dbTask);
                _task.Task.MyParent = new HierarchyDB(dbTask, _task.Task);
                _model.Insert(_baseLineOrder - 1, task);
                _model.Remove(_task);
            }

            task.Task.MyParent.LineOrder = _baseLineOrder;
            _task.Task.MyParent.LineOrder = 1;

            task.Children.Add(_task);
            task.HasChildren = true;
            task.IsExpanded = true;

            _unit.Tasks.Insert(dbTask);
            _unit.Commit();
        }

        public void Undo()
        {
            ProductionTask baseParent = (ProductionTask)_task.Parent;
            ProductionTask newParent = (ProductionTask)baseParent.Parent;

            _task.IsExpanded = false;
            _task.Task.MyParent.LineOrder = _baseLineOrder;
            baseParent.Children.Remove(_task);

            if(newParent == null)
            {
                _task.Task.MyParent.Parent = null;
                _model.Insert(_baseLineOrder - 1, _task);
                _model.Remove(baseParent);
            }
            else
            {
                _task.Task.MyParent.Parent = newParent.Task;
                newParent.Children.Insert(_baseLineOrder - 1, _task);
                newParent.Children.Remove(baseParent);
                newParent.CheckTaskHasChildren();
            }

            _task.UpOrderBelow();
            _unit.Tasks.Delete(baseParent.Task);
            _unit.Commit();
        }
    }
}

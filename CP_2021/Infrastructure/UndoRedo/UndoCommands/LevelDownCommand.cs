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
        private ProductionTask _topTask;
        private ProductionTask _parent;
        private int _baseLineOrder;
        private TreeGridModel _model;
        private ApplicationUnit _unit;

        public LevelDownCommand(TreeGridModel model, ProductionTask task, ProductionTask topTask, ProductionTask parent,  int level)
        {
            _task = task;
            _topTask = topTask;
            _parent = parent;
            _model = model;
            _baseLineOrder = level;
            _unit = ApplicationUnitSingleton.GetInstance().dbUnit;
        }

        public void Redo()
        {
            _task.DownOrderBelow();
            if(_parent == null)
            {
                _model.Remove(_task);
                
            }
            else
            {
                _parent.Children.Remove(_task);
            }

            _task.Task.MyParent = new HierarchyDB(_topTask.Task, _task.Task);
            _task.Task.MyParent.LineOrder = 1;

            _topTask.Children.Add(_task);
            _topTask.HasChildren = true;
            _topTask.IsExpanded = true;

            _unit.Commit();
        }

        public void Undo()
        {
            ProductionTask taskToDown = new ProductionTask();
            if (_parent != null)
            {
                taskToDown = (ProductionTask)_parent.Children.Cast<ProductionTask>().Where(t => t.Task.MyParent.LineOrder == _baseLineOrder - 1).FirstOrDefault();
            }
            else
            {
                taskToDown = (ProductionTask)_model.Cast<ProductionTask>().Where(t => t.Task.MyParent.LineOrder == _baseLineOrder - 1).FirstOrDefault();
            }
            taskToDown.UpOrderBelow();

            _topTask.Children.Remove(_task);
            _topTask.CheckTaskHasChildren();
            if (_parent != null)
            {
                _parent.Children.Insert(_baseLineOrder - 1, _task);
                _task.Task.MyParent = new HierarchyDB(_parent.Task, _task.Task);
                _parent.HasChildren = true;
            }
            else
            {
                _model.Insert(_baseLineOrder - 1, _task);
                _task.Task.MyParent = new HierarchyDB(_task.Task);
            }

            _task.Task.MyParent.LineOrder = _baseLineOrder;
            _unit.Commit();
        }
    }
}

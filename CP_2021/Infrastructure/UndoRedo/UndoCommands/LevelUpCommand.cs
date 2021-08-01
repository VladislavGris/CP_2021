using Common.Wpf.Data;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.UndoRedo.UndoCommands.Base;
using CP_2021.Infrastructure.Units;
using CP_2021.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.UndoRedo.UndoCommands
{
    class LevelUpCommand : IUndoRedoCommand
    {
        private ProductionTask _task;
        private ProductionTask _baseParent;
        private ApplicationUnit _unit;
        private TreeGridModel _model;
        private int _baseLineOrder;

        public LevelUpCommand(TreeGridModel model, ProductionTask parent, ProductionTask task, int lineOrder)
        {
            _model = model;
            _task = task;
            _baseParent = parent;
            _unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            _baseLineOrder = lineOrder;
        }

        public void Redo()
        {
            ProductionTask newParent = (ProductionTask)_baseParent.Parent;

            _task.IsExpanded = false;
            _baseLineOrder = _task.Task.MyParent.LineOrder;
            _task.DownOrderBelow();

            _baseParent.Children.Remove(_task);

            _task.Task.MyParent.LineOrder = _baseParent.Task.MyParent.LineOrder + 1;

            if (newParent == null)
            {
                _task.Task.MyParent.Parent = null;
                _model.Insert(_task.Task.MyParent.LineOrder - 1, _task);
            }
            else
            {
                _task.Task.MyParent.Parent = newParent.Task;
                _baseParent.Parent.Children.Insert(_task.Task.MyParent.LineOrder - 1, _task);
            }
            _task.UpOrderBelow();
            _baseParent.CheckTaskHasChildren();
            _unit.Commit();
        }

        public void Undo()
        {
            ProductionTask currentParent = (ProductionTask)_task.Parent;

            _task.IsExpanded = false;
            _task.DownOrderBelow();

            //ProductionTask taskCopy = ProductionTask.InitTask(_task.Task);
            _task.Task.MyParent.Parent = _baseParent.Task;
            _task.Task.MyParent.LineOrder = _baseLineOrder;
            if(currentParent == null)
            {
                _model.Remove(_task);
            }
            else
            {
                currentParent.Children.Remove(_task);
            }

            _baseParent.Children.Insert(_baseLineOrder - 1, _task);
            _task.UpOrderBelow();
            _baseParent.CheckTaskHasChildren();
            _baseParent.HasChildren = true;

            _unit.Commit();
        }
    }
}

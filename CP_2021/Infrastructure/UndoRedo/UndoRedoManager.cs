using CP_2021.Infrastructure.UndoRedo.UndoCommands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.UndoRedo
{
    class UndoRedoManager
    {
        private Stack<IUndoRedoCommand> _undoCommands = new Stack<IUndoRedoCommand>();
        private Stack<IUndoRedoCommand> _redoCommands = new Stack<IUndoRedoCommand>();

        public void AddUndoCommand(IUndoRedoCommand undoCommand)
        {
            if(_undoCommands.Count != 0)
            {
                _undoCommands.Pop();
            }
            _undoCommands.Push(undoCommand);
            ClearRedoStack();
        }

        public void AddRedoComman(IUndoRedoCommand redoCommand)
        {
            _redoCommands.Push(redoCommand);
        }

        public void ExecuteUndoCommand()
        {
            IUndoRedoCommand undoCommand = _undoCommands.Pop();
            undoCommand.Undo();
            _redoCommands.Push(undoCommand);
        }

        public void ExecuteRedoCommand()
        {
            IUndoRedoCommand redoCommand = _redoCommands.Pop();
            redoCommand.Redo();
            _undoCommands.Push(redoCommand);
        }

        public bool UndoStackEmpty()
        {
            return !(_undoCommands.Count == 0);
        }

        public bool RedoStackEmpty()
        {
            return !(_redoCommands.Count == 0);
        }

        public void ClearRedoStack()
        {
            _redoCommands.Clear();
        }
    }
}

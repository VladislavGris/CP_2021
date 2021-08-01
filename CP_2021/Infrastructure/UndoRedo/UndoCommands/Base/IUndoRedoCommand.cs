using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.UndoRedo.UndoCommands.Base
{
    interface IUndoRedoCommand
    {
        void Undo();
        void Redo();
    }
}

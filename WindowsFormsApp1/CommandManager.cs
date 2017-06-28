using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interp2D
{
    class CommandManager
    {
        private Stack<ICommand> undoCommands = new Stack<ICommand>();
        private Stack<ICommand> redoCommands = new Stack<ICommand>();
        

        public void ExecuteCommand(ICommand cmd)
        {
            cmd.Execute();
            undoCommands.Push(cmd);
        }
        public void Undo()
        {
            if(undoCommands.Count != 0)
            {
                ICommand command = redoCommands.Pop();
                command.UnExecute();
                redoCommands.Push(command);
            }
        }
        public void Redo()
        {
            if(redoCommands.Count != 0)
            {
                ICommand command = undoCommands.Pop();
                command.Execute();
                undoCommands.Push(command);
            }
        }
    }
}

/*
*	FILE			:	BoundingCommand.cs
*	PROJECT			:	PROG2111 - Relational Databases
*	PROGRAMMER		:	Amy Dayasundara
*	FIRST VERSION	:	2019 - 11 - 30
*	DESCRIPTION		:	
*	                    This code is taken from an online source which assist the
*	                    buttons to do their command.
*/
#region Systems
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
#endregion Systems

namespace adayasundara_RD_A03.Utilities
{
    /*
     * NAME: Bounding command
     * 
     * PURPOSE: Bounds the values of the commands to set to execution
     * 
     */

    public class BoundingCommand<T> : ICommand
    {
        private readonly Action<T> _execute = null;
        private readonly Func<T, bool> _canExecute = null;

        /// <summary>
        /// N/A
        /// </summary>
        /// <param name="execute"></param> <b>Action</b> - N/A
        /// <param name="canExecute"></param> <b>Func</b> - N/A
        public BoundingCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (_ => true);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => _canExecute((T)parameter);

        public void Execute(object parameter) => _execute((T)parameter);

    }

    /*
     * NAME: Bounding Command
     * 
     * PURPOSE: Object for the command bounding which decides execution
     * 
     */

    public class BoundingCommand : BoundingCommand<object>
    {
        /// <summary>
        /// N/A
        /// </summary>
        /// <param name="execute"></param> <b>Action</b> - N/A
        public BoundingCommand(Action execute)
            : base(_ => execute()) { }

        /// <summary>
        /// N/A
        /// </summary>
        /// <param name="execute"></param> <b>Action</b> - N/A
        /// <param name="canExecute"></param> <b>Func</b> - N/A
        public BoundingCommand(Action execute, Func<bool> canExecute)
            : base(_ => execute(), _ => canExecute()) { }
    }

}

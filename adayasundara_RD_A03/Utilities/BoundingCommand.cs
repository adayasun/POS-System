using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace adayasundara_RD_A03.Utilities
{
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

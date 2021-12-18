using System;
using System.Reflection;
using System.Windows.Input;

namespace be.berghs.nils.EetFestijnLib.Helpers
{
    /// <summary>
    /// Command is a platform agnostic ICommand implementation (although it is currently only tested on WPF)
    ///     
    /// 
    /// </summary>
    public class Command : ICommand
    {
        private readonly Action<object> _execute = null;
        private readonly Func<object, bool> _canExecute = null;

        #region Constructors

        public Command(Action execute):this(o=> execute())
        {

        }

        public Command(Action execute, Func<bool> canExecute) : this(o => execute(), o=> canExecute())
        {

        }

        public Command(Action<object> execute): this(execute, null)
        {
        }

        public Command(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute != null ? _canExecute(parameter) : true;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }

        public void ChangeCanExecute()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        #endregion

    }
}
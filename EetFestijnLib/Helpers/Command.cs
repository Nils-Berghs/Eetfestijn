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
        private Action Action { get; }
        private Func<bool> CanExecuteAction { get; }

        #region Constructors

        public Command(Action action):this(action, null)
        {
            
        }

        public Command(Action action, Func<bool> canExecute) 
        {
            Action = action ?? throw new ArgumentNullException("Action can not be null");
            CanExecuteAction = canExecute;
        }

        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return CanExecuteAction != null ? CanExecuteAction() : true;
        }

        public void Execute(object parameter)
        {
            Action();
        }

        public void ChangeCanExecute()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        #endregion

    }

    public class Command<T>:ICommand
    {
        private Action<T> Action { get; }
        private Func<T,bool> CanExecuteAction { get; }

        #region Constructors

        public Command(Action<T> action) : this(action, null)
        {

        }

        public Command(Action<T> action, Func<T,bool> canExecute)
        {
            Action = action ?? throw new ArgumentNullException("Action can not be null");
            CanExecuteAction = canExecute;
        }

        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (CanExecuteAction == null)
                return true;
            if (parameter == null)
                return CanExecuteAction(default);
            else if (parameter is T t) //if parameter is T or subclass Of T
                return CanExecuteAction(t);
            else
            {
                try
                {
                    //If parameters is not of type T, try converting it using I convertible
                    return CanExecuteAction((T)Convert.ChangeType(parameter, typeof(T)));
                }
                catch { }
            }
            return false;
        }

        public void Execute(object parameter)
        {
            if (parameter == null)
                Action(default);
            else if (parameter is T t) //if parameter is T or subclass Of T
                Action(t);
            else
            {
                try
                {
                    //If parameters is not of type T, try converting it using I convertible
                    Action((T)Convert.ChangeType(parameter, typeof(T)));
                }
                catch { }
            }
        }

        public void ChangeCanExecute()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        #endregion
    }
}
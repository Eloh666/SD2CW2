using System;
using System.Windows.Input;

namespace CourseworkTwoMetro.Utils.RelayCommands
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified October 
    /// Proper full implementation of the the RelayCommand class based on the ICommand interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : ICommand
    {

        readonly Action<T> _actionToExecute = null;
        readonly Predicate<T> _canExecute = null;

        // If can "canExecute" is not provided in the constructor, canExecute will return true by default
        public RelayCommand(Action<T> execute) : this(execute, null) {}

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _actionToExecute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            // JetBrains ReSharper suggestions are crazy... also this line returns true if the parameter is null
            // otherwise it will invoke the _canExecute with its parameter that will return a true or a false.
            return _canExecute?.Invoke((T)parameter) ?? true;
        }

        // event handler for the CanExecuteChanged
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // invokes the provided method
        public void Execute(object parameter)
        {
            _actionToExecute((T)parameter);
        }
    }
}
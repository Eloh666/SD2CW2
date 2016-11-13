using System;
using System.Windows.Input;

namespace CourseworkOneMetro.ViewModels.Utils
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified October 
    /// Zero parameters implementation of ar RelayCommand.
    /// </summary>
    public class LightRelayCommand : ICommand
    {

        readonly Action _actionToExecute;

        // initialises the Action
        public LightRelayCommand(Action execute)
        {
            this._actionToExecute = execute;
        }


        public bool CanExecute(object parameter)
        {
            // in this light, parameterless version the CanExecute always returns true
            return true;
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
            _actionToExecute();
        }
    }
}
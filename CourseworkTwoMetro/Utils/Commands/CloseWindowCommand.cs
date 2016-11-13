using System.Windows;
using CourseworkTwoMetro.Utils.RelayCommands;

// Created by Davide Morello
// Last Modified October 
// simple class relay command implementation of a close window event handler
namespace CourseworkTwoMetro.Utils.Commands
{
    public class CloseWindowCommand
    {
        public RelayCommand<Window> CloseWindow { get; private set; }

        public CloseWindowCommand()
        {
            this.CloseWindow = new RelayCommand<Window>(this.CloseWindowRequest);
        }

        // if the window is not null, it gets closed (null propagation)
        private void CloseWindowRequest(Window window)
        {
            window?.Close();
        }
    }
}
using CourseworkTwoMetro.Managers;

namespace CourseworkTwoMetro.ViewModels
{
    public class MainViewModel
    {   
        // Commands manager library
        public CommandsManager Commands { get; }
        // Windows manager library
        public WindowsManager Windows { get; }
        // The view model for the login window
        public LoginViewModel LoginViewModel { get; }
        // The view model for the main window, gets created when the login is successful
        public MainWindowViewModel MainWindowViewModel { get; set; }

        public MainViewModel()
        {   
            // initialises the commands singleton
            this.Commands = CommandsManager.Instance(this);
            // initialises the windows manager singleton
            this.Windows = WindowsManager.Instance(this);
            // holds the reference to the login view model
            this.LoginViewModel = new LoginViewModel();
        }

    }
}
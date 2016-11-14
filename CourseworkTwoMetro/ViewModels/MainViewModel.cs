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
        // The view model for the main window
        public MainWindowViewModel MainWindowViewModel { get; set; }

        public MainViewModel()
        {
            this.Commands = new CommandsManager(this);
            this.Windows = new WindowsManager(this);
            this.LoginViewModel = new LoginViewModel();
        }

    }
}
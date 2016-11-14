using CourseworkTwoMetro.Managers;

namespace CourseworkTwoMetro.ViewModels
{
    public class MainViewModel
    {
        public CommandsManager Commands { get; }
        public WindowsManager Windows { get; }
        public LoginViewModel LoginViewModel { get; }

        public MainViewModel()
        {
            this.Commands = new CommandsManager(this);
            this.Windows = new WindowsManager(this);
            this.LoginViewModel = new LoginViewModel();
        }

    }
}
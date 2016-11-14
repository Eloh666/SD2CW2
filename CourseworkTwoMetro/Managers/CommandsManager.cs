using System;
using System.Threading.Tasks;
using System.Windows;
using CourseworkTwoMetro.Utils.API;
using CourseworkTwoMetro.Utils.RelayCommands;
using CourseworkTwoMetro.ViewModels;
using CourseworkTwoMetro.Views;

namespace CourseworkTwoMetro.Managers
{
    public class CommandsManager
    {
        private static CommandsManager _instance;
        private readonly MainViewModel _mainViewModel;
        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        public RelayCommand<LoginWindow> LoginCommand { get; private set; }

        // constructor that sets a reference to the mainViewModel and sets up the various relay commands
        private CommandsManager(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel;
            this.LoginCommand = new RelayCommand<LoginWindow>(Login);
            this.CloseWindowCommand = new RelayCommand<Window>(this.CloseWindow);
        }

        // instance method for the singleton class
        public static CommandsManager Instance(MainViewModel mainWinReference)
        {
            return _instance ?? (_instance = new CommandsManager(mainWinReference));
        }

        // login method for the login command
        private async void Login(LoginWindow loginWindow)
        {
            //LoginViewModel loginModel = _mainViewModel.LoginViewModel;
            //if (loginModel.IsUserValid)
            //{
                //loginModel.Loading = true;
                //bool loginIsSuccessful = await ApiFacade.Login(loginModel.User);
                //loginModel.Loading = false;
                //loginModel.LoginFailed = !loginIsSuccessful;
                //if (loginIsSuccessful)
                //{
                    this._mainViewModel.Windows.CreateMainWindow();
                    loginWindow.Close();
            //    }
            //};
        }

        // close window command for the close window command
        private void CloseWindow(Window window)
        {
            window?.Close();
        }
    }
}
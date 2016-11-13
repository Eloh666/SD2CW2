using System;
using System.Threading.Tasks;
using CourseworkTwoMetro.Utils.API;
using CourseworkTwoMetro.Utils.RelayCommands;
using CourseworkTwoMetro.ViewModels;

namespace CourseworkTwoMetro.Managers
{
    public class CommandsManager
    {
            public RelayCommand<LoginViewModel> LoginCommand { get; private set; }
            public CommandsManager()
            {
                LoginCommand = new RelayCommand<LoginViewModel>(Login);
            }

            public async void Login(LoginViewModel loginModel)
            {
                if (loginModel.IsUserValid)
                {
                    await ApiFacade.Login(loginModel.User);
                }
            }
    }
}
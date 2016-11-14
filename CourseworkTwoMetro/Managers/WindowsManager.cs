using System.Windows;
using CourseworkTwoMetro.Utils.RelayCommands;
using CourseworkTwoMetro.ViewModels;
using CourseworkTwoMetro.Views;

namespace CourseworkTwoMetro.Managers
{
    public class WindowsManager
    {
        private MainViewModel _mainViewModel;
        public object CreateMainWindowCommand { get; }

        public WindowsManager(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel;
            CreateMainWindowCommand = new LightRelayCommand(CreateMainWindow);
        }

        public void CreateMainWindow()
        {

            MainWindow mainWindow = new MainWindow {DataContext = new MainWindowViewModel()};
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }
    }
}
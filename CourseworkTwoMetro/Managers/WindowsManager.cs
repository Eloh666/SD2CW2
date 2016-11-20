using System.Windows;
using CourseworkTwoMetro.Utils.RelayCommands;
using CourseworkTwoMetro.ViewModels;
using CourseworkTwoMetro.Views;

namespace CourseworkTwoMetro.Managers
{
    public class WindowsManager
    {
        private static WindowsManager _instance;
        private readonly MainViewModel _mainViewModel;
        public LightRelayCommand NewCustomerCommand { get; private set; }
        public LightRelayCommand NewBookingCommand { get; private set; }

        private WindowsManager(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel;
            this.NewCustomerCommand = new LightRelayCommand(this.NewCustomer);
            this.NewBookingCommand = new LightRelayCommand(this.NewBooking);
        }

        public static WindowsManager Instance(MainViewModel mainWinReference)
        {
            return _instance ?? (_instance = new WindowsManager(mainWinReference));
        }

        public void CreateMainWindow()
        {
            this._mainViewModel.MainWindowViewModel = new MainWindowViewModel();
            MainWindow mainWindow = new MainWindow {DataContext = this._mainViewModel};
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }

        public void NewCustomer()
        {
            CustomerEdit customerEditWindow = new CustomerEdit {DataContext = new EditCustomerViewModel("Add new customer", null)};
            customerEditWindow.ShowDialog();
        }

        public void NewBooking()
        {
            BookingEdit customerEditWindow = new BookingEdit { DataContext = new BookingViewModel(this._mainViewModel) };
            customerEditWindow.ShowDialog();
        }
    }
}
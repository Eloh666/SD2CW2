using System.Windows;
using CourseworkTwoMetro.Utils.API;
using CourseworkTwoMetro.Utils.RelayCommands;
using CourseworkTwoMetro.ViewModels;
using CourseworkTwoMetro.Views;
using MahApps.Metro.Controls.Dialogs;

namespace CourseworkTwoMetro.Managers
{
    public class WindowsManager
    {
        private static WindowsManager _instance;
        private readonly MainViewModel _mainViewModel;
        public RelayCommand<MainWindowViewModel> NewCustomerCommand { get; private set; }
        public RelayCommand<MainWindowViewModel> NewBookingCommand { get; private set; }

        public RelayCommand<MainWindowViewModel> EditMainTabItemCommand { get; private set; }
        public RelayCommand<MainWindowViewModel> DeleteMainTabItemCommand { get; private set; }

        public RelayCommand<BookingViewModel> NewGuestCommand { get; private set; }
        public RelayCommand<BookingViewModel> EditGuestCommand { get; private set; }
        public RelayCommand<BookingViewModel> DeleteGuestCommand { get; private set; }

        private readonly IDialogCoordinator _dialogCoordinator;

        private WindowsManager(MainViewModel mainViewModel)
        {
            this._dialogCoordinator = DialogCoordinator.Instance;
            this._mainViewModel = mainViewModel;
            this.NewCustomerCommand = new RelayCommand<MainWindowViewModel>(this.NewCustomer);
            this.NewBookingCommand = new RelayCommand<MainWindowViewModel>(this.NewBooking);
            this.NewGuestCommand = new RelayCommand<BookingViewModel>(this.NewGuest);

            this.EditMainTabItemCommand = new RelayCommand<MainWindowViewModel>(this.EditMainTabItem, this.CanModifyItem);
            this.DeleteMainTabItemCommand = new RelayCommand<MainWindowViewModel>(this.DeleteMainTabItem, this.CanModifyItem);

            this.EditGuestCommand = new RelayCommand<BookingViewModel>(this.EditGuest, this.CanModifyGuest);
            this.DeleteGuestCommand = new RelayCommand<BookingViewModel>(this.DeleteGuest, this.CanModifyGuest);
        }

        public static WindowsManager Instance(MainViewModel mainWinReference)
        {
            return _instance ?? (_instance = new WindowsManager(mainWinReference));
        }

        public void CreateMainWindow()
        {
            this._mainViewModel.MainWindowViewModel = new MainWindowViewModel(this._mainViewModel);
            MainWindow mainWindow = new MainWindow { DataContext = this._mainViewModel };
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }

        private void NewCustomer(MainWindowViewModel mainWindowViewModel)
        {
            CustomerEdit customerEditWindow = new CustomerEdit { DataContext = new EditCustomerViewModel("Add new customer", null) };
            customerEditWindow.ShowDialog();
        }

        private void NewBooking(MainWindowViewModel mainWindowViewModel)
        {
            BookingEdit customerEditWindow = new BookingEdit { DataContext = new BookingViewModel("Add new booking", this._mainViewModel) };
            customerEditWindow.ShowDialog();
        }

        private void EditMainTabItem(MainWindowViewModel mainWindowViewModel)
        {
            switch (mainWindowViewModel.SelectedTabNumber)
            {
                case 0:
                    {
                        BookingEdit customerEditWindow = new BookingEdit { DataContext = new BookingViewModel("Edit booking", this._mainViewModel, mainWindowViewModel.SelectedBooking) };
                        customerEditWindow.ShowDialog();
                    }
                    break;
                case 1:
                default:
                    {
                        CustomerEdit customerEditWindow = new CustomerEdit { DataContext = new EditCustomerViewModel("Edit customer", mainWindowViewModel.SelectedCustomer) };
                        customerEditWindow.ShowDialog();
                    }
                    break;
            }
        }

        private bool CanModifyItem(MainWindowViewModel mainWindowViewModel)
        {
            if (mainWindowViewModel == null)
            {
                return false;
            }
            switch (mainWindowViewModel.SelectedTabNumber)
            {
                case 0:
                    {
                        return mainWindowViewModel.SelectedBooking != null;
                    }
                default:
                    {
                        return mainWindowViewModel.SelectedCustomer != null;
                    }
            }
        }

        private void DeleteMainTabItem(MainWindowViewModel mainWindowViewModel)
        {
            switch (mainWindowViewModel.SelectedTabNumber)
            {
                case 0:
                    {
                        DeleteConfirmationDialog deleteDialog = new DeleteConfirmationDialog { DataContext = new DeleteDialogViewModel("Delete Booking", this._mainViewModel) };
                        deleteDialog.ShowDialog();
                    }
                    break;
                case 1:
                default:
                    {
                        DeleteConfirmationDialog deleteDialog = new DeleteConfirmationDialog { DataContext = new DeleteDialogViewModel("Delete Customer", this._mainViewModel) };
                        deleteDialog.ShowDialog();
                    }
                    break;
            }
        }

        private bool CanModifyGuest(BookingViewModel bookingViewModel)
        {
            return bookingViewModel?.SelectedGuest != null;
        }

        private void NewGuest(BookingViewModel bookingViewModel)
        {
            GuestEdit guestEdit = new GuestEdit { DataContext = new EditGuestViewModel("Add new guest", bookingViewModel.SelectedGuest) };
            guestEdit.ShowDialog();
        }

        private void EditGuest(BookingViewModel bookingViewModel)
        {
            GuestEdit guestEdit = new GuestEdit { DataContext = new EditGuestViewModel("Edit guest", bookingViewModel.SelectedGuest) };
            guestEdit.ShowDialog();
        }

        private void DeleteGuest(BookingViewModel bookingViewModel)
        {
            DeleteConfirmationDialog deleteDialog = new DeleteConfirmationDialog { DataContext = new DeleteDialogViewModel("Remove Guest", this._mainViewModel) };
            deleteDialog.ShowDialog();
        }
    }
}
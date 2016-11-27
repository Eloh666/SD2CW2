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

        public RelayCommand<EditBookingViewModel> NewGuestCommand { get; private set; }
        public RelayCommand<EditBookingViewModel> EditGuestCommand { get; private set; }
        public RelayCommand<EditBookingViewModel> DeleteGuestCommand { get; private set; }

        private readonly IDialogCoordinator _dialogCoordinator;

        private WindowsManager(MainViewModel mainViewModel)
        {
            this._dialogCoordinator = DialogCoordinator.Instance;
            this._mainViewModel = mainViewModel;
            this.NewCustomerCommand = new RelayCommand<MainWindowViewModel>(this.NewCustomer);
            this.NewBookingCommand = new RelayCommand<MainWindowViewModel>(this.NewBooking);
            this.NewGuestCommand = new RelayCommand<EditBookingViewModel>(this.NewGuest, this.CanAddGuest);

            this.EditMainTabItemCommand = new RelayCommand<MainWindowViewModel>(this.EditMainTabItem, this.CanModifyItem);
            this.DeleteMainTabItemCommand = new RelayCommand<MainWindowViewModel>(this.DeleteMainTabItem, this.CanModifyItem);

            this.EditGuestCommand = new RelayCommand<EditBookingViewModel>(this.EditGuest, this.CanModifyGuest);
            this.DeleteGuestCommand = new RelayCommand<EditBookingViewModel>(this.DeleteGuest, this.CanModifyGuest);
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
            CustomerEdit customerEditWindow = new CustomerEdit { DataContext = new EditCustomerViewModel("Add new customer", this._mainViewModel, null) };
            customerEditWindow.ShowDialog();
        }

        private void NewBooking(MainWindowViewModel mainWindowViewModel)
        {
            BookingEdit customerEditWindow = new BookingEdit { DataContext = new EditBookingViewModel("Add new booking", this._mainViewModel) };
            customerEditWindow.ShowDialog();
        }

        private void EditMainTabItem(MainWindowViewModel mainWindowViewModel)
        {
            switch (mainWindowViewModel.SelectedTabNumber)
            {
                case 0:
                    {
                        BookingEdit customerEditWindow = new BookingEdit { DataContext = new EditBookingViewModel("Edit booking", this._mainViewModel, mainWindowViewModel.BookingViewModel) };
                        customerEditWindow.ShowDialog();
                    }
                    break;
                case 1:
                default:
                    {
                        CustomerEdit customerEditWindow = new CustomerEdit { DataContext = new EditCustomerViewModel("Edit customer", this._mainViewModel, mainWindowViewModel.SelectedCustomer) };
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

        private bool CanModifyGuest(EditBookingViewModel bookingViewModel)
        {
            return bookingViewModel?.SelectedGuest != null;
        }

        private void NewGuest(EditBookingViewModel bookingViewModel)
        {
            GuestEdit guestEdit = new GuestEdit { DataContext = new EditGuestViewModel("Add new guest", this._mainViewModel, bookingViewModel.NewBooking.Guests) };
            guestEdit.ShowDialog();
        }

        private void EditGuest(EditBookingViewModel bookingViewModel)
        {
            GuestEdit guestEdit = new GuestEdit { DataContext = new EditGuestViewModel("Edit guest", this._mainViewModel, bookingViewModel.NewBooking.Guests, bookingViewModel.SelectedGuest) };
            guestEdit.ShowDialog();
        }

        private void DeleteGuest(EditBookingViewModel bookingViewModel)
        {
            DeleteConfirmationDialog deleteDialog = new DeleteConfirmationDialog { DataContext = new DeleteDialogViewModel("Remove Guest", this._mainViewModel) };
            deleteDialog.ShowDialog();
        }

        private bool CanAddGuest(EditBookingViewModel bookingViewModel)
        {
            return bookingViewModel?.NewBooking != null && bookingViewModel.NewBooking.Guests.Count < 4;
        }
    }
}
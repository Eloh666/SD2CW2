using System.Windows;
using CourseworkTwoMetro.Utils.API;
using CourseworkTwoMetro.Utils.RelayCommands;
using CourseworkTwoMetro.ViewModels;
using CourseworkTwoMetro.Views;
using MahApps.Metro.Controls.Dialogs;

namespace CourseworkTwoMetro.Managers
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// General windows manager singleton
    /// requires a reference to a min view model object and returns an istance of a library of commands
    /// all the commands use a RelayCommand implementation that relies on the Command pattern
    /// this commands are specific to open windows
    /// </summary>
    public class WindowsManager
    {
        // instance of this object for the singleton pattern
        private static WindowsManager _instance;
        // reference to the main view model
        private readonly MainViewModel _mainViewModel;
        // opens a window to add a new customer
        public RelayCommand<MainWindowViewModel> NewCustomerCommand { get; private set; }
        // opens a window to add a new booking
        public RelayCommand<MainWindowViewModel> NewBookingCommand { get; private set; }

        // opens a window to edit a table item (booking or customer)
        public RelayCommand<MainWindowViewModel> EditMainTabItemCommand { get; private set; }
        // opens a window to delete a table item (booking or customer)
        public RelayCommand<MainWindowViewModel> DeleteMainTabItemCommand { get; private set; }

        // opens a window to add a guest
        public RelayCommand<EditBookingViewModel> NewGuestCommand { get; private set; }
        // opens a window to edit a guest
        public RelayCommand<EditBookingViewModel> EditGuestCommand { get; private set; }
        // opens a window to read the booking invoice
        public RelayCommand<MainWindowViewModel> OpenInvoiceWindowCommand { get; private set; }

        // reference to the dialog coordinator singleton
        private readonly IDialogCoordinator _dialogCoordinator;

        // constructor that sets a reference to the mainViewModel and sets up the various relay commands
        private WindowsManager(MainViewModel mainViewModel)
        {
            this._dialogCoordinator = DialogCoordinator.Instance;
            this._mainViewModel = mainViewModel;
            this.NewCustomerCommand = new RelayCommand<MainWindowViewModel>(this.NewCustomer);
            this.NewBookingCommand = new RelayCommand<MainWindowViewModel>(this.NewBooking);
            this.NewGuestCommand = new RelayCommand<EditBookingViewModel>(this.NewGuest, this.CanAddGuest);

            this.EditMainTabItemCommand = new RelayCommand<MainWindowViewModel>(this.EditMainTabItem, this.CanModifyItem);
            this.DeleteMainTabItemCommand = new RelayCommand<MainWindowViewModel>(this.DeleteMainTabItem, this.CanModifyItem);

            this.OpenInvoiceWindowCommand = new RelayCommand<MainWindowViewModel>(this.OpenInvoiceWindow, this.CanModifyItem);

            this.EditGuestCommand = new RelayCommand<EditBookingViewModel>(this.EditGuest, this.CanModifyGuest);
        }

        // instance method for the singleton pattern
        public static WindowsManager Instance(MainViewModel mainWinReference)
        {
            return _instance ?? (_instance = new WindowsManager(mainWinReference));
        }

        // creates the main window
        public void CreateMainWindow()
        {
            // creates a main window view model
            this._mainViewModel.MainWindowViewModel = new MainWindowViewModel(this._mainViewModel);
            // creates the main window and initalises its data context
            MainWindow mainWindow = new MainWindow { DataContext = this._mainViewModel };
            // sets the main window reference to the current one and shows it
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }

        // creates a customer edit window in add mode and shows it as a dialog
        private void NewCustomer(MainWindowViewModel mainWindowViewModel)
        {
            CustomerEdit customerEditWindow = new CustomerEdit { DataContext = new EditCustomerViewModel("Add new customer", this._mainViewModel, null) };
            customerEditWindow.ShowDialog();
        }

        // creates a booking edit wizard window in add mode and shows it as a dialog
        private void NewBooking(MainWindowViewModel mainWindowViewModel)
        {
            BookingEdit customerEditWindow = new BookingEdit { DataContext = new EditBookingViewModel("Add new booking", this._mainViewModel) };
            customerEditWindow.ShowDialog();
        }

        // creates a customer/booking edit window in edit mode, changes based on the state of the tab being selected
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

        // second callback for the commands pattern, check whther or not it is possible to edit an item
        // it is only active if the item itself can be selected
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

        // opens the delete dialog on a object based on the status of the item currently under selection in the main window
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

        // second callback for the commands pattern, check whether or not it is possible to edit a guest
        // it is only active if the item itself can be selected
        private bool CanModifyGuest(EditBookingViewModel bookingViewModel)
        {
            return bookingViewModel?.SelectedGuest != null;
        }

        // creates a guest guest edit windows in add mode
        private void NewGuest(EditBookingViewModel bookingViewModel)
        {
            GuestEdit guestEdit = new GuestEdit { DataContext = new EditGuestViewModel("Add new guest", this._mainViewModel, bookingViewModel.NewBooking.Guests) };
            guestEdit.ShowDialog();
        }

        // creates a guest guest edit windows in edit mode
        private void EditGuest(EditBookingViewModel bookingViewModel)
        {
            GuestEdit guestEdit = new GuestEdit { DataContext = new EditGuestViewModel("Edit guest", this._mainViewModel, bookingViewModel.NewBooking.Guests, bookingViewModel.SelectedGuest) };
            guestEdit.ShowDialog();
        }

        // second callback for the commands pattern, check whether or not it is possible to add a guest
        private bool CanAddGuest(EditBookingViewModel bookingViewModel)
        {
            return bookingViewModel?.NewBooking != null && bookingViewModel.NewBooking.Guests.Count < 4;
        }

        // opens the invoice window
        private void OpenInvoiceWindow(MainWindowViewModel mainWindowViewModel)
        {
            var invoiceViewModel = new InvoiceViewModel(this._mainViewModel, mainWindowViewModel.BookingViewModel, mainWindowViewModel.Customers);
            var invoiceWindow = new InvoiceWindow {DataContext = invoiceViewModel};
            invoiceWindow.ShowDialog();
        }

    }
}
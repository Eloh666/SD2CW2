using System;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CourseworkTwoMetro.Models;
using CourseworkTwoMetro.Models.Extras;
using CourseworkTwoMetro.Utils.API;
using CourseworkTwoMetro.Utils.RelayCommands;
using CourseworkTwoMetro.ViewModels;
using CourseworkTwoMetro.ViewModels.Utils;
using CourseworkTwoMetro.Views;
using MahApps.Metro.Controls.Dialogs;


namespace CourseworkTwoMetro.Managers
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// General commands singleton
    /// requires a reference to a min view model object and returns an istance of a library of commands
    /// all the commands use a RelayCommand implementation that relies on the Command pattern
    /// </summary>
    public class CommandsManager
    {
        // instance of this object for the singleton pattern
        private static CommandsManager _instance;
        // reference to the main view model
        private readonly MainViewModel _mainViewModel;
        // command to close a a window
        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        // command that handles the login of a user
        public RelayCommand<LoginWindow> LoginCommand { get; private set; }
        // command that handles saving a customer
        public RelayCommand<EditCustomerViewModel> SubmitCustomerCommand { get; private set; }
        // command that handles saving a booking
        public RelayCommand<EditBookingViewModel> SubmitBookingCommand { get; private set; }
        // command that handles adding a guest to a customer
        public RelayCommand<EditGuestViewModel> SubmitGuestCommand { get; private set; }
        // command that handles deleting a booking or a customer
        public RelayCommand<DeleteDialogViewModel> DeleteSelectedMainWindowItemCommand { get; private set; }
        // command that handles removing a guest from a booking
        public RelayCommand<EditBookingViewModel> SubmitDeleteGuestCommand { get; private set; }
        // command that handles refreshing the lists of customers and booking
        public RelayCommand<MainWindowViewModel> RefreshListsCommand { get; private set; }
        // commands that updates a booking to display its invoice on the main window
        public RelayCommand<MainWindowViewModel> UpdateSelectedBookingViewModelCommand { get; private set; }
        // toggles a breakfast object from a menu
        public RelayCommand<EditBookingViewModel> ToggleBreakfastCommand { get; private set; }
        // toggles a dinner object from a menu
        public RelayCommand<EditBookingViewModel> ToggleDinnerCommand { get; private set; }
        // toggles a car hire from a booking
        public RelayCommand<EditBookingViewModel> ToggleCarHireCommand { get; private set; }

        // reference to the dialog coordinator singleton
        private readonly IDialogCoordinator _dialogCoordinator;

        // constructor that sets a reference to the mainViewModel and sets up the various relay commands
        private CommandsManager(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel;
            this._dialogCoordinator = DialogCoordinator.Instance;
            this.LoginCommand = new RelayCommand<LoginWindow>(Login);
            this.CloseWindowCommand = new RelayCommand<Window>(this.CloseWindow);
            this.RefreshListsCommand = new RelayCommand<MainWindowViewModel>(this.RefreshLists);

            this.SubmitCustomerCommand = new RelayCommand<EditCustomerViewModel>(this.SubmitCustomer);
            this.SubmitBookingCommand = new RelayCommand<EditBookingViewModel>(this.SubmitBooking);
            this.SubmitGuestCommand = new RelayCommand<EditGuestViewModel>(this.SubmitGuest);

            this.DeleteSelectedMainWindowItemCommand = new RelayCommand<DeleteDialogViewModel>(this.DeleteSelectedMainWindowItem);
            this.UpdateSelectedBookingViewModelCommand = new RelayCommand<MainWindowViewModel>(this.UpdateSelectedBookingViewModel);

            this.SubmitDeleteGuestCommand = new RelayCommand<EditBookingViewModel>(this.SubmitDeleteGuest, this.CanDeleteGuest);

            this.ToggleBreakfastCommand = new RelayCommand<EditBookingViewModel>(this.ToggleBreakfast);
            this.ToggleDinnerCommand = new RelayCommand<EditBookingViewModel>(this.ToggleDinner);
            this.ToggleCarHireCommand = new RelayCommand<EditBookingViewModel>(this.ToggleCarHire);
        }

        // instance method for the singleton pattern
        public static CommandsManager Instance(MainViewModel mainWinReference)
        {
            return _instance ?? (_instance = new CommandsManager(mainWinReference));
        }

        // simple function that uses the API to refresh the lists of customers adn bookings
        // if an exception is thrown, the error message is displayed
        public async void RefreshLists(MainWindowViewModel mainWindowViewModel)
        {
            // sets display flags
            mainWindowViewModel.Loading = true;
            mainWindowViewModel.LoadingFailed = false;
            try
            {
                // awaits on the async calls
                mainWindowViewModel.Customers = await ApiFacade.GetCustomers();
                mainWindowViewModel.Bookings = await ApiFacade.GetBookings();
                mainWindowViewModel.LoadingFailed = false;
            }
            // catches issues and resets the flags at the end
            catch
            {
                mainWindowViewModel.LoadingFailed = true;
            }
            finally
            {
                mainWindowViewModel.Loading = false;
            }
        }

        // adds the selected booking from the data table in the middle and displays its
        // information as invoice on the left
        private void UpdateSelectedBookingViewModel(MainWindowViewModel mainWindowViewModel)
        {
            var selectedBooking = mainWindowViewModel.SelectedBooking;
            if (selectedBooking == null)
            {
                mainWindowViewModel.BookingViewModel = null;
            }
            else if (mainWindowViewModel.BookingViewModel == null)
            {
                mainWindowViewModel.BookingViewModel = new BookingViewModel(selectedBooking);
            }
            else
            {
                mainWindowViewModel.BookingViewModel.Booking = selectedBooking;
            }
        }

        // logs the user in when the login button is clicked
        private async void Login(LoginWindow loginWindow)
        {
            LoginViewModel loginModel = _mainViewModel.LoginViewModel;
            // if the user is valid sets the lags
            if (loginModel.IsUserValid)
            {
                loginModel.Loading = true;
                // awaits on the API facade async login
                bool loginIsSuccessful = await ApiFacade.Login(loginModel.User);
                // if the login is successful the user is logged in, else the lags are displayed
                loginModel.Loading = false;
                loginModel.LoginFailed = !loginIsSuccessful;
                // if the login is sets the user to a new main window by invoking the createMainWindow command
                // from the windows manager and closes the login one
                if (loginIsSuccessful)
                {
                    this._mainViewModel.Windows.CreateMainWindow();
                    loginWindow.Close();
                }
            }
            // otherwise a simple warning is displayed
            else
            {
                await this._dialogCoordinator.ShowMessageAsync(loginModel,
                    "Data Invalid", "Please make sure all the fields are properly filled and validated.");
            }
        }

        // close window command for the close window command
        private void CloseWindow(Window window)
        {
            window?.Close();
        }

        // fairly complex method that relies on the API facade to edit or add a customer based on the state of the view model
        private async void SubmitCustomer(EditCustomerViewModel editCustomerViewModel)
        {
            // returns an error if the customer is not valid/validated
            if (!editCustomerViewModel.CustomerViewModel.IsCustomerValid)
            {
                await this._dialogCoordinator.ShowMessageAsync(editCustomerViewModel, "Data Invalid", "Please make sure all the fields are properly filled.");
                return;
            }
            try
            {
                // awaits on the API facade trying to save the customer
                editCustomerViewModel.Loading = true;
                var updatedCustomer =
                    await
                        ApiFacade.SaveCustomer(editCustomerViewModel.CustomerViewModel.Customer,
                            editCustomerViewModel.ExistingCustomer);
                var customerList = this._mainViewModel.MainWindowViewModel.Customers;
                // if we are adding a customer (ExistingCustome flag NOT set in the viewmodel)
                // the customer is added the the list and a helpful message is displayed
                if (!editCustomerViewModel.ExistingCustomer)
                {
                    customerList.Add(updatedCustomer);
                    // sets the window to editing mode
                    editCustomerViewModel.ExistingCustomer = true;
                    editCustomerViewModel.CustomerViewModel.Customer = updatedCustomer;
                    editCustomerViewModel.Title = "Edit Customer";
                    await this._dialogCoordinator.ShowMessageAsync(editCustomerViewModel, "Added", "The customer has been successfully added.");
                }
                // if the flag "existingCustomer" is set in the viewModel, said customer is replaced with its updated version
                // at the end a message is displayed
                else
                {
                    for (int i = 0; i < customerList.Count; i++)
                    {
                        if (customerList[i].ReferenceNumber == updatedCustomer.ReferenceNumber)
                        {
                            customerList[i] = updatedCustomer;
                            await this._dialogCoordinator.ShowMessageAsync(editCustomerViewModel, "Edited", "The customer has been successfully edited.");
                            break;
                        }
                    }
                }
                // switches flags off accordingly
                editCustomerViewModel.Loading = false;
                editCustomerViewModel.LoadingSuccess = true;
                editCustomerViewModel.LoadingFailed = false;
            }
            catch
            {
                editCustomerViewModel.Loading = false;
                editCustomerViewModel.LoadingSuccess = false;
                editCustomerViewModel.LoadingFailed = true;
                await this._dialogCoordinator.ShowMessageAsync(editCustomerViewModel, "Delete Failed", "Something went wrong in trying to delete the item.");
            }
        }

        // deletes an item from the main window, booking or customer
        private async void DeleteSelectedMainWindowItem(DeleteDialogViewModel deleteDialogViewModel)
        {
            // selects the current delete dialog (there can be only one open a time)
            var mainWindowViewModel = this._mainViewModel.MainWindowViewModel;
            Window currentDialog = Application.Current.Windows.OfType<DeleteConfirmationDialog>().SingleOrDefault(w => w.IsActive);
            try
            {
                // sets flags
                deleteDialogViewModel.Loading = true;
                var deleted = false;
                // based on the tab from which we are deleting
                switch (mainWindowViewModel.SelectedTabNumber)
                {
                    case 0:
                        {   
                            // deletes a booking awaiting on the API call
                            var selectedBooking = mainWindowViewModel.SelectedBooking;
                            deleted = await ApiFacade.DeleteBooking(selectedBooking);
                            if (deleted)
                            {
                                var bookingsList = this._mainViewModel.MainWindowViewModel.Bookings;
                                for (int i = 0; i < bookingsList.Count; i++)
                                {
                                    // and updates the bookings
                                    if (bookingsList[i].Id == selectedBooking.Id)
                                    {
                                        bookingsList.RemoveAt(i);
                                    }
                                }
                            }
                        }
                        break;
                    case 1:
                    default:
                        {
                            // deletes a customer awaiting on the API
                            var selectedCustomer = mainWindowViewModel.SelectedCustomer;
                            deleted = await ApiFacade.DeleteCustomer(selectedCustomer);
                            if (deleted)
                            {
                                // removes it from the list
                                var customerList = this._mainViewModel.MainWindowViewModel.Customers;
                                for (int i = 0; i < customerList.Count; i++)
                                {
                                    if (customerList[i].ReferenceNumber == selectedCustomer.ReferenceNumber)
                                    {
                                        customerList.RemoveAt(i);
                                    }
                                }
                            }
                        }
                        break;
                }
                // closes the current dialog
                await _dialogCoordinator.ShowMessageAsync(deleteDialogViewModel, "Delete", "Item deleted successfully.");
                this.CloseWindow(currentDialog);
            }
            catch (Exception)
            {
                // if something goes wrong, a helpful message is showin and the window kept open
                await _dialogCoordinator.ShowMessageAsync(deleteDialogViewModel, "Delete Failed", "Something went wrong in trying to delete the item.");
            }
            deleteDialogViewModel.Loading = false;
        }

        // toggles the breakfast on/off on the current booking
        private void ToggleBreakfast(EditBookingViewModel bookingViewModel)
        {
            // sets the status
            bool status = bookingViewModel.BreakfastSwitch;
            // calls the method to disable this extra based on the status
            this.ToggleExtra(bookingViewModel.NewBooking, "Breakfast", status);
        }

        // toggles the Dinner on/off on the current booking
        private void ToggleDinner(EditBookingViewModel bookingViewModel)
        {
            // sets the status
            bool status = bookingViewModel.DinnerSwitch;
            // calls the method to disable this extra based on the status
            this.ToggleExtra(bookingViewModel.NewBooking, "Dinner", status);
        }

        // toggles the CarHire on/off on the current booking
        private void ToggleCarHire(EditBookingViewModel bookingViewModel)
        {
            // sets the status
            bool status = bookingViewModel.CarHireSwitch;
            // calls the method to disable this extra based on the status
            this.ToggleExtra(bookingViewModel.NewBooking, "CarHire", status);
        }

        // toggles the status of an extra and sets it to null if previously on,
        // or to on by calling the ExtrasFactory if previously off
        private void ToggleExtra(BookingViewModel bookingViewModel, string extraType, bool turnItOn)
        {
            bookingViewModel.Extras[extraType] = turnItOn ? ExtrasFactory.CreateExtra(extraType) : null;
            bookingViewModel.RefreshView();
        }

        // simple method that tries to post a new customer through the API
        private async Task<Customer> postNewCustomer(ObservableCollection<Customer> customers, Customer customer)
        {
            try
            {
                var updatedCustomer = await ApiFacade.SaveCustomer(customer, false);
                customers.Add(updatedCustomer);
                return updatedCustomer;
            }
            catch
            {
                return null;
            }
        }

        // method that tries to post a new booking through the API
        private async void SubmitBooking(EditBookingViewModel bookingViewModel)
        {
            bookingViewModel.Loading = true;
            bool put = bookingViewModel.OriginalBooking != null;
            bool newCustomer = bookingViewModel.CreateNewCustomer;

            try
            {
                int customerId;
                // used in the booking wizard, if the user has selected a new customer
                if (newCustomer)
                {
                    // asynchronously posts the customer through the API
                    var customerAdded =
                        await this.postNewCustomer(bookingViewModel.Customers, bookingViewModel.NewCustomer.Customer);
                    // throws an error if things go wrong
                    if (customerAdded == null)
                    {
                        bookingViewModel.LoadingSuccess = false;
                        bookingViewModel.LoadingFailed = true;
                        return;
                    }
                    customerId = customerAdded.ReferenceNumber;
                }
                else
                {
                    customerId = bookingViewModel.ExistingCustomer.ReferenceNumber;
                }
                // add the customer ID to the booking new or existing
                var bookingToSave = bookingViewModel.NewBooking.Booking;
                bookingToSave.CustomerId = customerId;
                // based on whether we are editing or adding sends a bool flag to the API
                var shouldPutInsteadOfPost = bookingViewModel.OriginalBooking != null;
                Booking updaterdBooking = await ApiFacade.SaveBooking(bookingToSave, shouldPutInsteadOfPost);
                var bookings = bookingViewModel.Bookings;
                // adds the booking if we were working with a new one
                if (!shouldPutInsteadOfPost)
                {
                    bookings.Add(updaterdBooking);
                }
                // else replaces the existing one
                else
                {
                    for (int i = 0; i < bookings.Count; i++)
                    {
                        if (bookings[i] == bookingViewModel.OriginalBooking)
                        {
                            bookings[i] = updaterdBooking;
                            break;
                        }
                    }
                }
                // resolves the status flags accordingly
                bookingViewModel.LoadingSuccess = true;
                bookingViewModel.LoadingFailed = false;
            }
            catch
            {
                // resolves the status flags accordingly
                bookingViewModel.LoadingSuccess = false;
                bookingViewModel.LoadingFailed = true;
            }
            bookingViewModel.Loading = false;
        }

        // adds/edits a guest to an booking
        private void SubmitGuest(EditGuestViewModel editGuestViewModel)
        {
            Guest editedGuest = editGuestViewModel.Guest;
            // errors out if the data being entered is invalid
            if (!editGuestViewModel.IsGuestValid)
            {
                _dialogCoordinator.ShowMessageAsync(editGuestViewModel, "Cannot editd", "Cannot add or edit the current guest. Some of the vields are invalid.");
                return;
            }
            // otherwise the guest object is simply added or replaced in the guest list of the current bookin
            ObservableCollection<Guest> guests = editGuestViewModel.Guests;
            Guest originalGuest = editGuestViewModel.SelectedGuest;
            // replaces the guest if the original guest object is not null
            if (originalGuest != null)
            {
                for (int i = 0; i < guests.Count; i++)
                {
                    if (guests[i] == originalGuest)
                    {
                        guests[i] = editedGuest;
                        break;
                    }
                }
            }
            // else adds it
            else
            {
                editGuestViewModel.AddGuest(editedGuest);
            }
            // selects the current dialog
            Window currentDialog = Application.Current.Windows.OfType<GuestEdit>().SingleOrDefault(w => w.IsActive);
            // selects the current booking window
            var bookingWindow = (PropertyChangedNotifier)Application.Current.Windows.OfType<BookingEdit>().SingleOrDefault(w => !w.IsActive)?.DataContext;
            // refreshes a status calling a wrapper to the property changed notifier
            bookingWindow?.RefreshView();
            // closes the window
            CloseWindow(currentDialog);
        }

        // second arguement that checks wether the delete/edit guest button should be enabled
        // used for the relay command pattern
        private bool CanDeleteGuest(EditBookingViewModel bookingViewModel)
        {
            return bookingViewModel?.SelectedGuest != null;
        }

        // pops the selected guest from the current booking
        private void SubmitDeleteGuest(EditBookingViewModel bookingViewModel)
        {
            var guests = bookingViewModel.NewBooking.Guests;
            for (int i = 0; i < guests.Count; i++)
            {
                if (guests[i] == bookingViewModel.SelectedGuest)
                {
                    bookingViewModel.SelectedGuest = null;
                    guests.RemoveAt(i);
                    break;
                }
            }
            bookingViewModel.RefreshView();
        }
    }
}
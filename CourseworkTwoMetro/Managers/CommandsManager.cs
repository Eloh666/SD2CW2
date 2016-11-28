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
    public class CommandsManager
    {
        private static CommandsManager _instance;
        private readonly MainViewModel _mainViewModel;
        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        public RelayCommand<LoginWindow> LoginCommand { get; private set; }

        public RelayCommand<EditCustomerViewModel> SubmitCustomerCommand { get; private set; }
        public RelayCommand<EditBookingViewModel> SubmitBookingCommand { get; private set; }
        public RelayCommand<EditGuestViewModel> SubmitGuestCommand { get; private set; }

        public RelayCommand<DeleteDialogViewModel> DeleteSelectedMainWindowItemCommand { get; private set; }
        public RelayCommand<EditBookingViewModel> SubmitDeleteGuestCommand { get; private set; }
        public RelayCommand<MainWindowViewModel> RefreshListsCommand { get; private set; }
        public RelayCommand<MainWindowViewModel> UpdateSelectedBookingViewModelCommand { get; private set; }
        public RelayCommand<EditBookingViewModel> ToggleBreakfastCommand { get; private set; }
        public RelayCommand<EditBookingViewModel> ToggleDinnerCommand { get; private set; }
        public RelayCommand<EditBookingViewModel> ToggleCarHireCommand { get; private set; }

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

        public async void RefreshLists(MainWindowViewModel mainWindowViewModel)
        {
            mainWindowViewModel.Loading = true;
            try
            {
                mainWindowViewModel.Customers = await ApiFacade.GetCustomers();
                mainWindowViewModel.Bookings = await ApiFacade.GetBookings();
                mainWindowViewModel.LoadingFailed = false;
            }
            catch
            {
                mainWindowViewModel.LoadingFailed = true;
            }
            finally
            {
                mainWindowViewModel.Loading = false;
            }
        }

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

        private async void SubmitCustomer(EditCustomerViewModel editCustomerViewModel)
        {
            if (!editCustomerViewModel.CustomerViewModel.IsCustomerValid)
            {
                await this._dialogCoordinator.ShowMessageAsync(editCustomerViewModel, "Data Invalid", "Please make sure all the fields are properly filled.");
                return;
            }
            try
            {
                editCustomerViewModel.Loading = true;
                var updatedCustomer =
                    await
                        ApiFacade.SaveCustomer(editCustomerViewModel.CustomerViewModel.Customer,
                            editCustomerViewModel.ExistingCustomer);
                var customerList = this._mainViewModel.MainWindowViewModel.Customers;
                if (!editCustomerViewModel.ExistingCustomer)
                {
                    customerList.Add(updatedCustomer);
                    editCustomerViewModel.ExistingCustomer = true;
                    editCustomerViewModel.CustomerViewModel.Customer = updatedCustomer;
                    editCustomerViewModel.Title = "Edit Customer";
                    await this._dialogCoordinator.ShowMessageAsync(editCustomerViewModel, "Added", "The customer has been successfully added.");
                }
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

        private async void DeleteSelectedMainWindowItem(DeleteDialogViewModel deleteDialogViewModel)
        {
            var mainWindowViewModel = this._mainViewModel.MainWindowViewModel;
            Window currentDialog = Application.Current.Windows.OfType<DeleteConfirmationDialog>().SingleOrDefault(w => w.IsActive);
            try
            {
                deleteDialogViewModel.Loading = true;
                var deleted = false;
                switch (mainWindowViewModel.SelectedTabNumber)
                {
                    case 0:
                        {
                            var selectedBooking = mainWindowViewModel.SelectedBooking;
                            deleted = await ApiFacade.DeleteBooking(selectedBooking);
                            if (deleted)
                            {
                                var bookingsList = this._mainViewModel.MainWindowViewModel.Bookings;
                                for (int i = 0; i < bookingsList.Count; i++)
                                {
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
                            var selectedCustomer = mainWindowViewModel.SelectedCustomer;
                            deleted = await ApiFacade.DeleteCustomer(selectedCustomer);
                            if (deleted)
                            {
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
                await _dialogCoordinator.ShowMessageAsync(deleteDialogViewModel, "Delete", "Item deleted successfully.");
                this.CloseWindow(currentDialog);
            }
            catch (Exception)
            {
                await _dialogCoordinator.ShowMessageAsync(deleteDialogViewModel, "Delete Failed", "Something went wrong in trying to delete the item.");
            }
            deleteDialogViewModel.Loading = false;
        }

        private void ToggleBreakfast(EditBookingViewModel bookingViewModel)
        {
            bool status = bookingViewModel.BreakfastSwitch;
            this.ToggleExtra(bookingViewModel.NewBooking, "Breakfast", status);
        }

        private void ToggleDinner(EditBookingViewModel bookingViewModel)
        {
            bool status = bookingViewModel.DinnerSwitch;
            this.ToggleExtra(bookingViewModel.NewBooking, "Dinner", status);
        }

        private void ToggleCarHire(EditBookingViewModel bookingViewModel)
        {
            bool status = bookingViewModel.CarHireSwitch;
            this.ToggleExtra(bookingViewModel.NewBooking, "CarHire", status);
        }

        private void ToggleExtra(BookingViewModel bookingViewModel, string extraType, bool turnItOn)
        {
            bookingViewModel.Extras[extraType] = turnItOn ? ExtrasFactory.CreateExtra(extraType) : null;
            bookingViewModel.RefreshView();
        }

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


        private async void SubmitBooking(EditBookingViewModel bookingViewModel)
        {
            bookingViewModel.Loading = true;
            bool put = bookingViewModel.OriginalBooking != null;
            bool newCustomer = bookingViewModel.CreateNewCustomer;

            try
            {
                int customerId;
                if (newCustomer)
                {
                    var customerAdded =
                        await this.postNewCustomer(bookingViewModel.Customers, bookingViewModel.NewCustomer.Customer);
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

                var bookingToSave = bookingViewModel.NewBooking.Booking;
                bookingToSave.CustomerId = customerId;
                var shouldPutInsteadOfPost = bookingViewModel.OriginalBooking != null;
                Booking updaterdBooking = await ApiFacade.SaveBooking(bookingToSave, shouldPutInsteadOfPost);
                var bookings = bookingViewModel.Bookings;
                if (!shouldPutInsteadOfPost)
                {
                    bookings.Add(updaterdBooking);
                }
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
                bookingViewModel.LoadingSuccess = true;
                bookingViewModel.LoadingFailed = false;
            }
            catch
            {
                bookingViewModel.LoadingSuccess = false;
                bookingViewModel.LoadingFailed = true;
            }
            bookingViewModel.Loading = false;
        }

        private void SubmitGuest(EditGuestViewModel editGuestViewModel)
        {
            Guest editedGuest = editGuestViewModel.Guest;
            if (!editGuestViewModel.IsGuestValid)
            {
                _dialogCoordinator.ShowMessageAsync(editGuestViewModel, "Cannot editd", "Cannot add or edit the current guest. Some of the vields are invalid.");
                return;
            }
            ObservableCollection<Guest> guests = editGuestViewModel.Guests;
            Guest originalGuest = editGuestViewModel.SelectedGuest;
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
            else
            {
                editGuestViewModel.AddGuest(editedGuest);
            }
            Window currentDialog = Application.Current.Windows.OfType<GuestEdit>().SingleOrDefault(w => w.IsActive);
            var bookingWindow = (PropertyChangedNotifier) Application.Current.Windows.OfType<BookingEdit>().SingleOrDefault(w => !w.IsActive)?.DataContext;
            bookingWindow?.RefreshView();
            CloseWindow(currentDialog);
        }

        private bool CanDeleteGuest(EditBookingViewModel bookingViewModel)
        {
            return bookingViewModel?.SelectedGuest != null;
        }

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
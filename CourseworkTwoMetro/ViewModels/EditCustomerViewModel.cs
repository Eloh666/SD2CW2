using System.Collections.Generic;
using System.ComponentModel;
using CourseworkTwoMetro.Managers;
using CourseworkTwoMetro.Models;

namespace CourseworkTwoMetro.ViewModels
{
    /// <summary>
    /// Created by Davide Morello
    /// Last Modified November
    /// ViewModel for the customer edit view model class, it is used for the edit customer window.
    /// Even if not called so (MVVM conventions) is technically a wrapper/decorator
    /// This is basically a decorator for the the customer decorator.
    /// </summary>
    public class EditCustomerViewModel : FormWithSpinnerViewModel
    {
        private bool _existingCustomer;
        // managers singletons
        public WindowsManager Windows { get; }
        public CommandsManager Commands { get; }

        // title of the window
        public string Title { get; set; }
        // reference to the customer being edited/added and its view model
        public CustomerViewModel CustomerViewModel { get; }
        // sets up the singletons, the title and the flags the status of the customer to new or edited
        public EditCustomerViewModel(string title, MainViewModel mainViewModel, Customer customer = null)
        {
            this.Windows = WindowsManager.Instance(mainViewModel);
            this.Commands = CommandsManager.Instance(mainViewModel);
            this.Title = title;
            CustomerViewModel = new CustomerViewModel(customer ?? new Customer());
            this.ExistingCustomer = customer != null;
        }

        public bool ExistingCustomer
        {
            get { return _existingCustomer; }
            set
            {
                _existingCustomer = value;
                OnPropertyChangedEvent(null);
            }
        }

        // returns the displayed version of the reference, if any
        public string NormalizedReferenceNumber => this.ExistingCustomer ? this.CustomerViewModel.ReferenceNumber.ToString() : "The reference number will be set on save.";
    }
}
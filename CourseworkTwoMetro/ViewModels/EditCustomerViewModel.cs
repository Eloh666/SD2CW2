using System.Collections.Generic;
using System.ComponentModel;
using CourseworkTwoMetro.Managers;
using CourseworkTwoMetro.Models;

namespace CourseworkTwoMetro.ViewModels
{
    public class EditCustomerViewModel : FormWithSpinnerViewModel
    {
        private bool _existingCustomer;
        // managers singletons
        public WindowsManager Windows { get; }
        public CommandsManager Commands { get; }

        public string Title { get; set; }
        public CustomerViewModel CustomerViewModel { get; }
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

        public string NormalizedReferenceNumber => this.ExistingCustomer ? this.CustomerViewModel.ReferenceNumber.ToString() : "The reference number will be set on save.";
    }
}
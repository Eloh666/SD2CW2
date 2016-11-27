﻿using System;
using System.Collections.ObjectModel;
using CourseworkTwoMetro.Managers;
using CourseworkTwoMetro.Models;

namespace CourseworkTwoMetro.ViewModels
{
    public class EditBookingViewModel : FormWithSpinnerViewModel
    {
        public string Title { get; }

        // managers singletons
        public WindowsManager Windows { get; }
        public CommandsManager Commands { get; }

        // customers and bookings lists
        public ObservableCollection<Customer> Customers { get; }
        public ObservableCollection<Booking> Bookings { get; }

        private bool _newCustomerRadio;
        private bool _existingCustomerRadio;

        // wizard page two bindings
        public BookingViewModel NewBooking { get; set; }
        public Guest NewGuest { get; set; }
        public Guest SelectedGuest { get; set; }
        private bool _carHireSwitch;
        private bool _breakfastSwitch;
        private bool _dinnerSwitch;
        private ObservableCollection<Guest> _currentGestsList;
        private Customer _existingCustomer;
        private CustomerViewModel _newCustomer;

        // new booking
        public EditBookingViewModel(string title, MainViewModel mainViewModel, BookingViewModel selectedBooking = null)
        {

            this.NewBooking = selectedBooking ?? new BookingViewModel(new Booking());
            if (selectedBooking != null)
            {
                this.BreakfastSwitch = this.NewBooking.Breakfast != null;
                this.DinnerSwitch = this.NewBooking.Dinner != null;
                this.CarHireSwitch = this.NewBooking.CarHire != null;
            }
            this.Title = title;
            this.Commands = CommandsManager.Instance(mainViewModel);
            this.Windows = WindowsManager.Instance(mainViewModel);
            this.Customers = mainViewModel.MainWindowViewModel.Customers;
            this.Bookings = mainViewModel.MainWindowViewModel.Bookings;
            this.CreateNewCustomer = true;


            this.NewCustomer = new CustomerViewModel(new Customer());
            if (this.NewBooking.CustomerId == 0) return;
            foreach (var customer in Customers)
            {
                if (customer.ReferenceNumber != NewBooking.CustomerId) continue;
                this.ExistingCustomer = customer;
                this._existingCustomerRadio = true;
                break;
            }
            OnPropertyChangedEvent(null);
        }

        public CustomerViewModel NewCustomer
        {
            get { return _newCustomer; }
            set
            {
                _newCustomer = value;
                OnPropertyChangedEvent(null);
            }
        }

        public Customer ExistingCustomer
        {
            get { return _existingCustomer; }
            set
            {
                _existingCustomer = value;
                OnPropertyChangedEvent(null);
            }
        }

        public bool CanMoveToPageTwo => this._existingCustomerRadio ? this.ExistingCustomer != null : this.NewCustomer.IsCustomerValid;
        public bool CanMoveToPageThree { get; }
        public bool CanMoveBackToPageOne { get; }
        public bool CanMoveBackToPageTwo { get; }


        public bool DieteryReqsShow => this.BreakfastSwitch || this.DinnerSwitch;

        public bool CreateNewCustomer
        {
            get { return _newCustomerRadio; }
            set
            {
                _newCustomerRadio = value;
                _existingCustomerRadio = !value;
                OnPropertyChangedEvent(null);
            }
        }

        public bool UseExistingCustomer
        {
            get { return _existingCustomerRadio; }
            set
            {
                _existingCustomerRadio = value;
                _newCustomerRadio = !value;
                OnPropertyChangedEvent(null);
            }
        }

        public bool CarHireSwitch
        {
            get { return _carHireSwitch; }
            set
            {
                _carHireSwitch = value;
                OnPropertyChangedEvent(null);
            }
        }

        public bool BreakfastSwitch
        {
            get { return _breakfastSwitch; }
            set
            {
                _breakfastSwitch = value;
                OnPropertyChangedEvent(null);
            }
        }

        public bool DinnerSwitch
        {
            get { return _dinnerSwitch; }
            set
            {
                _dinnerSwitch = value;
                OnPropertyChangedEvent(null);
            }
        }

        public ObservableCollection<Guest> CurrentGestsList
        {
            get { return _currentGestsList; }
            set
            {
                _currentGestsList = value;
                OnPropertyChangedEvent(null);
            }
        }
    }
}
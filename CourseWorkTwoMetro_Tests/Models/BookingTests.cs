using System;
using CourseworkTwoMetro.Models;
using CourseworkTwoMetro.Models.Extras;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CourseWorkTwoMetro_Tests.Models
{
    [TestClass()]
    public class BookingTests
    {
        [TestMethod()]
        // shoul successully create a booking through the 0 params constructor
        public void ZeroParamsConstructorTest()
        {
            // creates a test booking
            Booking mockBooking = new Booking();
            // the object should not be null
            Assert.IsNotNull(mockBooking);
            // all the extras should be off
            Assert.IsNull(mockBooking.Breakfast);
            Assert.IsNull(mockBooking.CarHire);
            Assert.IsNull(mockBooking.Dinner);
            // the arrival and departure dates should be set to defaults
            Assert.AreEqual(mockBooking.ArrivalDate, DateTime.Today);
            Assert.AreEqual(mockBooking.DepartureDate, DateTime.Today.AddDays(1));
            // the dietary requirements should be empty
            Assert.IsNull(mockBooking.DietaryReqs);
            // no guests should have been added just yet
            Assert.AreEqual(mockBooking.Guests.Count, 0);
        }

        [TestMethod()]
        // shoul successully create a booking and set all the correct values through the constructor
        public void BookingTestFullParamsConstructor()
        {
            DateTime testArrivalDate = DateTime.Today.AddDays(10);
            DateTime testDepartureDate = testArrivalDate.AddDays(10);
            int testCustomerId = 666;
            int testBookingId = 123;

            // creates a test booking
            Booking mockBooking = new Booking(testBookingId, testArrivalDate, testDepartureDate, testCustomerId);
            // the object should not be null
            Assert.IsNotNull(mockBooking);

            // the arrival and departure dates should be set accordingly
            Assert.AreEqual(mockBooking.ArrivalDate, DateTime.Today.AddDays(10));
            Assert.AreEqual(mockBooking.DepartureDate, DateTime.Today.AddDays(20));

            // the customer ID and Booking ID should be properly set
            Assert.AreEqual(mockBooking.CustomerId, 666);
            Assert.AreEqual(mockBooking.Id, 123);
        }

        [TestMethod()]
        // the validation for the arrival date should be successful
        public void ValidateArrivalDateTest()
        {
            Booking mockBooking = new Booking();
            mockBooking.ArrivalDate = DateTime.Today.AddDays(5);
            mockBooking.DepartureDate = DateTime.Today.AddDays(10);

            // if the dates are valid should return null
            Assert.AreEqual(mockBooking.ValidateArrivalDate(), null);

            //should return the appropriate error message if the booking as a whole lasts less than 1 daty
            mockBooking.DepartureDate = mockBooking.DepartureDate.AddDays(-100);
            Assert.AreEqual(mockBooking.ValidateArrivalDate(), "Your booking cannot start after its end");

            // if the booking starts in the past
            mockBooking.DepartureDate = DateTime.Today.AddDays(10);
            mockBooking.ArrivalDate = DateTime.Today.AddDays(-5);
            Assert.AreEqual(mockBooking.ValidateArrivalDate(), "Your booking date cannot start in the past");

            // if the booking lasts a negative amount of days
            mockBooking.ArrivalDate = DateTime.Today.AddDays(30);
            Assert.AreEqual(mockBooking.ValidateArrivalDate(), "Your booking cannot start after its end");
        }

        [TestMethod()]
        // the validation for the arrival date should be successful
        public void ValidateDepartureDateTest()
        {
            Booking mockBooking = new Booking();
            mockBooking.ArrivalDate = DateTime.Today.AddDays(5);
            mockBooking.DepartureDate = DateTime.Today.AddDays(10);

            // if the dates are valid should return null
            Assert.AreEqual(mockBooking.ValidateDepartureDate(), null);

            //should return the appropriate error message if the booking as a whole lasts less than 1 daty
            mockBooking.DepartureDate = DateTime.Today.AddDays(-1);
            Assert.AreEqual(mockBooking.ValidateDepartureDate(), "Your booking date cannot start in the past");

            // departure date set in the past
            mockBooking.DepartureDate = DateTime.Today.AddDays(5); ;
            mockBooking.ArrivalDate = DateTime.Today.AddDays(15);
            Assert.AreEqual(mockBooking.ValidateDepartureDate(), "Your booking cannot end before its start");

            // if the booking lasts 0 days amount of days
            mockBooking.ArrivalDate = DateTime.Today.AddDays(5);
            mockBooking.DepartureDate = DateTime.Today.AddDays(5);
            Assert.AreEqual(mockBooking.ValidateDepartureDate(), "Your booking must last at least one day.");
        }

        [TestMethod()]
        // validates the status of the guest list
        public void ValidateGuestsListTest()
        {
            Booking mockBooking = new Booking();

            // returns an error if 0 guests have been added
            Assert.AreEqual(mockBooking.ValidateGuestsList(), "You need at least one guest to proceed with your booking.");

            for (int i = 0; i < 5; i++)
            {
                mockBooking.Guests.Add(new Guest());
                if (i <= 3)
                {   
                    // if the guests are between 1 and 4, no errors are returned
                    Assert.IsNull(mockBooking.ValidateGuestsList());
                }
                else
                {
                    // otherwise they are
                    Assert.IsNotNull(mockBooking.ValidateGuestsList());
                    Assert.AreEqual(mockBooking.ValidateGuestsList(), "Too many guests selected.");
                }
            }
        }

        [TestMethod()]
        // should create a deep copy of the booking
        public void CloneTest()
        {
            // create basic booking
            Booking mockBooking = new Booking();
            mockBooking.ArrivalDate = DateTime.Today.AddDays(5);
            mockBooking.DepartureDate = DateTime.Today.AddDays(10);
            mockBooking.Breakfast = new Breakfast();
            mockBooking.DietaryReqs = "Annoying vegan person";

            // create clone
            Booking cloneBooking = (Booking) mockBooking.Clone();

            // change values on the original
            mockBooking.ArrivalDate = DateTime.Today.AddDays(234);
            mockBooking.DepartureDate = DateTime.Today.AddDays(-345);
            mockBooking.Breakfast = null;
            mockBooking.DietaryReqs = "Some other annoying requirement";

            // observe that the deep copy has still the originals
            Assert.IsNotNull(cloneBooking);
            Assert.IsNotNull(cloneBooking.Breakfast);
            Assert.AreEqual(cloneBooking.ArrivalDate, DateTime.Today.AddDays(5));
            Assert.AreEqual(cloneBooking.DepartureDate, DateTime.Today.AddDays(10));
            Assert.AreEqual(cloneBooking.DietaryReqs, "Annoying vegan person");
            Assert.AreNotEqual(cloneBooking, mockBooking);

        }
    }
}
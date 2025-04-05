using AirportTicketBooking.AirportTicketBooking.Interfaces;
using AirportTicketBooking.AirportTicketBooking.Models.Enums;
using AirportTicketBooking.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirportTicketBooking.Menu
{
    public class PassengerOptions
    {
        private readonly IPassengerService _passengerService;
        private readonly IFlightService _flightService;
        private readonly IBookingsData _bookingsData;
        private readonly IFlightImportService _flightImportService;
        IConsoleReader consoleReader = new ConsoleReader();

        public PassengerOptions(
            IPassengerService passengerService,
            IFlightService flightService,
            IBookingsData bookingsData,
            IFlightImportService flightImportService)
        {
            _passengerService = passengerService;
            _flightService = flightService;
            _bookingsData = bookingsData;
            _flightImportService = flightImportService;
        }

        public async Task PassengerMenu()
        {
            _bookingsData.Load_Bookings();
            await _flightImportService.ImportFlightsFromCSVAsync(false);
            bool backToMain = false;

            while (!backToMain)
            {
                Console.WriteLine("=== Passenger Menu ===");
                Console.WriteLine("1. View All Available Flights");
                Console.WriteLine("2. Search for Available Flights");
                Console.WriteLine("3. Book a Flight");
                Console.WriteLine("4. Manage Booking");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        _flightService.DisplayFlights();
                        break;
                    case "2":
                        GetFlightSearchDetails();
                        break;
                    case "3":
                        GetBookingDetails();
                        break;
                    case "4":
                        ManageBookings();
                        break;
                    case "5":
                        backToMain = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void ManageBookings()
        {
            bool backToPassengerMenu = false;

            while (!backToPassengerMenu)
            {
                Console.WriteLine("=== Manage Bookings ===");
                Console.WriteLine("1. View My Bookings");
                Console.WriteLine("2. Modify a Booking");
                Console.WriteLine("3. Cancel a Booking");
                Console.WriteLine("4. Back to Passenger Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter Your ID:");
                        int passengerId;
                        while (!int.TryParse(Console.ReadLine(), out passengerId) || passengerId <= 0)
                        {
                            Console.WriteLine("Invalid input. Please enter a valid ID:");
                        }

                        _passengerService.ViewPersonalBookings(passengerId);
                        break;
                    case "2":
                        Console.WriteLine("Enter Your Booking ID:");
                        int bookId;
                        while (!int.TryParse(Console.ReadLine(), out bookId) || bookId <= 0)
                        {
                            Console.WriteLine("Invalid input. Please enter a valid ID:");
                        }
                        _passengerService.ModifyBook(bookId, consoleReader);

                        //_passengerService.ModifyBook(bookId);
                        break;
                    case "3":
                        Console.WriteLine("Enter The Booking ID You Want To Cancel:");
                        int cancelBookingId;
                        while (!int.TryParse(Console.ReadLine(), out cancelBookingId) || cancelBookingId <= 0)
                        {
                            Console.WriteLine("Invalid input. Please enter a valid ID:");
                        }

                        _passengerService.CancelBook(cancelBookingId);
                        break;
                    case "4":
                        backToPassengerMenu = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void GetFlightSearchDetails()
        {
            Console.WriteLine("Enter departure country (or press Enter to skip):");
            string departureCountry = Console.ReadLine();

            Console.WriteLine("Enter destination country (or press Enter to skip):");
            string destinationCountry = Console.ReadLine();

            Console.WriteLine("Enter departure date (YYYY-MM-DD) (or press Enter to skip):");
            string dateInput = Console.ReadLine();
            DateTime? departureDate = string.IsNullOrWhiteSpace(dateInput) ? null : DateTime.Parse(dateInput);

            Console.WriteLine("Enter departure airport (or press Enter to skip):");
            string departureAirport = Console.ReadLine();

            Console.WriteLine("Enter arrival airport (or press Enter to skip):");
            string arrivalAirport = Console.ReadLine();

            Console.WriteLine("Enter maximum price (or press Enter to skip):");
            string priceInput = Console.ReadLine();
            double? maxPrice = string.IsNullOrWhiteSpace(priceInput) ? null : double.Parse(priceInput);

            Console.WriteLine("Enter class type (economy, business, firstclass) (or press Enter to skip):");
            string classTypeInput = Console.ReadLine();

            List<Flight> results = _passengerService.SearchAvailableFlights(
                maxPrice, departureCountry, destinationCountry,
                departureDate, departureAirport, arrivalAirport, classTypeInput);

            _flightService.DisplaySearchResults(results);
        }

        private void GetBookingDetails()
        {
            Console.WriteLine("Enter The ID Of The Flight You Want To Book:");
            int flightId;
            while (!int.TryParse(Console.ReadLine(), out flightId) || flightId <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid Flight ID:");
            }

            Console.WriteLine("Enter Your ID:");
            int passengerId;
            while (!int.TryParse(Console.ReadLine(), out passengerId) || passengerId <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid ID:");
            }

            Console.WriteLine("Enter Class Type (Economy/Business/FirstClass):");
            ClassType classType;
            while (true)
            {
                string classTypeInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(classTypeInput))
                {
                    Console.WriteLine("Invalid input. Class type cannot be empty. Please enter 'Economy', 'Business', or 'FirstClass':");
                    continue;
                }

                if (Enum.TryParse(classTypeInput, true, out classType) &&
                    Enum.IsDefined(typeof(ClassType), classType))
                {
                    break;
                }

                Console.WriteLine("Invalid class type. Please enter 'Economy', 'Business', or 'FirstClass':");
            }

            _passengerService.Book(flightId, passengerId, classType);
        }
    }
}
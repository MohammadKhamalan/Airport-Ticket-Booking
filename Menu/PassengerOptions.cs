using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Ticket_Booking.Menu
{
    class PassengerOptions
    {
        static BookingService bookingservice = new BookingService();
        static FlightService flightService = new FlightService();

     public static async Task PassengerMenu()
        {
            bookingservice.Load_Bookings();
            await flightService.ImportFlightsFromCSVAsync(false);
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
                        flightService.DisplayFlights();
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
        public static void ManageBookings()
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
                        int PassengerId;
                        while (!int.TryParse(Console.ReadLine(), out PassengerId) || PassengerId <= 0)
                        {
                            Console.WriteLine("Invalid input. Please enter a valid ID:");
                        }
                        
                        bookingservice.ViewPersonalBookings(PassengerId);
                        break;
                    case "2":
                        Console.WriteLine("Enter Your Booking ID:");
                        int book_id;
                        while (!int.TryParse(Console.ReadLine(), out book_id) || book_id <= 0)
                        {
                            Console.WriteLine("Invalid input. Please enter a valid ID:");
                        }
                       
                        bookingservice.ModifyBook(book_id);
                        break;
                    case "3":
                        Console.WriteLine("Enter The Booking ID You Want To Cancel:");
                        int cancelbooking_id;
                        while (!int.TryParse(Console.ReadLine(), out cancelbooking_id) || cancelbooking_id <= 0)
                        {
                            Console.WriteLine("Invalid input. Please enter a valid ID:");
                        }
                       
                        bookingservice.CancelBook(cancelbooking_id);
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

        static void GetFlightSearchDetails()
        {
            Console.WriteLine("Enter departure country (or press Enter to skip):");
            string departureCountry = Console.ReadLine();

            Console.WriteLine("Enter destination country (or press Enter to skip):");
            string destinationCountry = Console.ReadLine();

            Console.WriteLine("Enter departure date (YYYY-MM-DD) (or press Enter to skip):");
            string dateInput = Console.ReadLine();
            DateTime? departureDate = string.IsNullOrWhiteSpace(dateInput) ? (DateTime?)null : DateTime.Parse(dateInput);

            Console.WriteLine("Enter departure airport (or press Enter to skip):");
            string departureAirport = Console.ReadLine();

            Console.WriteLine("Enter arrival airport (or press Enter to skip):");
            string arrivalAirport = Console.ReadLine();
            Console.WriteLine("Enter maximum price (or press Enter to skip):");
            string priceInput = Console.ReadLine();
            double? maxPrice = string.IsNullOrWhiteSpace(priceInput) ? (double?)null : double.Parse(priceInput);
            Console.WriteLine("Enter class type (economy, business, firstclass) (or press Enter to skip):");
            string classTypeInput = Console.ReadLine();
            ClassType? classType = null;
            if (!string.IsNullOrEmpty(classTypeInput))
            {
                if (Enum.TryParse<ClassType>(classTypeInput, true, out var parsedClassType))
                {
                    classType = parsedClassType;
                }
                else
                {
                    Console.WriteLine("Invalid class type entered. Using default 'Economy'.");
                    classType = ClassType.Economy;
                }
            }
            List<Flight> results = flightService.Search_Available_Flights(maxPrice, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, classType?.ToString());
            flightService.DisplaySearchResults(results);
        }
        static void GetBookingDetails()
        {
            bookingservice.Load_Bookings();
            Console.WriteLine("Enter The ID Of The Flight You Want To Book:");
            int flight_id;
            while (!int.TryParse(Console.ReadLine(), out flight_id) || flight_id <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid Flight ID:");
            }
            Console.WriteLine("Enter Your ID:");
            int passenger_id;
            while (!int.TryParse(Console.ReadLine(), out passenger_id) || passenger_id <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid ID:");
            }
            Console.WriteLine("Enter Class Type (Economy/Business/FirstClass):");
            string class_type_input;
            ClassType class_type_enum;
            while (true)
            {
                class_type_input = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(class_type_input))
                {
                    Console.WriteLine("Invalid input. Class type cannot be empty. Please enter 'Economy', 'Business', or 'FirstClass':");
                    continue;
                }
                class_type_input = class_type_input.ToLower();
                if (class_type_input == "economy")
                    class_type_input = "Economy";
                else if (class_type_input == "business")
                    class_type_input = "Business";
                else if (class_type_input == "firstclass")
                    class_type_input = "FirstClass";
                if (Enum.TryParse(class_type_input, out class_type_enum) &&
                    Enum.IsDefined(typeof(ClassType), class_type_enum))
                {
                    break;
                }

                Console.WriteLine("Invalid class type. Please enter 'Economy', 'Business', or 'FirstClass':");
            }
            bookingservice.Book(flight_id, passenger_id, class_type_enum);
        }
    }
}
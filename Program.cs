
using System;
using System.Collections.Generic;
using Airport_Ticket_Booking.Services;

namespace Airport_Ticket_Booking
{
    class Program
    {
        static FlightService flightService = new FlightService();
        static BookingService bookingService = new BookingService();


        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {

                Console.WriteLine("=== Airport Ticket Booking System ===");
                Console.WriteLine("1. Passenger Menu");
                Console.WriteLine("2. Manager Menu");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        PassengerMenu();
                        break;
                    case "2":
                        ManagerMenu();
                        break;
                    case "3":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void PassengerMenu()
        {
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
                        flightService.ImportFlightsFromCSV(false);
                        break;
                    case "2":
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
                        string classType = Console.ReadLine();

                        List<Flight> results = flightService.Search_Available_Flights(maxPrice, departureCountry, destinationCountry, departureDate, departureAirport, arrivalAirport, classType);
                        flightService.DisplaySearchResults(results);
                        break;
                    case "3":
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
                        string class_type;
                        while (true)
                        {
                            class_type = Console.ReadLine()?.Trim().ToLower();
                            if (class_type == "economy" || class_type == "business" || class_type == "firstclass")
                                break;
                            Console.WriteLine("Invalid class type. Please enter 'Economy', 'Business', or 'FirstClass':");
                        }
                        bookingService.Book(flight_id, passenger_id, class_type);

                        break;
                    case "4":
                        ManageBookings();
                        break;
                    case "5":
                        backToMain = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }






        static void ManageBookings()
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
                        bookingService.View_Personal_Bookings(PassengerId);
                        break;
                    case "2":
                        Console.WriteLine("Enter Your Booking ID:");
                        int book_id;
                        while (!int.TryParse(Console.ReadLine(), out book_id) || book_id <= 0)
                        {
                            Console.WriteLine("Invalid input. Please enter a valid ID:");
                        }
                        bookingService.Modify_Book(book_id);
                        break;
                    case "3":
                        Console.WriteLine("Enter The Booking ID You Want To Cancel:");
                        int cancelbooking_id;
                        while (!int.TryParse(Console.ReadLine(), out cancelbooking_id) || cancelbooking_id <= 0)
                        {
                            Console.WriteLine("Invalid input. Please enter a valid ID:");
                        }
                        bookingService.Cancel_Book(cancelbooking_id);
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

        static void ManagerMenu()
        {
            bool backToMain = false;

            while (!backToMain)
            {
                Console.WriteLine("=== Manager Menu ===");
                Console.WriteLine("1. Filter Bookings");
                Console.WriteLine("2. Import Flights from CSV");
                Console.WriteLine("3. Display All Bookings");
                Console.WriteLine("4. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter Flight ID (or press Enter to skip):");
                        int? flightId = TryParseNullableInt(Console.ReadLine());

                        Console.WriteLine("Enter Max Price (or press Enter to skip):");
                        double? maxPrice = TryParseNullableDouble(Console.ReadLine());

                        Console.WriteLine("Enter Departure Country (or press Enter to skip):");
                        string departureCountry = Console.ReadLine();

                        Console.WriteLine("Enter Destination Country (or press Enter to skip):");
                        string destinationCountry = Console.ReadLine();

                        Console.WriteLine("Enter Departure Date (YYYY-MM-DD) (or press Enter to skip):");
                        DateTime? departureDate = TryParseNullableDateTime(Console.ReadLine());

                        Console.WriteLine("Enter Departure Airport (or press Enter to skip):");
                        string departureAirport = Console.ReadLine();

                        Console.WriteLine("Enter Arrival Airport (or press Enter to skip):");
                        string arrivalAirport = Console.ReadLine();

                        Console.WriteLine("Enter Passenger ID (or press Enter to skip):");
                        int? passengerId = TryParseNullableInt(Console.ReadLine());

                        Console.WriteLine("Enter Class Type (Economy, Business, FirstClass) (or press Enter to skip):");
                        string classType = Console.ReadLine();

                        List<Booking> filteredBookings = bookingService.filtered_Bookings(
                            flightId, maxPrice, departureCountry, destinationCountry,
                            departureDate, departureAirport, arrivalAirport, passengerId, classType);

                        if (maxPrice.HasValue)
                        {
                            bookingService.DisplayFilteredBookings(filteredBookings, maxPrice.Value);
                        }
                        else
                        {
                            bookingService.DisplayFilteredBookings(filteredBookings, double.MaxValue);
                        }

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "2":

                        Console.WriteLine("Here Is All Flights Stored In System:");
                        flightService.ImportFlightsFromCSV(true);

                        break;


                    case "3":

                        Console.WriteLine("=== All Bookings ===");
                        bookingService.Display_Bookings();

                        break;

                    case "4":
                        backToMain = true;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }


        static int? TryParseNullableInt(string input)
        {
            return int.TryParse(input, out int result) ? result : (int?)null;
        }

        static double? TryParseNullableDouble(string input)
        {
            return double.TryParse(input, out double result) ? result : (double?)null;
        }

        static DateTime? TryParseNullableDateTime(string input)
        {
            return DateTime.TryParse(input, out DateTime result) ? result : (DateTime?)null;
        }
    }

}
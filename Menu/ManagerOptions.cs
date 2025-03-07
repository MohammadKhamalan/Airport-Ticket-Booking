using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Airport_Ticket_Booking.Services;
using Airport_Ticket_Booking.Models;

namespace Airport_Ticket_Booking
{
    public class ManagerOptions
    {
       static  BookingService bookingservice = new BookingService();
        static FlightService flightService = new FlightService();

    
        public static async Task ManagerMenu()
        {
            bookingservice.Load_Bookings();
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

                       await FilterBookings();

                        break;

                    case "2":
                        Console.WriteLine("Here Is All Flights Stored In System:");
                        await flightService.ImportFlightsFromCSVAsync(true);
                        flightService.DisplayFlights();
                        break;

                    case "3":
                        Console.WriteLine("=== All Bookings ===");

                       
                        bookingservice.DisplayBookings();


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
        static async Task FilterBookings()
        {
           
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

            List<Booking> filteredBookings = await bookingservice.FilteredBookingsAsync(
flightId, maxPrice, departureCountry, destinationCountry,
departureDate, departureAirport, arrivalAirport, passengerId, classType);


            if (maxPrice.HasValue)
            {
                bookingservice.DisplayFilteredBookings(filteredBookings, maxPrice.Value);
            }
            else
            {
                bookingservice.DisplayFilteredBookings(filteredBookings, double.MaxValue);
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
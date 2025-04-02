using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AirportTicketBooking.Menu;
using AirportTicketBooking.Services;

namespace AirportTicketBooking
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            var flights = new List<Flight>();

            var bookingsData = new BookingDataService();
            var flightDataService = new FlightDataService(flights);

            var flightImportService = new FlightImportService(flightDataService);
            var flightDisplayService = new FlightDisplayService(flightDataService);
            var passengerService = new PassengerService(bookingsData, flightDataService);
            var bookingDisplayService = new BookingDisplayService(bookingsData, flightDataService);
            var bookingManagerService = new BookingManagerService(
                bookingsData, flightImportService, passengerService);

            bookingsData.Load_Bookings();
            await flightImportService.ImportFlightsFromCSVAsync(false);

            var managerOptions = new ManagerOptions(
                bookingManagerService,
                bookingDisplayService,
                flightImportService,
                flightDisplayService);

            var passengerOptions = new PassengerOptions(
                passengerService,
                flightDisplayService,
                bookingsData,
                flightImportService);

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
                        await passengerOptions.PassengerMenu();
                        break;
                    case "2":
                        await managerOptions.ManagerMenu();
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
    }
}
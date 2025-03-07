using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Airport_Ticket_Booking.Menu;
using Airport_Ticket_Booking.Models;
using Airport_Ticket_Booking.Services;
namespace Airport_Ticket_Booking
{
    class Program
    {
     
        static async Task Main(string[] args)
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
                        await PassengerOptions.PassengerMenu();
                        break;
                    case "2":
                        await ManagerOptions.ManagerMenu();

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
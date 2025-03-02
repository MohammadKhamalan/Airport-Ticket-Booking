using System;
using Airport_Ticket_Booking.Services;

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to Airport Ticket Booking System");
        FlightService f = new FlightService();
        BookingService b = new BookingService();
        f.DisplayFlights();
        b.Book(101,3, "Economy");
        
    }
}

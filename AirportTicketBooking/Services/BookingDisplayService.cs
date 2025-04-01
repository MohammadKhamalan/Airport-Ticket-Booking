using AirportTicketBooking.AirportTicketBooking.Interfaces;    
using AirportTicketBooking.AirportTicketBooking.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirportTicketBooking.Services
{
    public class BookingDisplayService : IBookingService
    {
        private readonly IBookingsData _bookingsData;
        private readonly IFlightsData _flightsData;

        public BookingDisplayService(IBookingsData bookingsData, IFlightsData flightsData)
        {
            _bookingsData = bookingsData;
            _flightsData = flightsData;
        }

        public void DisplayFilteredBookings(List<Booking> bookings, double maxPrice)
        {
            if (bookings.Count == 0)
            {
                Console.WriteLine("No matching bookings found.");
                return;
            }

            var bookingWithPrices = from booking in bookings
                                    join flight in _flightsData.GetFlights() on booking.FlightId equals flight.FlightId
                                    let price = booking.ClassType switch
                                    {
                                        ClassType.Economy => flight.EconomyPrice,
                                        ClassType.Business => flight.BusinessPrice,
                                        ClassType.FirstClass => flight.FirstClassPrice,
                                        _ => 0
                                    }
                                    where price <= maxPrice
                                    select new
                                    {
                                        booking.Id,
                                        booking.FlightId,
                                        booking.PassengerId,
                                        booking.ClassType,
                                        Price = price
                                    };

            if (!bookingWithPrices.Any())
            {
                Console.WriteLine("No matching bookings found within the price limit.");
                return;
            }

            Console.WriteLine("Filtered Bookings with Prices:");
            Console.WriteLine("-------------------------------------------------------------");
            foreach (var booking in bookingWithPrices)
            {
                Console.WriteLine($"Booking ID: {booking.Id}");
                Console.WriteLine($"Flight ID: {booking.FlightId}");
                Console.WriteLine($"Passenger ID: {booking.PassengerId}");
                Console.WriteLine($"Class: {booking.ClassType}");
                Console.WriteLine($"Price: {booking.Price} USD");
                Console.WriteLine("-------------------------------------------------------------");
            }
        }

        public void DisplayBookings()
        {
            foreach (var booking in _bookingsData.Bookings)
            {
                Console.WriteLine($"Booking {booking.Id}: Flight {booking.FlightId}, Class: {booking.ClassType}");
            }
        }
    }
}
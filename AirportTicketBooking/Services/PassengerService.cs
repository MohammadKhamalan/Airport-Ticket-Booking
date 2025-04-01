using AirportTicketBooking.AirportTicketBooking.Interfaces;
using AirportTicketBooking.AirportTicketBooking.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirportTicketBooking.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IBookingsData _bookingsData;
        private readonly IFlightsData _flightsData;

        public PassengerService(IBookingsData bookingsData, IFlightsData flightsData)
        {
            _bookingsData = bookingsData;
            _flightsData = flightsData;
        }

        public void Book(int flight_id, int passenger_id, ClassType classType)
        {
            var existingBooking = _bookingsData.Bookings.FirstOrDefault(b =>
                b.FlightId == flight_id && b.PassengerId == passenger_id);

            if (existingBooking != null)
            {
                Console.WriteLine("You have already booked this flight.");
                return;
            }

            int newBookingId = _bookingsData.Bookings.Count > 0 ?
                _bookingsData.Bookings.Max(b => b.Id) + 1 : 1;

            var newBooking = new Booking(newBookingId, flight_id, passenger_id, classType);
            _bookingsData.Bookings.Add(newBooking);
            _bookingsData.SaveBookings();

            Console.WriteLine($"Booking {newBookingId} added successfully!");
        }

        public List<Booking> ViewPersonalBookings(int passenger_id)
        {
            var personalBookings = _bookingsData.Bookings
                .Where(b => b.PassengerId == passenger_id)
                .ToList();

            if (personalBookings.Count == 0)
            {
                Console.WriteLine("No bookings found for this passenger.");
            }
            else
            {
                Console.WriteLine($"Found {personalBookings.Count} booking(s):");
                foreach (var booking in personalBookings)
                {
                    Console.WriteLine($"- Booking ID: {booking.Id}, Flight ID: {booking.FlightId}, Class: {booking.ClassType}");
                }
            }

            return personalBookings;
        }

        public void ModifyBook(int bookingId)
        {
            var booking = _bookingsData.Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null)
            {
                Console.WriteLine($"Booking with ID {bookingId} not found.");
                return;
            }

            Console.WriteLine("Current booking details:");
            Console.WriteLine($"- Flight ID: {booking.FlightId}");
            Console.WriteLine($"- Passenger ID: {booking.PassengerId}");
            Console.WriteLine($"- Class: {booking.ClassType}");

            Console.WriteLine("Enter new Flight ID (press Enter to keep current):");
            string flightInput = Console.ReadLine();
            int newFlightId = string.IsNullOrEmpty(flightInput) ?
                booking.FlightId : int.Parse(flightInput);

            Console.WriteLine("Enter new Passenger ID (press Enter to keep current):");
            string passengerInput = Console.ReadLine();
            int newPassengerId = string.IsNullOrEmpty(passengerInput) ?
                booking.PassengerId : int.Parse(passengerInput);

            Console.WriteLine("Enter new Class Type (Economy/Business/FirstClass, press Enter to keep current):");
            string classInput = Console.ReadLine();
            ClassType newClass = string.IsNullOrEmpty(classInput) ?
                booking.ClassType : Enum.Parse<ClassType>(classInput, true);

            var modifiedBooking = booking with
            {
                FlightId = newFlightId,
                PassengerId = newPassengerId,
                ClassType = newClass
            };

            _bookingsData.Bookings.Remove(booking);
            _bookingsData.Bookings.Add(modifiedBooking);
            _bookingsData.SaveBookings();

            Console.WriteLine($"Booking {bookingId} modified successfully!");
        }

        public void CancelBook(int bookingId)
        {
            var booking = _bookingsData.Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null)
            {
                Console.WriteLine("Booking not found.");
                return;
            }

            _bookingsData.Bookings.Remove(booking);
            _bookingsData.SaveBookings();
            Console.WriteLine($"Booking {bookingId} has been canceled successfully.");
        }

        public List<Flight> Search_Available_Flights(double? maxprice, string departure_country,
            string destination_country, DateTime? departure_date, string departure_airport,
            string arrival_airport, string classType)
        {
            return _flightsData.GetFlights()
                .Where(f => string.IsNullOrEmpty(departure_country) ||
                    f.DepartureCountry.Equals(departure_country, StringComparison.OrdinalIgnoreCase))
                .Where(f => string.IsNullOrEmpty(destination_country) ||
                    f.DestinationCountry.Equals(destination_country, StringComparison.OrdinalIgnoreCase))
                .Where(f => !departure_date.HasValue || f.DepartureDate.Date == departure_date.Value.Date)
                .Where(f => string.IsNullOrEmpty(departure_airport) ||
                    f.DepartureAirport.Equals(departure_airport, StringComparison.OrdinalIgnoreCase))
                .Where(f => string.IsNullOrEmpty(arrival_airport) ||
                    f.ArrivalAirport.Equals(arrival_airport, StringComparison.OrdinalIgnoreCase))
                .Where(f => !maxprice.HasValue || FilterByClassType(f, maxprice.Value, classType))
                .ToList();
        }

        private bool FilterByClassType(Flight flight, double maxprice, string classType)
        {
            if (string.IsNullOrEmpty(classType))
                return flight.EconomyPrice <= maxprice ||
                       flight.BusinessPrice <= maxprice ||
                       flight.FirstClassPrice <= maxprice;

            return classType.ToLower() switch
            {
                "economy" => flight.EconomyPrice <= maxprice,
                "business" => flight.BusinessPrice <= maxprice,
                "firstclass" => flight.FirstClassPrice <= maxprice,
                _ => false
            };
        }
    }
}
using AirportTicketBooking.AirportTicketBooking.Interfaces;
using AirportTicketBooking.AirportTicketBooking.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportTicketBooking.Services
{
    public class BookingManagerService : IBookingManager
    {
        private readonly IBookingsData _bookingsData;
        private readonly IFlightImportService _flightImportService;
        private readonly IPassengerService _passengerService;       

        public BookingManagerService(
            IBookingsData bookingsData,
            IFlightImportService flightImportService,
            IPassengerService passengerService)
        {
            _bookingsData = bookingsData;
            _flightImportService = flightImportService;
            _passengerService = passengerService;
        }

        public async Task<List<Booking>> FilteredBookingsAsync(int? id, double? max_price, string departure_country,
            string destination_country, DateTime? departure_date, string departure_airport,
            string arrival_airport, int? passenger_id, string class_type)
        {
            await _flightImportService.ImportFlightsFromCSVAsync(true);

            ClassType? classTypeEnum = null;
            if (!string.IsNullOrEmpty(class_type) && Enum.TryParse<ClassType>(class_type, true, out var parsedClassType))
            {
                classTypeEnum = parsedClassType;
            }

            var filteredBookings = _bookingsData.Bookings.ToList();
            if (id.HasValue)
            {
                filteredBookings = filteredBookings.Where(booking => booking.FlightId == id.Value).ToList();
            }

            if (passenger_id.HasValue)
            {
                filteredBookings = filteredBookings.Where(booking => booking.PassengerId == passenger_id.Value).ToList();
            }

            if (classTypeEnum.HasValue)
            {
                filteredBookings = filteredBookings.Where(booking => booking.ClassType == classTypeEnum.Value).ToList();
            }

            if (filteredBookings.Count == 0)
            {
                Console.WriteLine("No bookings match your search criteria.");
                return new List<Booking>();
            }

            var filteredFlights = _passengerService.SearchAvailableFlights(max_price, departure_country, destination_country,
                departure_date, departure_airport, arrival_airport, class_type);

            if (filteredFlights.Count == 0)
            {
                Console.WriteLine("No flights match your search criteria.");
                return new List<Booking>();
            }

            filteredBookings = filteredBookings
                .Where(booking => filteredFlights.Any(flight => flight.FlightId == booking.FlightId))
                .ToList();

            if (filteredBookings.Count == 0)
            {
                Console.WriteLine("No bookings match the available flights.");
                return new List<Booking>();
            }

            return filteredBookings;
        }
    }
}
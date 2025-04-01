using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirportTicketBooking.AirportTicketBooking.Interfaces
{
    public interface IBookingManager
    {
        Task<List<Booking>> FilteredBookingsAsync(int? id, double? max_price, string departure_country,
            string destination_country, DateTime? departure_date, string departure_airport,
            string arrival_airport, int? passenger_id, string class_type);
    }
}
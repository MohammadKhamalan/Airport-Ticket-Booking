using System.Collections.Generic;

namespace AirportTicketBooking.AirportTicketBooking.Interfaces
{
    public interface IBookingsData
    {
        List<Booking> Bookings { get; }
        void Load_Bookings();
        void SaveBookings();
    }
}
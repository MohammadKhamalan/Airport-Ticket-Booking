using System.Collections.Generic;

namespace AirportTicketBooking.AirportTicketBooking.Interfaces
{
    public interface IBookingService
    {
        void DisplayFilteredBookings(List<Booking> bookings, double maxPrice);
        void DisplayBookings();
    }
}
using System.Collections.Generic;

namespace AirportTicketBooking.AirportTicketBooking.Interfaces
{
    public interface IFlightService
    {
        void DisplaySearchResults(List<Flight> result);
        void DisplayFlights();
    }
}
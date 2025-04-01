using System.Collections.Generic;

namespace AirportTicketBooking.AirportTicketBooking.Interfaces
{
    public interface IFlightsData
    {
        void SaveFlights();
        List<Flight> GetFlights();
    }
}
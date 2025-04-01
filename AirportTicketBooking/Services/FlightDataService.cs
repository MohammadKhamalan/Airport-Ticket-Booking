using AirportTicketBooking.AirportTicketBooking.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace AirportTicketBooking.Services
{
    public class FlightDataService : IFlightsData
    {
        private List<Flight> _flights;

        public FlightDataService(List<Flight> flights)
        {
            _flights = flights;
        }

        public void SaveFlights()
        {
            string flightPath = @"C:\Users\ASUS\Desktop\Airport Ticket Booking\AirportTicketBooking\Data\Flight.csv";
            using (var sw = new StreamWriter(flightPath, false))
            {
                sw.WriteLine("FlightId,DepartureCountry,DestinationCountry,DepartureAirport,ArrivalAirport,DepartureDate,EconomyPrice,BusinessPrice,FirstClassPrice");
                foreach (var flight in _flights)
                {
                    sw.WriteLine($"{flight.FlightId},{flight.DepartureCountry},{flight.DestinationCountry}," +
                               $"{flight.DepartureAirport},{flight.ArrivalAirport},{flight.DepartureDate}," +
                               $"{flight.EconomyPrice},{flight.BusinessPrice},{flight.FirstClassPrice}");
                }
            }
        }

        public List<Flight> GetFlights()
        {
            return _flights;
        }
    }
}
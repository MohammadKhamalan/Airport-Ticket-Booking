using AirportTicketBooking.AirportTicketBooking.Interfaces;
using AirportTicketBooking;
using System.Collections.Generic;
using System.IO;

public class FlightDataService : IFlightsData
{
    private List<Flight> _flights;
    private string _filePath;

    
    public FlightDataService(List<Flight> flights, string filePath = null)
    {
        _flights = flights;
        _filePath = filePath ?? @"C:\Users\ASUS\Desktop\Airport Ticket Booking\AirportTicketBooking\Data\Flight.csv"; 
    }

    public void SaveFlights()
    {
        using (var sw = new StreamWriter(_filePath, false))
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

using AirportTicketBooking.AirportTicketBooking.Interfaces;
using System;
using System.Collections.Generic;

namespace AirportTicketBooking.Services
{
    public class FlightDisplayService : IFlightService
    {
        private readonly IFlightsData _flightsData;

        public FlightDisplayService(IFlightsData flightsData)
        {
            _flightsData = flightsData;
        }

        public void DisplaySearchResults(List<Flight> result)
        {
            if (result.Count == 0)
            {
                Console.WriteLine("No flights match your search criteria.");
                return;
            }

            Console.WriteLine("Available Flights:");
            Console.WriteLine("-------------------------------------------------------------");
            foreach (var flight in result)
            {
                Console.WriteLine($"Flight ID: {flight.FlightId}");
                Console.WriteLine($"From: {flight.DepartureCountry} ({flight.DepartureAirport})");
                Console.WriteLine($"To: {flight.DestinationCountry} ({flight.ArrivalAirport})");
                Console.WriteLine($"Departure Date: {flight.DepartureDate:yyyy-MM-dd}");
                Console.WriteLine($"Economy Price: ${flight.EconomyPrice}");
                Console.WriteLine($"Business Price: ${flight.BusinessPrice}");
                Console.WriteLine($"First Class Price: ${flight.FirstClassPrice}");
                Console.WriteLine("-------------------------------------------------------------");
            }
        }

        public void DisplayFlights()
        {
            foreach (var flight in _flightsData.GetFlights())
            {
                Console.WriteLine($"Flight ID: {flight.FlightId} from {flight.DepartureCountry} to {flight.DestinationCountry} on {flight.DepartureDate}");
            }
        }
    }
}
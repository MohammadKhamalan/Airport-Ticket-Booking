using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Airport_Ticket_Booking.Services
{
    class FlightService
    {
        private List<Flight> flights = new List<Flight>();
        string flight_path = @"C:\Users\ASUS\Desktop\Airport Ticket Booking\Data\Flight.csv";
        public FlightService()
        {
            Load_flight();
        }
        public void Load_flight()
        {
            if (File.Exists(flight_path))
            {
                var lines = File.ReadAllLines(flight_path).Skip(1);

                foreach (var line in lines)
                {
                    var data = line.Split(',');

                    flights.Add(new Flight(int.Parse(data[0]), data[1], data[2], data[3], data[4], DateTime.Parse(data[5]),double.Parse(data[6]), double.Parse(data[7]),double.Parse(data[8])));
               
                }
            }
        }
        public List<Flight> Search_flight(string Departure_country, string Destination_country)
        {
            return flights.Where(flight => flight.DepartureCountry == Departure_country && flight.DestinationCountry == Destination_country).ToList();
            
        }

        public void Display_flights()
        {
            foreach(var flight in flights)
            {
                Console.WriteLine($"Flight ID: {flight.FlightId} from {flight.DepartureCountry} to {flight.DestinationCountry} on {flight.DepartureDate}");
            }
        }
    }
}

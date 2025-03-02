using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Airport_Ticket_Booking.Services
{
    class FlightService
    {
        private List<Flight> flights = new List<Flight>();
        private string flight_path = @"C:\Users\ASUS\Desktop\Airport Ticket Booking\Data\Flight.csv";

        public FlightService()
        {
            ImportFlightsFromCSV();
        }

        public void ImportFlightsFromCSV()
        {
            if (File.Exists(flight_path))
            {
                var lines = File.ReadAllLines(flight_path).Skip(1);
                List<string> errors = new List<string>();

                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    try
                    {
                        if (data.Length < 9)
                        {
                            errors.Add($"Invalid data format in line: {line}");
                            continue;
                        }

                        if (!int.TryParse(data[0], out int flightId))
                        {
                            errors.Add($"Invalid Flight ID in line: {line}");
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(data[1]) || string.IsNullOrWhiteSpace(data[2]))
                        {
                            errors.Add($"Missing departure or destination country in line: {line}");
                            continue;
                        }

                        if (!DateTime.TryParse(data[5], out DateTime departureDate))
                        {
                            errors.Add($"Invalid departure date format in line: {line}");
                            continue;
                        }

                        if (!double.TryParse(data[6], out double economyPrice) ||
                            !double.TryParse(data[7], out double businessPrice) ||
                            !double.TryParse(data[8], out double firstClassPrice))
                        {
                            errors.Add($"Invalid price format in line: {line}");
                            continue;
                        }

                        if (flights.Any(f => f.FlightId == flightId))
                        {
                            errors.Add($"Duplicate Flight ID {flightId} in line: {line}");
                            continue;
                        }

                        var flight = new Flight(flightId, data[1], data[2], data[3], data[4], departureDate, economyPrice, businessPrice, firstClassPrice);
                        flights.Add(flight);
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Error processing line: {line}. Error: {ex.Message}");
                    }
                }

                if (errors.Count > 0)
                {
                    Console.WriteLine("Loading completed with errors:");
                    errors.ForEach(Console.WriteLine);
                }
                else
                {
                    Console.WriteLine("Flights loaded successfully.");
                }
            }
        }

        public void SaveFlights()
        {
            using (StreamWriter sw = new StreamWriter(flight_path, false))
            {
                sw.WriteLine("FlightId,DepartureCountry,DestinationCountry,DepartureAirport,ArrivalAirport,DepartureDate,EconomyPrice,BusinessPrice,FirstClassPrice");
                foreach (var flight in flights)
                {
                    sw.WriteLine($"{flight.FlightId},{flight.DepartureCountry},{flight.DestinationCountry},{flight.DepartureAirport},{flight.ArrivalAirport},{flight.DepartureDate},{flight.EconomyPrice},{flight.BusinessPrice},{flight.FirstClassPrice}");
                }
            }
        }

        public List<Flight> SearchFlight(string departureCountry, string destinationCountry)
        {
            return flights.Where(flight => flight.DepartureCountry == departureCountry && flight.DestinationCountry == destinationCountry).ToList();
        }

        public void DisplayFlights()
        {
            foreach (var flight in flights)
            {
                Console.WriteLine($"Flight ID: {flight.FlightId} from {flight.DepartureCountry} to {flight.DestinationCountry} on {flight.DepartureDate}");
            }
        }
    }
}

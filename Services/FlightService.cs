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
            
        }


        public void ImportFlightsFromCSV(bool user)
        {
            flights.Clear();
            if (!File.Exists(flight_path))
            {
                Console.WriteLine("Flight data file not found.");
                return;
            }

            var lines = File.ReadAllLines(flight_path).Skip(1);
            List<string> errors = new List<string>();

            foreach (var line in lines)
            {
                var data = line.Split(',');
                List<string> lineErrors = new List<string>();

                try
                {
                    if (data.Length != 9)
                    {
                        lineErrors.Add($"Invalid data format: Expected 9 fields but found {data.Length} -> {line}");
                    }

                    if (!int.TryParse(data[0].Trim(), out int flightId))
                    {
                        lineErrors.Add("Invalid Flight ID (Must be an integer).");
                    }

                    if (string.IsNullOrWhiteSpace(data[1]) || string.IsNullOrWhiteSpace(data[2]))
                    {
                        lineErrors.Add("Missing departure or destination country.");
                    }

                    if (!DateTime.TryParse(data[5].Trim(), out DateTime departureDate))
                    {
                        lineErrors.Add($"Invalid departure date: {data[5]} (Expected format: YYYY-MM-DD)");
                    }
                    else if (departureDate < DateTime.Today)
                    {
                        lineErrors.Add("Departure date cannot be in the past.");
                    }

                    bool validEconomy = double.TryParse(data[6].Trim(), out double economyPrice);
                    bool validBusiness = double.TryParse(data[7].Trim(), out double businessPrice);
                    bool validFirstClass = double.TryParse(data[8].Trim(), out double firstClassPrice);

                    if (!validEconomy || !validBusiness || !validFirstClass)
                    {
                        lineErrors.Add($"Invalid price format: {data[6]}, {data[7]}, {data[8]} (Must be numeric)");
                    }

                    if (flights.Any(f => f.FlightId == flightId))
                    {
                        lineErrors.Add($"Duplicate Flight ID: {flightId}.");
                    }

                    if (lineErrors.Count > 0)
                    {
                        if (user) 
                        {
                            errors.Add($"Line: {line} -> Errors: {string.Join(", ", lineErrors)}");
                        }
                    }
                    else
                    {
                        var flight = new Flight(flightId, data[1].Trim(), data[2].Trim(), data[3].Trim(), data[4].Trim(), departureDate, economyPrice, businessPrice, firstClassPrice);
                        flights.Add(flight);
                    }
                }
                catch (Exception ex)
                {
                    if (user)
                    {
                        errors.Add($"Unexpected error in line: {line}. Error: {ex.Message}");
                    }
                }
            }

            if (user && errors.Count > 0)
            {
                Console.WriteLine("Loading completed with errors:");
                errors.ForEach(Console.WriteLine);
            }
            else
            {
                Console.WriteLine("Flights loaded successfully.");
            }

            DisplayFlights();
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
        public List<Flight> Search_Available_Flights(double? maxprice, string departure_country, string destination_country, DateTime? departure_date,
    string departure_airport, string arrival_airport, string classType)
        {
            var result = flights.Where(flight =>
                (string.IsNullOrEmpty(departure_country) || flight.DepartureCountry.Equals(departure_country, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(destination_country) || flight.DestinationCountry.Equals(destination_country, StringComparison.OrdinalIgnoreCase)) &&
                (!departure_date.HasValue || flight.DepartureDate.Date == departure_date.Value.Date) &&
                (string.IsNullOrEmpty(departure_airport) || flight.DepartureAirport.Equals(departure_airport, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(arrival_airport) || flight.ArrivalAirport.Equals(arrival_airport, StringComparison.OrdinalIgnoreCase)) &&
                (!maxprice.HasValue || FilterByClassType(flight, maxprice.Value, classType))
            ).ToList();

            return result;
        }

        private bool FilterByClassType(Flight flight, double maxprice, string classType)
        {
            if (string.IsNullOrEmpty(classType))
                return flight.EconomyPrice <= maxprice || flight.BusinessPrice <= maxprice || flight.FirstClassPrice <= maxprice;

            classType = classType.ToLower();
            return classType switch
            {
                "economy" => flight.EconomyPrice <= maxprice,
                "business" => flight.BusinessPrice <= maxprice,
                "firstclass" => flight.FirstClassPrice <= maxprice,
                _ => false
            };
        }



        public List<Flight> GetFlights()
        {
            return flights;
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
            foreach (var flight in flights)
            {
                Console.WriteLine($"Flight ID: {flight.FlightId} from {flight.DepartureCountry} to {flight.DestinationCountry} on {flight.DepartureDate}");
            }
        }
    }
}



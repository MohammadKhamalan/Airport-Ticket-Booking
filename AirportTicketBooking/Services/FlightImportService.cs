using AirportTicketBooking.AirportTicketBooking.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Linq;
using AirportTicketBooking;

public class FlightImportService : IFlightImportService
{
    private readonly IFlightsData _flightsData;
    private readonly string _flightPath = @"C:\Users\ASUS\Desktop\Airport Ticket Booking\AirportTicketBooking\Data\Flight.csv";
    private readonly Func<string, bool> _fileExists;

    public FlightImportService(IFlightsData flightsData, Func<string, bool> fileExists = null)
    {
        _flightsData = flightsData;
        _fileExists = fileExists ?? File.Exists; // Default to File.Exists if delegate is not provided
    }

    public async Task ImportFlightsFromCSVAsync(bool user)
    {
        var flights = _flightsData.GetFlights();
        flights.Clear();

        // Use the injected delegate to check if the file exists
        if (!_fileExists(_flightPath))  // Use the injected fileExists delegate
        {
            Console.WriteLine("Flight data file not found.");
            return;
        }

        var errors = new List<string>();
        var lines = await File.ReadAllLinesAsync(_flightPath);

        await Task.Run(() =>
        {
            foreach (var line in lines.Skip(1))
            {
                var data = line.Split(',');
                var lineErrors = new List<string>();

                try
                {
                    if (data.Length != 9)
                        lineErrors.Add($"Invalid data format: Expected 9 fields but found {data.Length} -> {line}");

                    if (!int.TryParse(data[0].Trim(), out int flightId))
                        lineErrors.Add("Invalid Flight ID (Must be an integer).");

                    if (string.IsNullOrWhiteSpace(data[1]) || string.IsNullOrWhiteSpace(data[2]))
                        lineErrors.Add("Missing departure or destination country.");

                    if (!DateTime.TryParse(data[5].Trim(), out DateTime departureDate))
                        lineErrors.Add($"Invalid departure date: {data[5]} (Expected format: YYYY-MM-DD)");
                    else if (departureDate < DateTime.Today)
                        lineErrors.Add("Departure date cannot be in the past.");

                    bool validEconomy = double.TryParse(data[6].Trim(), out double economyPrice);
                    bool validBusiness = double.TryParse(data[7].Trim(), out double businessPrice);
                    bool validFirstClass = double.TryParse(data[8].Trim(), out double firstClassPrice);

                    if (!validEconomy || !validBusiness || !validFirstClass)
                        lineErrors.Add($"Invalid price format: {data[6]}, {data[7]}, {data[8]} (Must be numeric)");

                    if (flights.Any(f => f.FlightId == flightId))
                        lineErrors.Add($"Duplicate Flight ID: {flightId}.");
                    else
                        flights.Add(new Flight(
                            flightId,
                            data[1].Trim(),
                            data[2].Trim(),
                            data[3].Trim(),
                            data[4].Trim(),
                            departureDate,
                            economyPrice,
                            businessPrice,
                            firstClassPrice));

                    if (lineErrors.Count > 0 && user)
                        errors.Add($"Line: {line} -> Errors: {string.Join(", ", lineErrors)}");
                }
                catch (Exception ex)
                {
                    if (user)
                        errors.Add($"Unexpected error in line: {line}. Error: {ex.Message}");
                }
            }
        });

        if (user && errors.Count > 0)
        {
            Console.WriteLine("Loading completed with errors:");
            errors.ForEach(Console.WriteLine);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit;
using FluentAssertions;
using AirportTicketBooking;

namespace AirportTicketBooking.Tests
{
    public class FlightDataServiceTests
    {
        private readonly DateTime _assumedDate = new DateTime(2025, 1, 1, 8, 0, 0);

        private List<Flight> GetSampleFlights() => new List<Flight>
        {
            new Flight(1, "USA", "Canada", "JFK", "YYZ", _assumedDate, 100, 200, 300),
            new Flight(2, "UK", "Germany", "LHR", "FRA", _assumedDate, 150, 250, 350)
        };

        [Fact]
        public void GetFlights_ShouldReturnCorrectList_Pass()
        {
            // Arrange
            var flights = GetSampleFlights();
            var service = new FlightDataService(flights);

            // Act
            var result = service.GetFlights();

            // Assert
            result.Should().BeEquivalentTo(flights);
        }

        [Fact]
        public void GetFlights_ShouldNotReturnThree_WhenInitializedWithTwo()
        {
            // Arrange
            var flights = GetSampleFlights();
            var service = new FlightDataService(flights);

            // Act
            var result = service.GetFlights();

            // Assert 
            result.Should().NotHaveCount(3, "because only 2 flights were initialized");
        }


        [Fact]
        public void SaveFlights_ShouldCreateCorrectFile_Pass()
        {
            // Arrange
            var flights = GetSampleFlights();
            var filePath = Path.Combine(Path.GetTempPath(), "test_flights.csv");
            var service = new FlightDataService(flights, filePath);

            // Act
            service.SaveFlights();

            // Assert
            File.Exists(filePath).Should().BeTrue("because SaveFlights should create the file");

            var content = File.ReadAllText(filePath);
            content.Should().Contain("FlightId,DepartureCountry,DestinationCountry");
            content.Should().Contain("USA").And.Contain("UK");

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void SaveFlights_ShouldSaveAllFlightDetailsCorrectly()
        {
            // Arrange
            var flights = GetSampleFlights();
            var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + "_flights.csv");
            var service = new FlightDataService(flights, filePath);

            // Act
            service.SaveFlights();

            // Assert
            File.Exists(filePath).Should().BeTrue("because the file should be created by SaveFlights");

            var lines = File.ReadAllLines(filePath);
            lines.Should().HaveCount(3, "because there are 2 flights and 1 header");

            lines[1].Should().Contain("USA").And.Contain("JFK");
            lines[2].Should().Contain("Germany").And.Contain("FRA");

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void GetFlights_ShouldReturnSameInstance()
        {
            // Arrange
            var flights = GetSampleFlights();
            var service = new FlightDataService(flights);

            // Act
            var result = service.GetFlights();

            // Assert
            result.Should().BeSameAs(flights, "because the service should return the same instance passed in");
        }

       
    }
}

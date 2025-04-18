using AirportTicketBooking;
using AirportTicketBooking.AirportTicketBooking.Interfaces;
using AirportTicketBooking.AirportTicketBooking.Models.Enums;
using AirportTicketBooking.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

public class BookingManagerServiceTests
{
    private readonly Mock<IBookingsData> _mockBookingsData;
    private readonly Mock<IFlightImportService> _mockFlightImportService;
    private readonly Mock<IPassengerService> _mockPassengerService;
    private readonly BookingManagerService _bookingManagerService;

    public BookingManagerServiceTests()
    {
        _mockBookingsData = new Mock<IBookingsData>();
        _mockFlightImportService = new Mock<IFlightImportService>();
        _mockPassengerService = new Mock<IPassengerService>();

        _bookingManagerService = new BookingManagerService(
            _mockBookingsData.Object,
            _mockFlightImportService.Object,
            _mockPassengerService.Object
        );
    }

    [Fact]
    public async Task FilteredBookingsAsync_FiltersByFlightId()
    {
        // Arrange
        var bookings = new List<Booking>
    {
        new Booking(1, 10, 100, ClassType.Business),
        new Booking(2, 20, 101, ClassType.Economy)
    };

        var flights = new List<Flight>
    {
        new Flight(10, "US", "UK", "JFK", "LHR", DateTime.Today, 100, 200, 300),
        new Flight(20, "US", "FR", "JFK", "CDG", DateTime.Today, 100, 200, 300)
    };

        _mockBookingsData.Setup(b => b.Bookings).Returns(bookings);
        _mockPassengerService.Setup(p => p.SearchAvailableFlights(null, null, null, null, null, null, null))
            .Returns(flights);
        _mockFlightImportService.Setup(f => f.ImportFlightsFromCSVAsync(true)).Returns(Task.CompletedTask);

        // Act
        var result = await _bookingManagerService.FilteredBookingsAsync(
            id: 10, max_price: null, departure_country: null, destination_country: null,
            departure_date: null, departure_airport: null, arrival_airport: null,
            passenger_id: null, class_type: null
        );

        // Assert
        result.Should().ContainSingle().Which.FlightId.Should().Be(10);
    }

    [Fact]
    public async Task FilteredBookingsAsync_FiltersByPassengerId()
    {
        var bookings = new List<Booking>
    {
        new Booking(1, 10, 100, ClassType.Economy),
        new Booking(2, 20, 200, ClassType.Business)
    };

        var flights = new List<Flight>
    {
        new Flight(10, "DE", "IT", "BER", "ROM", DateTime.Today, 100, 200, 300),
        new Flight(20, "DE", "FR", "BER", "PAR", DateTime.Today, 100, 200, 300)
    };

        _mockBookingsData.Setup(b => b.Bookings).Returns(bookings);
        _mockPassengerService.Setup(p => p.SearchAvailableFlights(null, null, null, null, null, null, null))
            .Returns(flights);
        _mockFlightImportService.Setup(f => f.ImportFlightsFromCSVAsync(true)).Returns(Task.CompletedTask);

        var result = await _bookingManagerService.FilteredBookingsAsync(
            id: null, max_price: null, departure_country: null, destination_country: null,
            departure_date: null, departure_airport: null, arrival_airport: null,
            passenger_id: 200, class_type: null
        );

        result.Should().ContainSingle().Which.PassengerId.Should().Be(200);
    }

    [Fact]
    public async Task FilteredBookingsAsync_FiltersByClassType()
    {
        var bookings = new List<Booking>
    {
        new Booking(1, 10, 101, ClassType.Business),
        new Booking(2, 11, 102, ClassType.Economy)
    };

        var flights = new List<Flight>
    {
        new Flight(10, "US", "UK", "JFK", "LHR", DateTime.Today, 100, 200, 300),
        new Flight(11, "US", "FR", "JFK", "CDG", DateTime.Today, 100, 200, 300)
    };

        _mockBookingsData.Setup(b => b.Bookings).Returns(bookings);
        _mockPassengerService.Setup(p => p.SearchAvailableFlights(null, null, null, null, null, null, "Business"))
            .Returns(flights);
        _mockFlightImportService.Setup(f => f.ImportFlightsFromCSVAsync(true)).Returns(Task.CompletedTask);

        var result = await _bookingManagerService.FilteredBookingsAsync(
            id: null, max_price: null, departure_country: null, destination_country: null,
            departure_date: null, departure_airport: null, arrival_airport: null,
            passenger_id: null, class_type: "Business"
        );

        result.Should().ContainSingle().Which.ClassType.Should().Be(ClassType.Business);
    }

    [Fact]
    public async Task FilteredBookingsAsync_FiltersByDepartureCountry()
    {
        var bookings = new List<Booking>
    {
        new Booking(1, 10, 101, ClassType.Business),
        new Booking(2, 20, 102, ClassType.Economy)
    };

        var flights = new List<Flight>
    {
        new Flight(10, "Egypt", "Italy", "CAI", "ROM", DateTime.Today, 100, 200, 300),
    };

        _mockBookingsData.Setup(b => b.Bookings).Returns(bookings);
        _mockPassengerService.Setup(p => p.SearchAvailableFlights(null, "Egypt", null, null, null, null, null))
            .Returns(flights);
        _mockFlightImportService.Setup(f => f.ImportFlightsFromCSVAsync(true)).Returns(Task.CompletedTask);

        var result = await _bookingManagerService.FilteredBookingsAsync(
            id: null, max_price: null, departure_country: "Egypt", destination_country: null,
            departure_date: null, departure_airport: null, arrival_airport: null,
            passenger_id: null, class_type: null
        );

        result.Should().ContainSingle().Which.FlightId.Should().Be(10);
    }

}

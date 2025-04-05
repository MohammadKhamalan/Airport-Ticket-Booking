using Moq;
using FluentAssertions;
using Xunit;
using AirportTicketBooking.Services;
using AirportTicketBooking.AirportTicketBooking.Models;
using AirportTicketBooking.AirportTicketBooking.Models.Enums;
using AirportTicketBooking.AirportTicketBooking.Interfaces;
using AirportTicketBooking.Interfaces;
using AirportTicketBooking;
namespace AirportTicketBooking.Tests
{
    public class PassengerServiceTests
    {
        private readonly Mock<IBookingsData> _mockBookingData;
        private readonly Mock<IFlightsData> _mockFlightData;
        private readonly PassengerService _passengerService;

        public PassengerServiceTests()
        {
            _mockBookingData = new Mock<IBookingsData>();
            _mockFlightData = new Mock<IFlightsData>();
            _passengerService = new PassengerService(_mockBookingData.Object, _mockFlightData.Object);
        }
        [Fact]
        public void Book_ShouldAddBooking_WhenBookingDoesNotExist()
        {
            // Arrange
            var existingBookings = new List<Booking>();
            _mockBookingData.Setup(m => m.Bookings).Returns(existingBookings);
            _mockBookingData.Setup(m => m.SaveBookings()).Verifiable();

            int flightId = 1;
            int passengerId = 1;
            ClassType classType = ClassType.Economy;

            // Act
            _passengerService.Book(flightId, passengerId, classType);

            // Assert
            existingBookings.Count.Should().Be(1, "because a new booking should be added");
            existingBookings.First().FlightId.Should().Be(flightId, "because the flightId should match");
            existingBookings.First().PassengerId.Should().Be(passengerId, "because the passengerId should match");
            existingBookings.First().ClassType.Should().Be(classType, "because the classType should match");
            _mockBookingData.Verify(m => m.SaveBookings(), Times.Once, "because SaveBookings should be called once");
        }

        [Fact]
        public void CancelBook_ShouldRemoveBooking_WhenBookingExists()
        {
            // Arrange
            var existingBookings = new List<Booking>
        {
            new Booking(1, 1, 1, ClassType.Economy)
        };

            _mockBookingData.Setup(m => m.Bookings).Returns(existingBookings);
            _mockBookingData.Setup(m => m.SaveBookings()).Verifiable();

            int bookingIdToCancel = 1;

            // Act
            _passengerService.CancelBook(bookingIdToCancel);

            // Assert
            existingBookings.Should().BeEmpty("because the booking with the specified ID should be removed");
            _mockBookingData.Verify(m => m.SaveBookings(), Times.Once, "because SaveBookings should be called once after cancellation");
        }
        [Fact]
        public void CancelBook_ShouldNotRemoveBooking_WhenBookingDoesNotExist()
        {
            // Arrange
            var existingBookings = new List<Booking>
        {
            new Booking(1, 1, 1, ClassType.Economy)
        };

            _mockBookingData.Setup(m => m.Bookings).Returns(existingBookings);
            _mockBookingData.Setup(m => m.SaveBookings()).Verifiable();

            int nonExistingBookingId = 2;

            // Act
            _passengerService.CancelBook(nonExistingBookingId);

            // Assert
            existingBookings.Should().HaveCount(1, "because the booking list should remain unchanged when the booking does not exist");
            _mockBookingData.Verify(m => m.SaveBookings(), Times.Never, "because SaveBookings should not be called if the booking does not exist");
        }
        [Fact]
        public void ViewPersonalBookings_ShouldReturnBookings_WhenBookingsExist()
        {
            // Arrange
            var existingBookings = new List<Booking>
            {
                new Booking(1, 1, 1, ClassType.Economy),
                new Booking(2, 1, 2, ClassType.Business)
            };
            _mockBookingData.Setup(m => m.Bookings).Returns(existingBookings);

            int passengerId = 1;

            // Act
            var personalBookings = _passengerService.ViewPersonalBookings(passengerId);

            // Assert
            personalBookings.Should().HaveCount(1, "because there are one booking for this passenger");
           
        }
        [Fact]
        public void ModifyBook_ShouldModifyBooking_WhenBookingExists()
        {
            // Arrange
            var existingBookings = new List<Booking>
    {
        new Booking(1, 1, 1, ClassType.Economy)
    };

            _mockBookingData.Setup(m => m.Bookings).Returns(existingBookings);
            _mockBookingData.Setup(m => m.SaveBookings()).Verifiable();

            var consoleReaderMock = new Mock<IConsoleReader>();
            consoleReaderMock.SetupSequence(m => m.ReadLine())
                .Returns("2")  
                .Returns("2")  
                .Returns("Business"); 

            // Act
            _passengerService.ModifyBook(1, consoleReaderMock.Object);

            // Assert
            var modifiedBooking = existingBookings.FirstOrDefault(b => b.Id == 1);
            modifiedBooking.Should().NotBeNull("because the booking should be modified");
            modifiedBooking.FlightId.Should().Be(2, "because the FlightId should be updated");
            modifiedBooking.PassengerId.Should().Be(2, "because the PassengerId should be updated");
            modifiedBooking.ClassType.Should().Be(ClassType.Business, "because the ClassType should be updated");

            _mockBookingData.Verify(m => m.SaveBookings(), Times.Once, "because SaveBookings should be called after modifying the booking");
        }

        [Fact]
        public void SearchAvailableFlights_ShouldReturnFlights_WhenSearchingByDepartureCountry()
        {
            // Arrange
            var flights = new List<Flight>
    {
        new Flight(1, "USA", "UK", "JFK", "LHR", DateTime.Now.AddDays(1), 500, 1000, 1500),
        new Flight(2, "Canada", "France", "YYZ", "CDG", DateTime.Now.AddDays(2), 400, 800, 1200),
        new Flight(3, "USA", "Germany", "LAX", "FRA", DateTime.Now.AddDays(3), 550, 1100, 1600)
    };

            _mockFlightData.Setup(m => m.GetFlights()).Returns(flights);

            string departureCountry = "USA"; 

            // Act
            var result = _passengerService.SearchAvailableFlights(null, departureCountry, null, null, null, null, null);

            // Assert
            result.Should().HaveCount(2, "because there are two flights departing from the USA");
            result.All(f => f.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase))
                .Should().BeTrue("because all returned flights should have the specified departure country");
        }
        [Fact]
        public void SearchAvailableFlights_ShouldReturnFlights_WhenSearchingByMaxPrice()
        {
            // Arrange
            var flights = new List<Flight>
    {
        new Flight(1, "USA", "UK", "JFK", "LHR", DateTime.Now.AddDays(1), 500, 1000, 1500),
        new Flight(2, "Canada", "France", "YYZ", "CDG", DateTime.Now.AddDays(2), 400, 800, 1200),
        new Flight(3, "USA", "Germany", "LAX", "FRA", DateTime.Now.AddDays(3), 550, 1100, 1600)
    };
            _mockFlightData.Setup(m => m.GetFlights()).Returns(flights);
            double maxPrice = 500;
            //Act
            var result = _passengerService.SearchAvailableFlights(maxPrice, null, null, null, null, null, null);
            //Assert
            result.Should().HaveCount(2, "because there are two flights thats there price is equal to 500 or less");
            result.All(f => f.EconomyPrice <= maxPrice).Should().BeTrue("because all returned flights should have Economy prices less than or equal to the max price");

        }
    }
}

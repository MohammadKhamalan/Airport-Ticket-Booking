using AirportTicketBooking.AirportTicketBooking.Interfaces;
using AirportTicketBooking.Services;
using AirportTicketBooking;
using FluentAssertions;
using Moq;
using Xunit;

public class FlightDisplayServiceTests
{
    private readonly Mock<IFlightsData> _mockFlightsData;
    private readonly FlightDisplayService _flightDisplayService;

    public FlightDisplayServiceTests()
    {
        _mockFlightsData = new Mock<IFlightsData>();
        _flightDisplayService = new FlightDisplayService(_mockFlightsData.Object);
    }

    [Fact]
    public void DisplaySearchResults_ShouldPrintFlightDetails_WhenListIsNotEmpty()
    {
        // Arrange
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);  

        var flights = new List<Flight>
        {
            new Flight(1, "USA", "UK", "JFK", "LHR", DateTime.Today.AddDays(1), 200, 400, 600)
        };

        // Act
        _flightDisplayService.DisplaySearchResults(flights);

        // Assert
        var output = stringWriter.ToString();
        output.Should().Contain("Flight ID: 1");
        output.Should().Contain("From: USA (JFK)");
        output.Should().Contain("To: UK (LHR)");
        

        Console.SetOut(new StringWriter());  // Reset Console output
    }

    [Fact]
    public void DisplayFlights_ShouldPrintAllFlights()
    {
        // Arrange
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var flights = new List<Flight>
        {
            new Flight(2, "France", "Germany", "CDG", "BER", DateTime.Today.AddDays(2), 150, 300, 450)
        };

        _mockFlightsData.Setup(f => f.GetFlights()).Returns(flights);

        // Act
        _flightDisplayService.DisplayFlights();

        // Assert
        var output = stringWriter.ToString();
        output.Should().Contain("Flight ID: 2 from France to Germany");

        Console.SetOut(new StringWriter());  // Reset Console output
    }

   
}

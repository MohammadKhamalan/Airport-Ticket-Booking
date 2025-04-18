using AirportTicketBooking.AirportTicketBooking.Interfaces;
using AirportTicketBooking;
using FluentAssertions;
using Moq;
using Xunit;

public class FlightImportServiceTests
{
    private Mock<IFlightsData> _mockFlightsData;
    private FlightImportService _flightImportService;
    public readonly string _flightPath = @"C:\Users\ASUS\Desktop\Airport Ticket Booking\AirportTicketBooking\Data\Flight.csv";

    public FlightImportServiceTests()
    {
        _mockFlightsData = new Mock<IFlightsData>();


        _mockFlightsData.Setup(f => f.GetFlights()).Returns(new List<Flight>());


        var fileExistsMock = new Mock<Func<string, bool>>();
        fileExistsMock.Setup(f => f(It.IsAny<string>())).Returns(false);


        _flightImportService = new FlightImportService(_mockFlightsData.Object, fileExistsMock.Object);
    }

 
    [Fact]
    public async Task ImportFlightsFromCSVAsync_ShouldNotImport_WhenFileDoesNotExist()
    {
        // Arrange
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

       
        var fileExistsMock = new Mock<Func<string, bool>>();
        fileExistsMock.Setup(f => f(It.IsAny<string>())).Returns(false);

        var flightImportService = new FlightImportService(_mockFlightsData.Object, fileExistsMock.Object);

        // Act
        await flightImportService.ImportFlightsFromCSVAsync(true);

        // Assert
        var consoleOutput = stringWriter.ToString();
        consoleOutput.Should().Contain("Flight data file not found.");
    }

    [Fact]
    public async Task ImportFlightsFromCSVAsync_ShouldImport_WhenFileExists()
    {
        // Arrange
        var mockFlights = new List<Flight>
    {
        new Flight(1, "USA", "UK", "NYC", "London", DateTime.Today.AddDays(1), 500, 1000, 1500)
    };

        
        var fileExistsMock = new Mock<Func<string, bool>>();
        fileExistsMock.Setup(f => f(It.IsAny<string>())).Returns(true);

        var flightImportService = new FlightImportService(_mockFlightsData.Object, fileExistsMock.Object);

        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        // Act
        await flightImportService.ImportFlightsFromCSVAsync(true);

        // Assert
        var consoleOutput = stringWriter.ToString();
        consoleOutput.Should().Contain("Loading completed with errors:");
    }

}

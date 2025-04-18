using AirportTicketBooking.AirportTicketBooking.Models.Enums;
using AirportTicketBooking.Services;
using System.IO;
using Xunit;
using System.Linq;

namespace AirportTicketBooking.Tests
{
    public class BookingDataServiceTests
    {
        [Fact]
        public void Load_Bookings_Should_Load_Correct_Data_From_CSV()
        {
            // Arrange
            var tempPath = Path.GetTempFileName();
            File.WriteAllLines(tempPath, new[]
            {
                "BookingId,FlightId,PassengerId,ClassType",
                "1,101,201,Economy",
                "2,102,202,Business",
                "3,103,203,FirstClass"
            });

            var service = new BookingDataServiceTestable(tempPath);

            // Act
            service.Load_Bookings();

            // Assert
            Assert.Equal(3, service.Bookings.Count);
            Assert.Contains(service.Bookings, b => b.ClassType == ClassType.Economy && b.Id == 1);
            Assert.Contains(service.Bookings, b => b.ClassType == ClassType.Business && b.Id == 2);
            Assert.Contains(service.Bookings, b => b.ClassType == ClassType.FirstClass && b.Id == 3);

            // Clean up
            File.Delete(tempPath);
        }
        [Fact]
        public void SaveBookings_Should_Write_Correct_Data_To_CSV()
        {
            // Arrange
            var tempPath = Path.GetTempFileName();
            var service = new BookingDataServiceTestable(tempPath);

            service.Bookings.AddRange(new[]
            {
                new Booking(1, 101, 201, ClassType.Economy),
                new Booking(2, 102, 202, ClassType.Business)
            });

            // Act
            service.SaveBookings();

            // Assert
            var lines = File.ReadAllLines(tempPath);
            Assert.Equal(3, lines.Length); // header + 2 bookings
            Assert.Equal("BookingId,FlightId,PassengerId,ClassType", lines[0]);
            Assert.Contains("1,101,201,Economy", lines);
            Assert.Contains("2,102,202,Business", lines);

            // Clean up
            File.Delete(tempPath);
        }
    }

    
    public class BookingDataServiceTestable : BookingDataService
    {
        public BookingDataServiceTestable(string testPath)
        {
            typeof(BookingDataService)
                .GetField("_bookingPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(this, testPath);
        }
    }
}

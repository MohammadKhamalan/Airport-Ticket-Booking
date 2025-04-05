using AirportTicketBooking.AirportTicketBooking.Models.Enums;
using AirportTicketBooking.Services;
using AirportTicketBooking.AirportTicketBooking.Interfaces;
using System.Collections.Generic;
using Xunit;
using Moq;
using System.IO;
using System;
using FluentAssertions;

namespace AirportTicketBooking.Tests
{
    public class BookingDisplayServiceTests
    {
        [Fact]
        public void DisplayFilteredBookings_Should_Display_Only_Bookings_Within_MaxPrice()
        {
            // Arrange
            var bookings = new List<Booking>
    {
        new Booking(1, 101, 1001, ClassType.Business),     
        new Booking(2, 102, 1002, ClassType.FirstClass),  
        new Booking(3, 103, 1003, ClassType.FirstClass), 
    };

            
            var flights = new List<Flight>
    {
        new Flight(101, "US", "UK", "JFK", "LHR", DateTime.Today, 90, 200, 300),  
        new Flight(102, "US", "FR", "JFK", "CDG", DateTime.Today, 90, 250, 400),   
        new Flight(103, "US", "DE", "JFK", "BER", DateTime.Today, 80, 170, 500),   
    };

            
            var mockBookingsData = new Mock<IBookingsData>();
            mockBookingsData.Setup(x => x.Bookings).Returns(bookings);

            var mockFlightsData = new Mock<IFlightsData>();
            mockFlightsData.Setup(x => x.GetFlights()).Returns(flights);

            var service = new BookingDisplayService(mockBookingsData.Object, mockFlightsData.Object);

          
            using var sw = new StringWriter();
            Console.SetOut(sw);

            // Act
            service.DisplayFilteredBookings(bookings, 100);  

            // Assert
            var output = sw.ToString().Trim();  

           
            output.Should().NotContain("Booking ID: 1");  
            output.Should().NotContain("Booking ID: 2");  
            output.Should().NotContain("Booking ID: 3");  

           
        }


    }
}

using AirportTicketBooking.AirportTicketBooking.Models.Enums;

public record Booking(int Id, int FlightId, int PassengerId, ClassType ClassType);


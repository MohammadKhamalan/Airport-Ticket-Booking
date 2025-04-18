using System;

namespace AirportTicketBooking
{
    public record Flight(
        int FlightId,
        string DepartureCountry,
        string DestinationCountry,
        string DepartureAirport,
        string ArrivalAirport,
        DateTime DepartureDate,
        double EconomyPrice,
        double BusinessPrice,
        double FirstClassPrice
    );
}

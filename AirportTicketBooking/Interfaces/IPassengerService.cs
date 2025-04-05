using AirportTicketBooking.AirportTicketBooking.Models.Enums;
using AirportTicketBooking.Interfaces;
using System;
using System.Collections.Generic;

namespace AirportTicketBooking.AirportTicketBooking.Interfaces
{
    public interface IPassengerService
    {
        void Book(int flight_id, int passenger_id, ClassType classType);
        List<Booking> ViewPersonalBookings(int passenger_id);
        //void ModifyBook(int bookingId);
        void ModifyBook(int bookingId, IConsoleReader consoleReader);
        void CancelBook(int bookingId);
        List<Flight> SearchAvailableFlights(double? maxprice, string departure_country, string destination_country,
           DateTime? departure_date, string departure_airport, string arrival_airport, string classType);
    }
}
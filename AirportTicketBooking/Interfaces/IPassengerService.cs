using AirportTicketBooking.AirportTicketBooking.Models.Enums;
using System;
using System.Collections.Generic;

namespace AirportTicketBooking.AirportTicketBooking.Interfaces
{
    public interface IPassengerService
    {
        void Book(int flight_id, int passenger_id, ClassType classType);
        List<Booking> ViewPersonalBookings(int passenger_id);
        void ModifyBook(int bookingId);
        void CancelBook(int bookingId);
        List<Flight> Search_Available_Flights(double? maxprice, string departure_country, string destination_country,
           DateTime? departure_date, string departure_airport, string arrival_airport, string classType);
    }
}
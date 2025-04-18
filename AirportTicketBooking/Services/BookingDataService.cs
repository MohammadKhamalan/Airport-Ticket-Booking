using AirportTicketBooking.AirportTicketBooking.Interfaces;
using AirportTicketBooking.AirportTicketBooking.Models.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AirportTicketBooking.Services
{
    public class BookingDataService : IBookingsData
    {
        public List<Booking> Bookings { get; } = new List<Booking>();
        private string _bookingPath = @"C:\Users\ASUS\Desktop\Airport Ticket Booking\AirportTicketBooking\Data\Booking.csv";

        public void Load_Bookings()
        {
            Bookings.Clear();

            if (File.Exists(_bookingPath))
            {
                var lines = File.ReadAllLines(_bookingPath).Skip(1);

                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    if (data.Length >= 4 && Enum.TryParse<ClassType>(data[3].Trim(), out var classType))
                    {
                        int bookingId = int.Parse(data[0]);
                        int flightId = int.Parse(data[1]);
                        int passengerId = int.Parse(data[2]);

                        if (!Bookings.Any(b => b.Id == bookingId))
                        {
                            Bookings.Add(new Booking(bookingId, flightId, passengerId, classType));
                        }
                    }
                }
            }
        }

        public void SaveBookings()
        {
            using (StreamWriter sw = new StreamWriter(_bookingPath, false))
            {
                sw.WriteLine("BookingId,FlightId,PassengerId,ClassType");
                foreach (var booking in Bookings)
                {
                    sw.WriteLine($"{booking.Id},{booking.FlightId},{booking.PassengerId},{booking.ClassType}");
                }
            }
        }
    }
}
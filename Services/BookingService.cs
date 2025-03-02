using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Airport_Ticket_Booking.Services
{
    class BookingService
    {
        private List<Booking> Bookings = new List<Booking>();
        string Booking_path = @"C:\Users\ASUS\Desktop\Airport Ticket Booking\Data\Booking.csv";
        public BookingService()
        {
            Load_Bookings();
        }
        public void Load_Bookings()
        {
            if (File.Exists(Booking_path))
            {
                var lines = File.ReadAllLines(Booking_path).Skip(1);

                foreach (var line in lines)
                {
                    var data = line.Split(',');

                    Bookings.Add(new Booking(int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]), data[3]));


                }
            }
        }
        public void Book(int flight_id, int passenger_id, string classType)
        {
            int NewBookingId = (Bookings.Count > 0) ? Bookings.Max(b => b.Id + 1) : 1;
            Bookings.Add(new Booking(NewBookingId, flight_id, passenger_id, classType));
            using (StreamWriter sw = new StreamWriter(Booking_path, true))
            {
                sw.WriteLine($"{NewBookingId},{flight_id},{passenger_id},{classType}");
            }
            Console.WriteLine($"Booking {NewBookingId} added successfully!");

        }
        public void Modify_Book(int bookingId)
        {
            var booking = Bookings.Find(booking => booking.Id == bookingId);

            if (booking != null)
            {
                Console.WriteLine("Enter New Flight ID:");
                booking.FlightId = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter New Passenger ID:");
                booking.PassengerId = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter New Class type:");
                booking.ClassType = Console.ReadLine();

                Console.WriteLine($"Booking {bookingId} modified successfully!");

                Save_Bookings();
            }
            else
            {
                Console.WriteLine($"Booking with ID {bookingId} not found.");
            }
        }

        public void Cancel_Book(int bookingId)
        {
            var booking = Bookings.Find(booking => booking.Id == bookingId);
            if (booking != null)
            {
                Bookings.Remove(booking);
                Save_Bookings();
                Console.WriteLine($"Booking {bookingId} has been canceled successfully.");
            }
            else
            {
                Console.WriteLine("Booking not found.");
            }
        }

        private void Save_Bookings()
        {
            using (StreamWriter sw = new StreamWriter(Booking_path, false))
            {
                sw.WriteLine("BookingId,FlightId,PassengerId,ClassType");
                foreach (var booking in Bookings)
                {
                    sw.WriteLine($"{booking.Id},{booking.FlightId},{booking.PassengerId},{booking.ClassType}");
                }
            }
        }


        public void Display_Bookings()
        {
            foreach (var booking in Bookings)
            {
                Console.WriteLine($"Booking {booking.Id}: Flight {booking.FlightId}, Class: {booking.ClassType}");

            }
        }

    }
}
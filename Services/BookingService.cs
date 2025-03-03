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
        FlightService flights = new FlightService();
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
        public List<Booking> View_Personal_Bookings(int passenger_id)
        {
            var personal_bookings = Bookings.Where(booking => booking.PassengerId == passenger_id).ToList(); // Convert to List

            if (personal_bookings.Count == 0)
            {
                Console.WriteLine("No bookings found for this passenger.");
            }
            else
            {
                foreach (var booking in personal_bookings)
                {
                    Console.WriteLine($"Booking {booking.Id}: Flight {booking.FlightId}, Class: {booking.ClassType}");
                }
            }

            return personal_bookings; // Now the method returns a List<Booking>
        }

        public void Modify_Book(int bookingId)
        {
            var booking = Bookings.Find(booking => booking.Id == bookingId);

            if (booking != null)
            {
                Console.WriteLine("Enter New Flight ID (Press Enter to keep current):");
                string flightInput = Console.ReadLine();
                if (int.TryParse(flightInput, out int newFlightId))
                {
                    booking.FlightId = newFlightId;
                }

                Console.WriteLine("Enter New Passenger ID (Press Enter to keep current):");
                string passengerInput = Console.ReadLine();
                if (int.TryParse(passengerInput, out int newPassengerId))
                {
                    booking.PassengerId = newPassengerId;
                }

                Console.WriteLine("Enter New Class Type (Press Enter to keep current):");
                string newClassType = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newClassType))
                {
                    booking.ClassType = newClassType;
                }

                Console.WriteLine($"Booking {booking.Id} modified successfully!");

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
        public List<Booking> filtered_Bookings(int? id, double? max_price, string departure_country, string destination_country, DateTime? departure_date,
     string departure_airport, string arrival_airport, int? passenger_id, string class_type)
        {
            flights.ImportFlightsFromCSV(true);
            var result_booking = Bookings
                .Where(booking =>
                    (!id.HasValue || booking.FlightId == id.Value) &&
                    (!passenger_id.HasValue || booking.PassengerId == passenger_id.Value) &&
                    (string.IsNullOrEmpty(class_type) || booking.ClassType.Equals(class_type, StringComparison.OrdinalIgnoreCase))
                ).ToList();

            if (result_booking.Count == 0)
            {
                Console.WriteLine("No bookings match your search criteria.");
                return new List<Booking>();
            }

           
            var filteredFlights = flights.Search_Available_Flights(max_price, departure_country, destination_country, departure_date, departure_airport, arrival_airport, class_type);

            if (filteredFlights.Count == 0)
            {
                Console.WriteLine("No flights match your search criteria.");
                return new List<Booking>();
            }

            
            result_booking = result_booking
                .Where(booking => filteredFlights.Any(flight => flight.FlightId == booking.FlightId))
                .ToList();

            if (result_booking.Count == 0)
            {
                Console.WriteLine("No bookings match the available flights.");
                return new List<Booking>();
            }




            if (max_price.HasValue)
            {
                DisplayFilteredBookings(result_booking, max_price.Value);
            }
            else
            {
                DisplayFilteredBookings(result_booking, double.MaxValue); 
            }

            return result_booking;
        }

        public void DisplayFilteredBookings(List<Booking> bookings, double maxPrice)
        {
            if (bookings.Count == 0)
            {
                Console.WriteLine("No matching bookings found.");
                return;
            }

            var bookingWithPrices = from booking in bookings
                                    join flight in flights.GetFlights() on booking.FlightId equals flight.FlightId
                                    let price = booking.ClassType.Equals("Economy", StringComparison.OrdinalIgnoreCase) ? flight.EconomyPrice :
                                                booking.ClassType.Equals("Business", StringComparison.OrdinalIgnoreCase) ? flight.BusinessPrice :
                                                flight.FirstClassPrice 
                                    where price <= maxPrice

                                    select new
                                    {
                                        booking.Id,
                                        booking.FlightId,
                                        booking.PassengerId,
                                        booking.ClassType,
                                        Price = price
                                    };

            if (!bookingWithPrices.Any()) 
            {
                Console.WriteLine("No matching bookings found within the price limit.");
                return;
            }

            Console.WriteLine("Filtered Bookings with Prices:");
            Console.WriteLine("-------------------------------------------------------------");
            foreach (var booking in bookingWithPrices)
            {
                Console.WriteLine($"Booking ID: {booking.Id}");
                Console.WriteLine($"Flight ID: {booking.FlightId}");
                Console.WriteLine($"Passenger ID: {booking.PassengerId}");
                Console.WriteLine($"Class: {booking.ClassType}");
                Console.WriteLine($"Price: {booking.Price} USD");
                Console.WriteLine("-------------------------------------------------------------");
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
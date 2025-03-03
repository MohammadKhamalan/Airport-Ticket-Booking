using System;
using System.Collections.Generic;
using System.Text;

namespace Airport_Ticket_Booking
{
    class Flight
    {
        public int FlightId { get; set; }
        public string DepartureCountry { get; set; }
        public string DestinationCountry { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTime DepartureDate { get; set; }
        public double EconomyPrice { get; set; }
        public double BusinessPrice { get; set; }
        public double FirstClassPrice { get; set; }
        
        public Flight(int id, string departure_country, string destination_country, string departure_airport, string arrival_airport, DateTime departure_date, double economy_price, double business_price, double first_class_price)
        {
            FlightId = id;
            DepartureCountry = departure_country;
            DestinationCountry = destination_country;
            DepartureAirport = departure_airport;
            ArrivalAirport = arrival_airport;
            DepartureDate = departure_date;
            EconomyPrice = economy_price;
            BusinessPrice = business_price;
            FirstClassPrice = first_class_price;
        }
    }
}

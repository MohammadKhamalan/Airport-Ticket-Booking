using System;
using System.Collections.Generic;
using System.Text;

namespace Airport_Ticket_Booking
{
    class Passenger
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Passenger(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}

using Airport_Ticket_Booking.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Airport_Ticket_Booking
{
 
        class Booking
        {
            public int Id { get; set; }
            public int FlightId { get; set; }
            public int PassengerId { get; set; }
            public ClassType ClassType { get; set; } 

            public Booking(int id, int flightId, int passengerId, ClassType classType)
            {
                Id = id;
                FlightId = flightId;
                PassengerId = passengerId;
                ClassType = classType;
            }
        

    }
}

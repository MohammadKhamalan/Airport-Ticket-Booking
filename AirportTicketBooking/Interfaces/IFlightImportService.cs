using System.Threading.Tasks;

namespace AirportTicketBooking.AirportTicketBooking.Interfaces
{
    public interface IFlightImportService
    {
        Task ImportFlightsFromCSVAsync(bool user);
    }
}
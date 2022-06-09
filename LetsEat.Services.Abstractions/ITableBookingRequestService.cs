using LetsEat.Models;

namespace LetsEat.Services
{
    public interface ITableBookingRequestService
    {
        TableBookingResult BookTable(TableBookingRequest request);
    }
}
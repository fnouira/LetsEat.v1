using LetsEat.Models;

namespace LetsEat.Services
{
    public interface ITableBookingProcessorService
    {
        TableBookingResult BookTable(TableBookingRequest request);
    }
}
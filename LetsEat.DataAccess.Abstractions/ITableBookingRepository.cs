using LetsEat.Models;

namespace LetsEat.DataAccess.Abstractions
{
    public interface ITableBookingRepository
    {
         void Save(TableBookingDao tableBooking);
    }
}
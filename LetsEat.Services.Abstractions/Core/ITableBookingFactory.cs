using LetsEat.Models;

namespace LetsEat.Services.Core
{
    public interface ITableBookingFactory
    {
        T Create<T>(TableBookingRequest request) where T : TableBookingBase, new();
    }
}
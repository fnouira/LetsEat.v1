using LetsEat.Models;

namespace LetsEat.Services.Core
{
    internal class TableBookingFactory : ITableBookingFactory
    {
        public T Create<T>(TableBookingRequest request)
            where T : TableBookingBase, new()
        {
            return new T
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Tel = request.Tel,
                Email = request.Email,
                Date = request.Date
            };
        }
    }
}

using LetsEat.DataAccess.Abstractions;
using LetsEat.Models;

namespace LetsEat.Services
{
    internal class TableBookingProcessorService : ITableBookingProcessorService
    {
        private readonly ITableBookingRepository tableBookingRepository;
        private readonly ITableRepository tableRepository;

        public TableBookingProcessorService(ITableBookingRepository tableBookingRepository, ITableRepository tableRepository)
        {
            this.tableRepository = tableRepository;
            this.tableBookingRepository = tableBookingRepository;
        }

        public TableBookingResult BookTable(TableBookingRequest request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            TableBookingResult result = Create<TableBookingResult>(request);

            IReadOnlyCollection<TableDao> availableTables = tableRepository.GetAvailableTables(request.Date);
            if (availableTables.FirstOrDefault() is TableDao availableTable)
            {
                TableBookingDao tableBooking = Create<TableBookingDao>(request);
                tableBooking.TableId = availableTable.Id;
                tableBookingRepository.Save(tableBooking);
                result.TableBookingId = tableBooking.Id;
                result.Status = TableBookingStatus.Success;
            }
            else
            {
                result.Status = TableBookingStatus.NoTableAvailable;
            }

            return result;
        }

        private T Create<T>(TableBookingRequest request)
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
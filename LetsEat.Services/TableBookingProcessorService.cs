using LetsEat.DataAccess.Abstractions;
using LetsEat.Models;
using LetsEat.Services.Core;

namespace LetsEat.Services
{
    /// <summary>
    /// This should always be internal as we need to use dependency injection to use it
    /// <see cref="ServiceCollectionExtensions" for more information/>
    /// </summary>
    internal class TableBookingProcessorService : ITableBookingProcessorService
    {
        private readonly ITableBookingFactory tableBookingFactory;
        private readonly ITableBookingRepository tableBookingRepository;
        private readonly ITableRepository tableRepository;

        public TableBookingProcessorService(ITableBookingFactory tableBookingFactory, ITableBookingRepository tableBookingRepository, ITableRepository tableRepository)
        {
            this.tableRepository = tableRepository;
            this.tableBookingFactory = tableBookingFactory;
            this.tableBookingRepository = tableBookingRepository;
        }

        public TableBookingResult BookTable(TableBookingRequest request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }

            TableBookingResult result = tableBookingFactory.Create<TableBookingResult>(request);

            IReadOnlyCollection<TableDao> availableTables = tableRepository.GetAvailableTables(request.Date);
            if (availableTables.FirstOrDefault() is TableDao availableTable)
            {
                TableBookingDao tableBooking = tableBookingFactory.Create<TableBookingDao>(request);
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
    }
}
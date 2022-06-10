using LetsEat.DataAccess.Abstractions;
using LetsEat.Models;

namespace LetsEat.Services
{
    [TestFixture]
    internal class TableBookingProcessorServiceTest
    {
        [Test]
        public void book_table_should_return_reservation_info()
        {
            TableBookingRequest request = new TableBookingRequest
            {
                FirstName = "first name",
                LastName = "last name",
                Tel = "tel",
                Email = "email",
                Date = new DateTime(2022, 05, 15)
            };

            Mock<ITableBookingRepository> mockedTableBookingRepository = new Mock<ITableBookingRepository>();
            Mock<ITableRepository> mockedTableRepository = new Mock<ITableRepository>();
            mockedTableRepository.Setup(m => m.GetAvailableTables(new DateTime(2022, 05, 15))).Returns(Array.Empty<TableDao>());
            ITableBookingProcessorService bookingService = new TableBookingProcessorService(mockedTableBookingRepository.Object, mockedTableRepository.Object);

            TableBookingResult actual = bookingService.BookTable(request);

            Assert.That(actual.FirstName, Is.EqualTo("first name"));
            Assert.That(actual.LastName, Is.EqualTo("last name"));
            Assert.That(actual.Tel, Is.EqualTo("tel"));
            Assert.That(actual.Email, Is.EqualTo("email"));
            Assert.That(actual.Date, Is.EqualTo(new DateTime(2022, 05, 15)));
        }

        [Test]
        public void book_table_should_throw_exception_if_request_is_null()
        {
            Mock<ITableBookingRepository> mockedTableBookingRepository = new Mock<ITableBookingRepository>();
            Mock<ITableRepository> mockedTableRepository = new Mock<ITableRepository>();
            mockedTableRepository.Setup(m => m.GetAvailableTables(new DateTime(2022, 05, 15))).Returns(Array.Empty<TableDao>());
            ITableBookingProcessorService bookingService = new TableBookingProcessorService(mockedTableBookingRepository.Object, mockedTableRepository.Object);

            TableBookingRequest request = null;

            var exception = Assert.Throws<ArgumentNullException>(() => bookingService.BookTable(request));

            Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'request')"));
        }

        [Test]
        public void book_table_should_save_table_booking()
        {
            TableBookingRequest request = new TableBookingRequest
            {
                FirstName = "first name",
                LastName = "last name",
                Tel = "tel",
                Email = "email",
                Date = new DateTime(2022, 05, 15)
            };
            IReadOnlyCollection<TableDao> availableTables = new List<TableDao> { new TableDao { Id = 7 } };

            TableBookingDao savedTableBooking = null;
            Mock<ITableBookingRepository> mockedTableBookingRepository = new Mock<ITableBookingRepository>();
            mockedTableBookingRepository.Setup(m => m.Save(It.IsAny<TableBookingDao>())).Callback<TableBookingDao>(tableBooking =>
            {
                savedTableBooking = tableBooking;
            });
            Mock<ITableRepository> mockedTableRepository = new Mock<ITableRepository>();
            mockedTableRepository.Setup(m => m.GetAvailableTables(new DateTime(2022, 05, 15))).Returns(availableTables);

            ITableBookingProcessorService bookingService = new TableBookingProcessorService(mockedTableBookingRepository.Object, mockedTableRepository.Object);

            TableBookingResult actual = bookingService.BookTable(request);

            mockedTableBookingRepository.Verify(m => m.Save(It.IsAny<TableBookingDao>()), Times.Once);

            Assert.IsNotNull(savedTableBooking);

            Assert.That(savedTableBooking.FirstName, Is.EqualTo(request.FirstName));
            Assert.That(savedTableBooking.LastName, Is.EqualTo(request.LastName));
            Assert.That(savedTableBooking.Tel, Is.EqualTo(request.Tel));
            Assert.That(savedTableBooking.Email, Is.EqualTo(request.Email));
            Assert.That(savedTableBooking.Date, Is.EqualTo(request.Date));
            Assert.That(savedTableBooking.TableId, Is.EqualTo(availableTables.First().Id));
        }

        [Test]
        public void book_table_should_not_save_table_booking_if_no_table_available()
        {

            TableBookingRequest request = new TableBookingRequest
            {
                FirstName = "first name",
                LastName = "last name",
                Tel = "tel",
                Email = "email",
                Date = new DateTime(2022, 05, 15)
            };


            Mock<ITableBookingRepository> mockedTableBookingRepository = new Mock<ITableBookingRepository>();
            Mock<ITableRepository> mockedTableRepository = new Mock<ITableRepository>();
            // Make sure no table available
            mockedTableRepository.Setup(m => m.GetAvailableTables(new DateTime(2022, 05, 15))).Returns(Array.Empty<TableDao>());

            ITableBookingProcessorService bookingService = new TableBookingProcessorService(mockedTableBookingRepository.Object, mockedTableRepository.Object);

            TableBookingResult actual = bookingService.BookTable(request);

            mockedTableBookingRepository.Verify(m => m.Save(It.IsAny<TableBookingDao>()), Times.Never);
        }

        [TestCase(true, TableBookingStatus.Success)]
        [TestCase(false, TableBookingStatus.NoTableAvailable)]
        public void book_table_should_return_the_right_status_code(bool isTableAvailable, TableBookingStatus expectedResultStatus)
        {

            TableBookingRequest request = new TableBookingRequest
            {
                FirstName = "first name",
                LastName = "last name",
                Tel = "tel",
                Email = "email",
                Date = new DateTime(2022, 05, 15)
            };


            IReadOnlyCollection<TableDao> availableTables = isTableAvailable ? new List<TableDao> { new TableDao { Id = 7 } } : Array.Empty<TableDao>();

            Mock<ITableBookingRepository> mockedTableBookingRepository = new Mock<ITableBookingRepository>();
            Mock<ITableRepository> mockedTableRepository = new Mock<ITableRepository>();
            mockedTableRepository.Setup(m => m.GetAvailableTables(new DateTime(2022, 05, 15))).Returns(availableTables);

            ITableBookingProcessorService bookingService = new TableBookingProcessorService(mockedTableBookingRepository.Object, mockedTableRepository.Object);

            TableBookingResult actual = bookingService.BookTable(request);

            Assert.That(actual.Status, Is.EqualTo(expectedResultStatus));
        }

        [TestCase(true, 12)]
        [TestCase(false, null)]
        public void book_table_should_return_the_right_table_booking_id(bool isTableAvailable, int? expectedTableBookingId)
        {
            Mock<ITableBookingRepository> mockedTableBookingRepository = new Mock<ITableBookingRepository>();
            Mock<ITableRepository> mockedTableRepository = new Mock<ITableRepository>();

            TableBookingRequest request = new TableBookingRequest
            {
                FirstName = "first name",
                LastName = "last name",
                Tel = "tel",
                Email = "email",
                Date = new DateTime(2022, 05, 15)
            };


            IReadOnlyCollection<TableDao> availableTables = Array.Empty<TableDao>();

            if (isTableAvailable)
            {
                availableTables = new List<TableDao> { new TableDao { Id = 7 } };
                mockedTableBookingRepository.Setup(m => m.Save(It.IsAny<TableBookingDao>()))
                .Callback<TableBookingDao>(
                    tableBooking =>
                    {
                        if (expectedTableBookingId.HasValue)
                        {
                            tableBooking.Id = expectedTableBookingId.Value;
                        }
                    });
            }

            mockedTableRepository.Setup(m => m.GetAvailableTables(new DateTime(2022, 05, 15))).Returns(availableTables);

            ITableBookingProcessorService bookingService = new TableBookingProcessorService(mockedTableBookingRepository.Object, mockedTableRepository.Object);

            TableBookingResult actual = bookingService.BookTable(request);

            Assert.That(actual.TableBookingId, Is.EqualTo(expectedTableBookingId));
        }
    }
}
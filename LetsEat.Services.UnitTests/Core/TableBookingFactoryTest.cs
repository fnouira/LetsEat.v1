using LetsEat.Models;

namespace LetsEat.Services.Core
{
    [TestFixture]
    internal class TableBookingFactoryTest
    {
        [Test]
        public void Create_should_create_TableBookingResult_from_TableBookingRequest()
        {
            TableBookingRequest request = new TableBookingRequest
            {
                FirstName = "first name",
                LastName = "last name",
                Tel = "tel",
                Email = "email",
                Date = new DateTime(2022, 05, 15)
            };

            ITableBookingFactory factory = new TableBookingFactory();

            TableBookingResult actual = factory.Create<TableBookingResult>(request);

            Assert.That(actual.FirstName, Is.EqualTo("first name"));
            Assert.That(actual.LastName, Is.EqualTo("last name"));
            Assert.That(actual.Tel, Is.EqualTo("tel"));
            Assert.That(actual.Email, Is.EqualTo("email"));
            Assert.That(actual.Date, Is.EqualTo(new DateTime(2022, 05, 15)));
            Assert.That(actual.Status, Is.EqualTo(TableBookingStatus.None));
            Assert.IsNull(actual.TableBookingId);
        }

        [Test]
        public void Create_should_create_TableBookingDao_from_TableBookingRequest()
        {
            TableBookingRequest request = new TableBookingRequest
            {
                FirstName = "first name",
                LastName = "last name",
                Tel = "tel",
                Email = "email",
                Date = new DateTime(2022, 05, 15)
            };

            ITableBookingFactory factory = new TableBookingFactory();

            TableBookingDao actual = factory.Create<TableBookingDao>(request);

            Assert.That(actual.FirstName, Is.EqualTo("first name"));
            Assert.That(actual.LastName, Is.EqualTo("last name"));
            Assert.That(actual.Tel, Is.EqualTo("tel"));
            Assert.That(actual.Email, Is.EqualTo("email"));
            Assert.That(actual.Date, Is.EqualTo(new DateTime(2022, 05, 15)));
            Assert.That(actual.Id, Is.EqualTo(default(int)));
            Assert.That(actual.TableId, Is.EqualTo(default(int)));
        }
    }
}

namespace LetsEat.Models
{
    public class TableBookingResult : TableBookingBase
    {
        public TableBookingStatus Status { get; set; }
        public int? TableBookingId { get; set; }
    }
}
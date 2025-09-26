namespace Eventix.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public decimal Price { get; set; }

        public int PerformanceId { get; set; }
    }
}

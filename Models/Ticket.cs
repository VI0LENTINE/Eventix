namespace Eventix.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public decimal Price { get; set; }

        // Foreign key
        public int EventId { get; set; }
    }
}

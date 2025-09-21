namespace Eventix.Models
{
    public class Event
    {
        // Primary key
        public int EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation Property
        public Category? Category { get; set; } // nullable
    }
}

namespace Eventix.Models
{
    public class Performance
    {
        public string ImagePath { get; set; } = string.Empty;
        public int PerformanceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Host { get; set;} = string.Empty;
        public DateTime PerformanceDate { get; set; }
        public DateTime EndDate { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}

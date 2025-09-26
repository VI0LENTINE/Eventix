namespace Eventix.Models
{
    public class Performance
    {
        public int PerformanceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PerformanceDate { get; set; }
        public DateTime EndDate { get; set; }


        public Category? Category { get; set; }
    }
}

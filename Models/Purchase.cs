using System.ComponentModel.DataAnnotations;

namespace Eventix.Models
{
    public class Purchase
    {   
        public int PurchaseId { get; set; }
        public int Tickets { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CardLast4 {  get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }

        // Foreign key
        [Display(Name = "Id")]
        public int PerformanceId { get; set; }

        // Navigation property
        public Performance? Performance { get; set; }

    }
}

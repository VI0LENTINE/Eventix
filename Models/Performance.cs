using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventix.Models
{
    public class Performance
    {
        public int PerformanceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Host { get; set;} = string.Empty;
        public DateTime PerformanceDate { get; set; }
        public DateTime EndDate { get; set; }

        [NotMapped]
        [Display(Name = "ImageFilepath")]
        public IFormFile? FormFile { get; set; } // nullable
        public string? ImagePath { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}

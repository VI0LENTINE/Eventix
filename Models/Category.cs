namespace Eventix.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public List<Performance>? Performances { get; set; } // nullable
    }
}

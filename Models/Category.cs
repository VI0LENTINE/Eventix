namespace Eventix.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Performance>? Performances { get; set; } // nullable
    }
}

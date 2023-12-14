namespace Shared.Models
{
    public class Category : Detail
    {
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public CategoryGroup? CategoryGroup { get; set; }
        public int CategoryGroupId { get; set; }

    }
}
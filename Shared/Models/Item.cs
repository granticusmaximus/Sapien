namespace Shared.Models
{
    public class Item : Detail
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SellingPrice { get; set; }
        public int Amount { get; set; }
        public int Quantity { get; set; }
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public Unit? Unit { get; set; }
        public int UnitId { get; set; }

    }

}
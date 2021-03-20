namespace EShopOnion.DataAccess.Entities
{
    public class BasketItem : BaseEntity
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
    }
}
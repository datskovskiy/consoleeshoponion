namespace EShopOnion.DataAccess.Entities
{
    public class OrderItem : BaseEntity
    {
        public int ProductItemId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public OrderItem(int productItemId, decimal price, int quantity)
        {
            ProductItemId = productItemId;
            Price = price;
            Quantity = quantity;
        }
    }
}

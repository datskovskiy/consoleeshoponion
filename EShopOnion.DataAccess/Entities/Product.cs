using EShopOnion.DataAccess.Interfaces;

namespace EShopOnion.DataAccess.Entities
{
    public class Product : BaseEntity, IProduct
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public bool IsDeleted { get; set; }
    }
}
using EShopOnion.DataAccess.Interfaces;

namespace EShopOnion.DataAccess.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
    }
}
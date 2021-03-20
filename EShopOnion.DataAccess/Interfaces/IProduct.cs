using System;
using System.Collections.Generic;
using System.Text;

namespace EShopOnion.DataAccess.Interfaces
{
    public interface IProduct : IBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public bool IsDeleted { get; set; }
    }
}

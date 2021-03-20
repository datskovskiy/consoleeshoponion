using EShopOnion.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EShopOnion.DataAccess.Entities
{
    public class Order: BaseEntity
    {
        public Order()
        {
        }

        public Order(int clientId, IReadOnlyList<OrderItem> orderItems, string address)
        {
            Address = address;
            Total = orderItems.Sum(o => o.Quantity * o.Price);
            ClientId = clientId;
            OrderItems = orderItems;
        }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Address { get; set; }
        public decimal Total { get; set; }
        public int ClientId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.New;

        public IReadOnlyList<OrderItem> OrderItems { get; set; }
    }
}

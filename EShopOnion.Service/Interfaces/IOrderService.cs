using EShopOnion.DataAccess.Entities;
using EShopOnion.DataAccess.Enums;
using EShopOnion.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopOnion.Service.Interfaces
{
    public interface IOrderService
    {
        Order GetOrderById(int id);

        IReadOnlyList<Order> GetUserOrders(IUser user);

        void CreateOrder(Order order);

        void UpdateStatusOrder(int id, OrderStatus status);
    }
}

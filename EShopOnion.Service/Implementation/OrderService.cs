using EShopOnion.DataAccess.Entities;
using EShopOnion.DataAccess.Enums;
using EShopOnion.DataAccess.Interfaces;
using EShopOnion.Repository.Interfaces;
using EShopOnion.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EShopOnion.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _orderRepository = unitOfWork.Repository<Order>();
        }

        public void CreateOrder(Order order)
        {
            if (order is null)
                return;

            _orderRepository.Create(order);
        }

        public Order GetOrderById(int id)
        {
            return _orderRepository.List(p => p.Id == id).FirstOrDefault();
        }

        public IReadOnlyList<Order> GetUserOrders(IUser user)
        {
            return _orderRepository.List(o => o.ClientId == user.Id).ToList();
        }

        public void UpdateStatusOrder(int id, OrderStatus status)
        {
            var order = GetOrderById(id);
            if (order is null)
                return;

            order.Status = status;

            _orderRepository.Update(order);
        }
    }
}

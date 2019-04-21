using WebMvc.Models.OrderModels;
using WebMvc.ViewModels;
using WebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrders();
        Task<Order> GetOrder(string orderId);
        Task<int> CreateOrder(Order order);
    }
}

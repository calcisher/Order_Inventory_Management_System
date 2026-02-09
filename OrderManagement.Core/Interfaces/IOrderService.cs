using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagement.Core.Entities;
namespace OrderManagement.Core.Interfaces
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync(Order order);
    }
}

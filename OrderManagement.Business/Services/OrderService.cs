using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagement.Core.Interfaces;
using OrderManagement.Core.Entities;
using Microsoft.Extensions.Logging;
namespace OrderManagement.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IUnitOfWork unitOfWork, ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<bool> CreateOrderAsync(Order order)
        {
            _logger.LogInformation("Creating order for user {UserId} ", order.UserId);
            foreach (var item in order.OrderItems)
            {
                var product=await _unitOfWork.GetRepository<Product>().GetByIdAsync(item.ProductId);
                if (product == null || product.StockQuantity < 0){
                    _logger.LogWarning("Product {ProductId} is out of stock or does not exist", item.ProductId);
                    return false;
                }
                product.StockQuantity -= item.Quantity;
                _unitOfWork.GetRepository<Product>().Update(product);
            
            }
            await _unitOfWork.GetRepository<Order>().AddAsync(order);
            var result=await _unitOfWork.CommitAsync();
            if(result > 0){
                _logger.LogInformation("Order created successfully for user {UserId} ", order.UserId);
                return true;
            }
            _logger.LogError("Failed to create order for user {UserId} ", order.UserId);
            return false;
        }
    }
}

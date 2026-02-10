using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagement.Core.Interfaces;
using OrderManagement.Core.Entities;
namespace OrderManagement.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IGenericRepository<Product> productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateOrderAsync(Order order)
        {
            foreach(var item in order.OrderItems)
            {
                var product=await _unitOfWork.GetRepository<Product>().GetByIdAsync(item.ProductId);
                if (product == null || product.StockQuantity < 0){
                    return false;
                }
                product.StockQuantity -= item.Quantity;
                _unitOfWork.GetRepository<Product>().Update(product);
            
            }
            await _unitOfWork.GetRepository<Order>().AddAsync(order);
            var result=await _unitOfWork.CommitAsync();
            return result > 0;
        }
    }
}

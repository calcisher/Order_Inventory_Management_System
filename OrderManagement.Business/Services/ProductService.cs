using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace OrderManagement.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IGenericRepository<Product> productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync() => await _productRepository.GetAllAsync();

        public async Task<Product> GetProductByIdAsync(int id) => await _productRepository.GetByIdAsync(id);

        public async Task CreateProductAsync(Product product)
        {
            await _productRepository.AddAsync(product);
            await _unitOfWork.CommitAsync(); //changes are sent to the sql server
        }

        public async Task UpdateProductAsync(Product product)
        {
            _productRepository.Update(product);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                _productRepository.Delete(product);
                await _unitOfWork.CommitAsync();
            }
        }
        public async Task<bool> AddStockAsync(int productId, int quantity)
        {
            var product= await _unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
            if (product == null)
            {
                _logger.LogWarning($"Failed to add stock. Product with ID {productId} not found");
                return false;
            }
            product.StockQuantity += quantity;
            _unitOfWork.GetRepository<Product>().Update(product);
            var result = await _unitOfWork.CommitAsync();
            if(result>0){
                _logger.LogInformation($"Stock added to {product.Name}. New stock quantity: {product.StockQuantity}");
                return true;
            }
            return false;
        }
    }
}

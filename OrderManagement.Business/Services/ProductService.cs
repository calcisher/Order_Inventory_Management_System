using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IUnitOfWork _unitOfWork;

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
            await _unitOfWork.CommitAsync(); // Değişiklikleri SQL Server'a gönderir
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
    }
}

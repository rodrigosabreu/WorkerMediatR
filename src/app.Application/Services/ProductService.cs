using app.Application.Interfaces;
using app.Domain.Entities;

namespace app.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetLowQuantityProducts(int quantityThreshold)
        {
            return _productRepository.GetLowQuantityProducts(quantityThreshold);
        }

        public List<Product> GetProducts()
        {
            return _productRepository.GetProducts();
        }
    }
}

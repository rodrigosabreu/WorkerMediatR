using app.Domain.Entities;

namespace app.Application.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetProducts();
        List<Product> GetLowQuantityProducts(int quantityThreshold);
    }
}

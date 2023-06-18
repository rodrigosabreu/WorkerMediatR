using app.Domain.Entities;

namespace app.Application.Interfaces
{
    public interface IProductService
    {
        List<Product> GetProducts();
        List<Product> GetLowQuantityProducts(int quantityThreshold);
    }
}

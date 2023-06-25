using app.Application.Interfaces;
using app.Domain.Entities;
using app.Infrastructure.Context;

namespace app.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;
        private List<Product> _lista;        

        public ProductRepository(AppDbContext dbContext)
        {

            Console.WriteLine("Consultando base de dados...");

            _dbContext = dbContext;
            _lista = new List<Product>()
            {
                new Product { Id = 1, Name = "Teste 1", Quantity = 5 },
                new Product { Id = 20, Name = "Teste 2", Quantity = 5 },
                new Product { Id = 30, Name = "Teste 3", Quantity = 5 }
            };            
        }

        public List<Product> GetLowQuantityProducts(int quantityThreshold)
        {                        
            return _lista.Where(x => x.Quantity <= quantityThreshold).ToList();
            //return _dbContext.Products.Where(p => p.Quantity < quantityThreshold).ToList();
        }

        public List<Product> GetProducts()
        {
            Console.WriteLine("Listando produtos...");
            return _lista;
            //return _dbContext.Products.ToList();
        }
    }
}

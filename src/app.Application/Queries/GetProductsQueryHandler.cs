using app.Application.Interfaces;
using app.Domain.Entities;
using MediatR;
using System.Drawing;

namespace app.Application.Queries
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
    {
        private readonly IProductService _productService;
        private readonly IEstruturaComercialService _estruturaComerciaService;

        public GetProductsQueryHandler(IProductService productService, IEstruturaComercialService estruturaComerciaService = null)
        {
            _productService = productService;
            _estruturaComerciaService = estruturaComerciaService;
        }

        public async Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var produtos =  _productService.GetLowQuantityProducts(5).Where(p => RemoverProdutos(p)).ToList();

            return produtos;
        }

        private bool RemoverProdutos(Product p)
        {
            var dadosEstruturaComercial = _estruturaComerciaService.GetCacheEstruturaComercial();

            int chave = p.Id;

            if (dadosEstruturaComercial.TryGetValue(chave, out EstruturaComercial estrutura))
            {
                Console.WriteLine($"Cliente encontrado na estrutura '{chave}': {estrutura}");
            }
            else
            {
                Console.WriteLine($"Cliente não encontrado '{chave}' não encontrada no dicionário");
            }


            return true;
        }
    }
}

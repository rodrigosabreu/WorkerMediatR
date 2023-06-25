using app.Application.Interfaces;
using app.Domain.Entities;
using app.Infrastructure.Context;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace app.Infrastructure.Repository
{
    public class EstruturaComercialRepository : IEstruturaComercialRepository
    {
        private readonly IMemoryCache _cache;
        private readonly EstruturaComercialContext _context;
        private readonly ILogger<EstruturaComercialRepository> _logger;        

        public EstruturaComercialRepository(EstruturaComercialContext context, IMemoryCache cache, ILogger<EstruturaComercialRepository> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;            
        }

        public void SetCacheEstruturaComercial()
        {
            try
            {
                _logger.LogInformation("Método SetCacheEstruturaComercial : {time}", DateTimeOffset.Now);

                var dados = _context.estruturas.ToList();

                Dictionary<int, EstruturaComercial> dicionarioProdutos = dados.ToDictionary(p => p.IdCliente);

                _cache.Set("EstruturaComercialCache", dicionarioProdutos);                
                
            }            
            catch(Exception e)
            {
                _logger.LogError("Método SetCacheEstruturaComercial : {time} - Mensagem: ", e.Message);
                throw;
            }
        }

        public Dictionary<int, EstruturaComercial> GetCacheEstruturaComercial()
        {
            if (_cache.TryGetValue("EstruturaComercialCache", out Dictionary<int, EstruturaComercial> dados))
            {
                _logger.LogInformation("Método GetCacheEstruturaComercial : {time}", DateTimeOffset.Now);
                // Utilize os dados conforme necessário
                return dados;
            }
            return new Dictionary<int, EstruturaComercial> { };
        }
    }
}

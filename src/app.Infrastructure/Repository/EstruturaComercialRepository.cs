using app.Application.Interfaces;
using app.Application.Log;
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
        private readonly ILoggerWorker<EstruturaComercialRepository> _logger;        

        public EstruturaComercialRepository(EstruturaComercialContext context, IMemoryCache cache, ILoggerWorker<EstruturaComercialRepository> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;            
        }

        public void SetCacheEstruturaComercial()
        {
            try
            {
                _logger.LogInfo($"Método SetCacheEstruturaComercial : {DateTimeOffset.Now}");

                var dados = _context.estruturas.ToList();

                Dictionary<int, EstruturaComercial> dicionarioProdutos = dados.ToDictionary(p => p.IdCliente);

                _cache.Set("EstruturaComercialCache", dicionarioProdutos);                
                
            }            
            catch(Exception e)
            {
                _logger.LogException("Método SetCacheEstruturaComercial : {time} - Mensagem: ", e);
                throw;
            }
        }

        public Dictionary<int, EstruturaComercial> GetCacheEstruturaComercial()
        {
            if (_cache.TryGetValue("EstruturaComercialCache", out Dictionary<int, EstruturaComercial> dados))
            {
                _logger.LogInfo($"Método GetCacheEstruturaComercial : {DateTimeOffset.Now}");
                // Utilize os dados conforme necessário
                return dados;
            }
            return new Dictionary<int, EstruturaComercial> { };
        }
    }
}

using app.Application.Interfaces;
using app.Application.Log;
using app.Domain.Entities;
using app.Infrastructure.Context;
using Microsoft.Extensions.Caching.Memory;

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

        public async Task<bool> SetCacheEstruturaComercial()
        {
            try
            {
                _logger.LogInfo($"Consultando Banco de Dados...");

                var dados = _context.estruturas.ToList();

                Dictionary<int, EstruturaComercial> dicionarioProdutos = dados.ToDictionary(p => p.IdCliente);

                _cache.Set("EstruturaComercialCache", dicionarioProdutos);

                return true;
            }            
            catch(Exception e)
            {
                _logger.LogException("Erro ao consultar Banco de Dados", e);
                return false;
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

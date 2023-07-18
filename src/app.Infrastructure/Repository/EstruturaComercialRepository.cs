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
                _logger.LogInfo($"Consultando Banco de Dados");

                var dados = _context.estruturas.ToList();

                var log = new Dictionary<string, object>
                {                    
                    ["qtd_registros"] = dados.Count
                };

                _logger.LogInfo(log, "Registros Encontrados no Banco de Dados");

                Dictionary<int, EstruturaComercial> dicionarioProdutos = dados.ToDictionary(p => p.IdCliente);

                _logger.LogInfo($"Subindo Dados Para Memoria");

                _cache.Set("EstruturaComercialCache", dicionarioProdutos);

                return true;
            }            
            catch(Exception e)
            {
                _logger.LogException("Erro ao Consultar Banco de Dados", e);
                return false;
            }
        }

        public Dictionary<int, EstruturaComercial> GetCacheEstruturaComercial()
        {
            if (_cache.TryGetValue("EstruturaComercialCache", out Dictionary<int, EstruturaComercial> dados))
            {
                _logger.LogInfo("Consultando Dados na Memoria");
                
                return dados;
            }
            return new Dictionary<int, EstruturaComercial> { };
        }
    }
}

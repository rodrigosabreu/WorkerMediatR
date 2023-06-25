using app.Application.Interfaces;
using app.Domain.Entities;
using app.Infrastructure.Context;
using app.Infrastructure.Log;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace app.Infrastructure.Repository
{
    public class EstruturaComercialRepository : IEstruturaComercialRepository
    {
        private readonly IMemoryCache _cache;
        private readonly EstruturaComercialContext _context;
        private readonly ILogger<EstruturaComercialRepository> _logger;
        private readonly ILogManager _loggerManager;

        public EstruturaComercialRepository(EstruturaComercialContext context, IMemoryCache cache, ILogger<EstruturaComercialRepository> logger, ILogManager loggerManager)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
            _loggerManager = loggerManager;
        }

        public void SetCacheEstruturaComercial()
        {
            try
            {
                var dados = _context.estruturas.ToList();

                Dictionary<int, EstruturaComercial> dicionarioProdutos = dados.ToDictionary(p => p.IdCliente);

                _cache.Set("EstruturaComercialCache", dicionarioProdutos);

                _logger.LogInformation("Subindo Estrutura Comerical para memoria em : {time}", DateTimeOffset.Now);
                _loggerManager.LogInformation($"{typeof(EstruturaComercialRepository)} - Subindo Estrutura Comerical para memoria em : {DateTimeOffset.Now}");
            }
            catch(Exception e)
            {
                _loggerManager.LogError($"{typeof(EstruturaComercialRepository)} - ERRO: {e.Message}");
                throw;
            }
        }

        public Dictionary<int, EstruturaComercial> GetCacheEstruturaComercial()
        {
            if (_cache.TryGetValue("EstruturaComercialCache", out Dictionary<int, EstruturaComercial> dados))
            {
                _loggerManager.LogInformation($"{typeof(EstruturaComercialRepository)} - Consultado os dados da memoria : {dados}");
                // Utilize os dados conforme necessário
                return dados;
            }
            return new Dictionary<int, EstruturaComercial> { };
        }
    }
}

﻿using app.Application.Interfaces;
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
            var dados = _context.estruturas.ToList();

            Dictionary<int, EstruturaComercial> dicionarioProdutos = dados.ToDictionary(p => p.IdCliente);            

            _cache.Set("EstruturaComercialCache", dicionarioProdutos);

            _logger.LogInformation("Subindo Estrutura Comerical para memoria em : {time}", DateTimeOffset.Now);
        }

        public Dictionary<int, EstruturaComercial> GetCacheEstruturaComercial()
        {
            if (_cache.TryGetValue("EstruturaComercialCache", out Dictionary<int, EstruturaComercial> dados))
            {
                // Utilize os dados conforme necessário
                return dados;
            }
            return new Dictionary<int, EstruturaComercial> { };
        }
    }
}
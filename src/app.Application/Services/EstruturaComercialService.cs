using app.Application.Interfaces;
using app.Domain.Entities;

namespace app.Domain.Services
{
    public class EstruturaComercialService : IEstruturaComercialService
    {
        private readonly IEstruturaComercialRepository _estruturaComercialRepository;

        public EstruturaComercialService(IEstruturaComercialRepository estruturaComercialRepository)
        {
            _estruturaComercialRepository = estruturaComercialRepository;
        }        

        public void SetCacheEstruturaComercial()
        {
            _estruturaComercialRepository.SetCacheEstruturaComercial();
        }

        public Dictionary<int, EstruturaComercial> GetCacheEstruturaComercial()
        {
            return _estruturaComercialRepository.GetCacheEstruturaComercial();
        }
    }
}

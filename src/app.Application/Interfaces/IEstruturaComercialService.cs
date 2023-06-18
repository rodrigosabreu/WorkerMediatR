using app.Domain.Entities;

namespace app.Application.Interfaces
{
    public interface IEstruturaComercialService
    {
        void SetCacheEstruturaComercial();

        Dictionary<int, EstruturaComercial> GetCacheEstruturaComercial();
    }
}

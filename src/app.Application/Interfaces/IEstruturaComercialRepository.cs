using app.Domain.Entities;

namespace app.Application.Interfaces
{
    public interface IEstruturaComercialRepository
    {
        void SetCacheEstruturaComercial();

        Dictionary<int, EstruturaComercial> GetCacheEstruturaComercial();
    }
}

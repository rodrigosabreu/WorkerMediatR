using app.Domain.Entities;

namespace app.Application.Interfaces
{
    public interface IEstruturaComercialRepository
    {
        Task<bool> SetCacheEstruturaComercial();

        Dictionary<int, EstruturaComercial> GetCacheEstruturaComercial();
    }
}

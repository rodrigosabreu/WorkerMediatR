using app.Domain.Entities;

namespace app.Application.Interfaces
{
    public interface IEstruturaComercialService
    {
        Task<bool> SetCacheEstruturaComercial();

        Dictionary<int, EstruturaComercial> GetCacheEstruturaComercial();
    }
}

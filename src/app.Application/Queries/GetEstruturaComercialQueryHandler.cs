using app.Domain.Entities;
using app.Application.Interfaces;
using MediatR;

namespace app.Application.Queries
{
    public class GetEstruturaComercialQueryHandler : IRequestHandler<GetEstruturaComercialQuery>
    {
        private readonly IEstruturaComercialService _estruturaComercialService;

        public GetEstruturaComercialQueryHandler(IEstruturaComercialService estruturaComercialService)
        {
            _estruturaComercialService = estruturaComercialService;
        }

        public async Task Handle(GetEstruturaComercialQuery request, CancellationToken cancellationToken)
        {
            _estruturaComercialService.SetCacheEstruturaComercial();
        }
    }
}

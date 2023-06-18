using app.Domain.Entities;
using MediatR;

namespace app.Application.Queries
{
    public class GetProductsQuery : IRequest<IEnumerable<Product>>{}
}

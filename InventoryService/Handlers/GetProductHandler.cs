using InventoryService.Models;
using InventoryService.Queries;
using InventoryService.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Handlers
{
    public class GetProductHandler : IRequestHandler<GetProductQuery, Product>
    {
        private readonly IRepository repository;

        public GetProductHandler(IRepository repository) =>
            this.repository = repository;

        public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken) =>
            await Task.FromResult(repository.Get(request.Id));
    }
}

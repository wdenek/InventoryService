using InventoryService.Models;
using InventoryService.Queries;
using InventoryService.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Handlers
{
    public class GetProductsHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
    {
        private readonly IRepository repository;

        public GetProductsHandler(IRepository repository) => 
            this.repository = repository;

        public async Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken) =>
            await Task.FromResult(repository.Get());
    }
}

using InventoryService.Models;
using MediatR;

namespace InventoryService.Queries
{
    public class GetProductQuery : IRequest<Product>
    {
        public GetProductQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}

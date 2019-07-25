using System.Collections.Generic;
using InventoryService.Models;
using MediatR;

namespace InventoryService.Queries
{
    public class GetProductsQuery : IRequest<IEnumerable<Product>>
    {

    }
}

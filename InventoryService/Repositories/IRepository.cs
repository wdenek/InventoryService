using InventoryService.Models;
using InventoryService.Queries;
using System.Collections.Generic;

namespace InventoryService.Repositories
{
    public interface IRepository
    {
        IEnumerable<Product> Get();

        Product Get(int id);

        IEnumerable<Product> Search(SearchProductsQuery searchQuery);
    }
}

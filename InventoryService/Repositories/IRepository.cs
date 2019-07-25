using InventoryService.Models;
using System.Collections.Generic;

namespace InventoryService.Repositories
{
    public interface IRepository
    {
        IEnumerable<Product> Get();

        Product Get(int id);
    }
}

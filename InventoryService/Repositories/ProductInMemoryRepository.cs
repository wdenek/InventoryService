using InventoryService.Models;
using InventoryService.Queries;
using InventoryService.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace BestelService.Repositories
{
    public class ProductInMemoryRepository : IRepository
    {
        private static List<Product> products = new List<Product>
        {
                new Product(1, "Bike"),
                new Product(2, "Smartphone"),
                new Product(3, "Umbrella")
        };
        
        public IEnumerable<Product> Get()
        {
            return products;
        }

        public Product Get(int id)
        {
            return products.FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Product> Search(SearchProductsQuery searchQuery)
        {
            return products.Where(p => p.Name.Contains(searchQuery.Name));
        }
    }
}

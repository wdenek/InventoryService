using InventoryService.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Queries
{
    public class SearchProductsQuery : IRequest<IEnumerable<Product>>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public bool IsInStock { get; set; }
    }
}

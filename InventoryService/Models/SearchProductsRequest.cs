using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService.Models
{
    public class SearchProductsRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public bool IsInStock { get; set; }
    }
}

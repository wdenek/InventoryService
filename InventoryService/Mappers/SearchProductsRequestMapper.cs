using InventoryService.Models;
using InventoryService.Queries;
using System;

namespace InventoryService.Mappers
{
    public static class SearchProductsRequestMapper
    {
        public static SearchProductsQuery ToQuery(this SearchProductsRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return new SearchProductsQuery
            {
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                IsInStock = request.IsInStock
            };
        }
    }
}

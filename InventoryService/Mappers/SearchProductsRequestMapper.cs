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

            // In this case it could also be mapped using AutoMapper
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

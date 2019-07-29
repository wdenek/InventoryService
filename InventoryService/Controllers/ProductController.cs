using InventoryService.Mappers;
using InventoryService.Models;
using InventoryService.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;
        public ProductController(IMediator mediator) => 
            this.mediator = mediator;
        
        [HttpGet]
        public async Task<IEnumerable<Product>> Get() =>
            await mediator.Send(new GetProductsQuery());

        [HttpGet("{id}")]
        public async Task<Product> Get(int id) =>
            await mediator.Send(new GetProductQuery(id));

        public async Task<IEnumerable<Product>> SearchBad(SearchProductsRequest request)
        {
            // I wouldn't do this because the only way to test the mapping
            // is through the mediator mock.
            var query = new SearchProductsQuery
            {
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                IsInStock = request.IsInStock
            };
            return await mediator.Send(query);
        }

        public async Task<IEnumerable<Product>> Search(SearchProductsRequest request)
        {
            // I'd create an extension method containing the mapping
            // (wich can be implemented manually, with AutoMapper, etc.)
            var query = request.ToQuery();
            return await mediator.Send(query);
        }
    }   
}

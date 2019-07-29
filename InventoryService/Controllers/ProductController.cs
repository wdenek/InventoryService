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
            throw new System.NotImplementedException();
        }
    }   
}

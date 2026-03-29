using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Features.Products.Commands;
using ProductService.Application.Features.Products.Queries;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace ProductService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
		[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
			 Log.Information("Product created: {ProductId}", id);
            return Ok(id);
        }
        
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] 
       public async Task<IActionResult> Update(Guid id, UpdateProductCommand command)
{
    if (id != command.Id)
        return BadRequest("Id uyuşmuyor");

    var result = await _mediator.Send(command);

    if (!result)
    {
        Log.Warning("Product not found: {ProductId}", id);
        return NotFound();
    }

    Log.Information("Product updated: {ProductId}", id);

    return NoContent();
}

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetProductsQuery();
            var products = await _mediator.Send(query);
            return Ok(products);
        }
    }
}
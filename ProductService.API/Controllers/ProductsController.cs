using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Features.Products.Commands;
using ProductService.Application.Features.Products.Queries;
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
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
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
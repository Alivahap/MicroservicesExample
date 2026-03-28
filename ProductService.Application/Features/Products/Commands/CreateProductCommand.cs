using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ProductService.Application.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<Guid>
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
}

using MediatR;
using ProductService.Domain.Entities;
using System.Collections.Generic;

namespace ProductService.Application.Features.Products.Queries
{
    public class GetProductsQuery : IRequest<List<Product>>
    {
    }
}
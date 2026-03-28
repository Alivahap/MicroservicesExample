using ProductService.Infrastructure.Persistence;
using ProductService.Infrastructure.Repositories;
using ProductService.Domain.Interfaces;

using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Application.Features.Products.Commands;
using ProductService.Infrastructure.Events;
using ProductService.Application.Events;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMediatR(typeof(CreateProductCommandHandler).Assembly);


builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IEventPublisher, EventPublisher>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/ping", () => "pong");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
using Contracts.Mediatr.Wrappers;
using Products.Domain.Entities;

namespace Products.Application.Features.AddProduct;

public record CreateProductCommand(string Name,List<int> Categories,double Price,string ImageBase64) : IValidatedCommand<Product>;
using Helpers.SourceGenerator.Attributes;
using Products.Domain;

namespace Products.Application.Models;

[Mapped(typeof(Product))]
public class ProductDetailedDto
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? Expiration { get; set; }
    public DateTimeOffset? BoughtAt { get; set; }
}

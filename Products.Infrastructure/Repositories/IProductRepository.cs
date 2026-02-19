using Products.Domain;

namespace Products.Infrastructure.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(ulong id);
    Task<(List<Product> items, uint totalCount)> GetListAsync(ulong? id, uint page, uint pageSize);
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(ulong id);
}

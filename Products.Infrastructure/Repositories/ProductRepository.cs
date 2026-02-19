using Microsoft.EntityFrameworkCore;
using Products.Domain;
using Products.Infrastructure.Data;

namespace Products.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductsDbContext _context;

    public ProductRepository(ProductsDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(ulong id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<(List<Product> items, uint totalCount)> GetListAsync(ulong? id, uint page, uint pageSize)
    {
        var query = _context.Products.AsQueryable();

        if (id.HasValue && id.Value > 0)
        {
            query = query.Where(p => p.Id == id.Value);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((int)((page - 1) * pageSize))
            .Take((int)pageSize)
            .ToListAsync();

        return (items, (uint)totalCount);
    }

    public async Task<Product> AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ulong id)
    {
        var product = await GetByIdAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}

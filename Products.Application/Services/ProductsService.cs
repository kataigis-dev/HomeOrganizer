using Helpers.Common.Models;
using Helpers.Web.Controllers;
using Products.Application.Filters;
using Products.Application.Models;
using Products.Domain;
using Products.Infrastructure.Repositories;

namespace Products.Application.Services;

public class ProductsService : IApplicationService<ProductFilters, ProductDetailedDto, ProductDetailedDto, ProductDto>
{
    private readonly IProductRepository _repository;
    private readonly ITranslationHandler _translationHandler;

    public ProductsService(IProductRepository repository, ITranslationHandler translationHandler)
    {
        _repository = repository;
        _translationHandler = translationHandler;
    }

    public async Task<Pagination<ProductDetailedDto>> GetListAsync(ProductFilters filters)
    {
        var (items, totalCount) = await _repository.GetListAsync(filters.Id, filters.Page, filters.PageSize);
        
        var pageCount = (uint)Math.Ceiling((double)totalCount / filters.PageSize);
        
        var dtos = items.Select(p => new ProductDetailedDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Expiration = p.Expiration,
            BoughtAt = p.BoughtAt
        }).ToList();

        return new Pagination<ProductDetailedDto>(dtos, filters.Page, pageCount, filters.PageSize);
    }

    public async Task<ProductDetailedDto> GetOneAsync(ulong id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with id {id} not found");
        }

        return new ProductDetailedDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Expiration = product.Expiration,
            BoughtAt = product.BoughtAt
        };
    }

    public async Task CreateAsync(ProductDto request)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Expiration = request.Expiration,
            BoughtAt = request.BoughtAt
        };

        product.ValidateAndThrow(_translationHandler);
        await _repository.AddAsync(product);
    }

    public async Task UpdateAsync(ulong id, ProductDto request)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with id {id} not found");
        }

        product.Name = request.Name;
        product.Description = request.Description;
        product.Expiration = request.Expiration;
        product.BoughtAt = request.BoughtAt;

        product.ValidateAndThrow(_translationHandler);
        await _repository.UpdateAsync(product);
    }

    public async Task DeleteAsync(ulong id)
    {
        await _repository.DeleteAsync(id);
    }
}

using Helpers.Common.Models;
using Helpers.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Products.Application.Filters;
using Products.Application.Models;

namespace Products.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : BaseCrudsController<ProductFilters, ProductDetailedDto, ProductDetailedDto, ProductDto>
{
    public ProductsController(
        ILogger<ProductsController> logger,
        IApplicationService<ProductFilters, ProductDetailedDto, ProductDetailedDto, ProductDto> svc,
        ITranslationHandler translationHandler)
        : base(logger, svc, translationHandler)
    {
    }
}

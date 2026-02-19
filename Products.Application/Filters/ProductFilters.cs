using Helpers.Common.Models;
using Helpers.Common.Validations;
using Helpers.Web.Controllers;

namespace Products.Application.Filters;

public class ProductFilters : IHttpRequestFilter, IValidate
{
    public ulong Id { get; set; }
    public uint Page { get; set; } = 1;
    public uint PageSize { get; set; } = 10;

    public ValidationResult Validate(ITranslationHandler translationHandler)
        => ValidationResult.BuildWithRules(
            new()
            {
                {
                    "Page must be greater than 0",
                    () => Page > 0
                },
                {
                    "PageSize must be between 1 and 100",
                    () => PageSize > 0 && PageSize <= 100
                }
            });

    public void ValidateAndThrow(ITranslationHandler translationHandler)
    {
        var validationResult = Validate(translationHandler);
        if (!validationResult.Succeeded)
        {
            throw new ArgumentException(validationResult.ToString());
        }
    }
}

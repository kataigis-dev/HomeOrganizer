using Helpers.Common.Models;
using Helpers.Common.Validations;
using Helpers.SourceGenerator.Attributes;
using Products.Domain;

namespace Products.Application.Models;

[Mapped(typeof(Product))]
public class ProductDto : IValidate
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? Expiration { get; set; }
    public DateTimeOffset? BoughtAt { get; set; }

    public ValidationResult Validate(ITranslationHandler translationHandler)
        => ValidationResult.BuildWithRules(
            new()
            {
                {
                    translationHandler.Generic.NameIsRequired,
                    () => !string.IsNullOrWhiteSpace(Name)
                },
                {
                    translationHandler.Others[nameof(Product)][nameof(Expiration)],
                    () => !Expiration.HasValue || Expiration.Value.Date >= DateTimeOffset.UtcNow.Date
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

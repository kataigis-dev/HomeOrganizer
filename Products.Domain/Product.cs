using Helpers.Common.Models;
using Helpers.Common.Exceptions;
using Helpers.Common.Validations;

namespace Products.Domain;

public class Product : IDomainModel
{
    public ulong Id { get; init; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? Expiration { get; set; }
    public DateTimeOffset? BoughtAt { get; set; }

    public ValidationResult Validate(ITranslationHandler translationHandler)
        => ValidationResult.BuildWithRules(
            new()
            {
                {
                    translationHandler.Others[nameof(Product)][nameof(Expiration)],
                    () => !Expiration.HasValue || Expiration.Value.Date <= DateTimeOffset.UtcNow.Date
                },
                {
                    translationHandler.Generic.NameIsRequired,
                    () => !string.IsNullOrWhiteSpace(Name)
                }
            });

    public void ValidateAndThrow(ITranslationHandler translationHandler)
    {
        var validationResult = Validate(translationHandler);
        DomainRulesException.AssertAndThrow(() => validationResult.Succeeded, validationResult.ToString());
    }
}

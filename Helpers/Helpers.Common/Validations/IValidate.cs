using Helpers.Common.Models;

namespace Helpers.Common.Validations;

public interface IValidate
{
    ValidationResult Validate(ITranslationHandler translationHandler);
    void ValidateAndThrow(ITranslationHandler translationHandler);
}

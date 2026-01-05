using Helpers.Common.Validations;

namespace Helpers.Common.Models;

/// <summary>
/// All domain models should implement this interface
/// </summary>
public interface IDomainModel : IValidate
{
    public ulong Id { get; init; }
}
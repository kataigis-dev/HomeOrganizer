namespace Helpers.Common.Exceptions;

/// <summary>
/// This class represents a failed validation of some business logic
/// </summary>
public class DomainRulesException : Exception
{
    public DomainRulesException(string message) : base(message) { }

    /// <summary>
    /// If the assertion fails then it throws
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="DomainRulesException"></exception>
    public static void AssertAndThrow(Func<bool> assertion, string? message)
    {
        ArgumentNullException.ThrowIfNull(assertion, nameof(assertion));
        if (!assertion()) throw new DomainRulesException(message ?? string.Empty);
    }
}

namespace Helpers.Common.Validations;

public class ValidationResult(IEnumerable<string>? errors = null)
{
    public bool Succeeded => !Errors.Any();
    public IEnumerable<string> Errors { get; private set; } = errors ?? [];

    /// <summary>
    /// A quality of life improvement for easy aggregation of multiple results
    /// </summary>
    public static ValidationResult operator +(ValidationResult a, ValidationResult b)
    {
        a.Errors = a.Errors.Union(b.Errors);
        return a;
    }

    /// <summary>
    /// This method returns the errors formatted in a simple string
    /// </summary>
    public override string ToString() => string.Join(", ", Errors);

    /// <summary>
    /// Executes all the rules as assertions then returns a new <see cref="ValidationResult"/>
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static ValidationResult BuildWithRules(Dictionary<string, Func<bool>> rules)
    {
        ArgumentNullException.ThrowIfNull(rules, nameof(rules));
        return new(rules?.Where(r => !r.Value()).Select(r => r.Key));
    }
}
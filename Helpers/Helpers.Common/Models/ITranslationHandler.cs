
namespace Helpers.Common.Models
{
    public interface ITranslationHandler
    {
        GenericMessages Generic { get; init; }
        Dictionary<string, Dictionary<string, string>> Others { get; init; }

        void Deconstruct(out GenericMessages Generic, out Dictionary<string, Dictionary<string, string>> Others);
        bool Equals(object? obj);
        bool Equals(TranslationHandler? other);
        int GetHashCode();
        string ToString();
    }
}
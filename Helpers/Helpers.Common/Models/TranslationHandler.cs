namespace Helpers.Common.Models;

// TODO: source generators for translation handlers
public record TranslationHandler(GenericMessages Generic,
    Dictionary<string, Dictionary<string, string>> Others) : ITranslationHandler;

public record GenericMessages(string NameIsRequired);
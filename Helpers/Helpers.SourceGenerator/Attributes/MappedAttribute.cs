using System;

namespace Helpers.SourceGenerator.Attributes;

public class MappedAttribute : Attribute
{
    public Type MapTo { get; set; }
    public MappedAttribute(Type mapTo)
    {
        if (!mapTo.IsClass || mapTo.IsAbstract)
            throw new ArgumentException("Mapped type must be a non-abstract class type.", nameof(mapTo));

        MapTo = mapTo;
    }
}

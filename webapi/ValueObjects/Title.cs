using System.ComponentModel.DataAnnotations;
using webapi.Primitives;

namespace webapi.ValueObjects;

public sealed class Title : ValueObject
{
    public const int MaxLength = 100;

    private Title(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Title Create(string title)
    {
        if (title.Length > MaxLength)
            throw new ValidationException("Title is too long");
        
        return new Title(title);
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
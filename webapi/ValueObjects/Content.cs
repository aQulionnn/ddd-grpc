using System.ComponentModel.DataAnnotations;
using webapi.Primitives;

namespace webapi.ValueObjects;

public sealed class Content : ValueObject
{
    public const int MinLength = 100;

    private Content(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Content Create(string content)
    {
        if (content.Length < MinLength)
            throw new ValidationException("Content is too short");
        
        return new Content(content);
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
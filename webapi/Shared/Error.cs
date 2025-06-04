namespace webapi.Shared;

public class Error : IEquatable<Error>
{
    public static readonly Error None = new (string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.Null", "The value is null.");

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
    
    public string Code { get; set; }    
    public string Message { get; set; }
    
    public static implicit operator string(Error error) => error.Code;

    public static bool operator ==(Error? first, Error? second)
    {
        if (first is null && second is null) return true;
        if (first is null || second is null) return false;
        
        return first.Code == second.Code && first.Message == second.Message;
    }

    public static bool operator !=(Error? first, Error? second)
    {
        return !(first == second);
    }
    
    public bool Equals(Error? other)
    {
        if (other is null) return false;
        if (other.GetType() != GetType()) return false;
        
        return other.Code == Code && other.Message == Message;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        if (obj is not Error error) return false;

        return error.Code == Code && error.Message == Message;;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Message);
    }
}
namespace webapi.Shared;

public interface IValidationResult
{
    public static readonly Error ValidationError = new("Validation.Error", "One or more validation errors occurred.");
    
    Error[] Errors { get; }
}
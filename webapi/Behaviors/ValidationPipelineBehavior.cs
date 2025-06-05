using FluentValidation;
using MediatR;
using webapi.Shared;

namespace webapi.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next(cancellationToken);
        
        var errors = _validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage)) 
            .Distinct()
            .ToArray();

        if (errors.Any()) return CreateValidationResult<TResponse>(errors);
            
       return await next(cancellationToken);      
    }
    
    private static TResult CreateValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Unit))
            return (ValidationResult.WithErrors(errors) as TResult)!;

        object validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult)).GenericTypeArguments[0]
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, [errors])!;
        
        return (TResult)validationResult;
    }
}
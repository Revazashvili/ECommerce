namespace Contracts.Mediatr.Validation;

public class ValidationResult
{
    public ValidationResult(string error)
    {
        Messages = new []{ error };
        IsValid = false;
    }
    
    public ValidationResult(IEnumerable<string> errors)
    {
        Messages = errors;
        IsValid = false;
    }
    
    public IEnumerable<string> Messages { get; }
    public bool IsValid { get; }
    
    public static implicit operator ValidationResult(string message) => new(message);
    public static implicit operator ValidationResult(Exception exception) => new(exception.Message);
}
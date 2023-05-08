namespace DocumentsStore.UseCases;


public class UseCaseResult<T>
{
    private UseCaseResult(ErrorType errorType, string message)
    {
        Error = errorType;
        ErrorMessage = message;
    }

    private UseCaseResult(T result)
    {
        Result = result;
        Error = null;
        ErrorMessage = null;
    }
    
    public T? Result { get; set; }
    public ErrorType? Error { get; set; }
    public string? ErrorMessage { get; set; }

    public static UseCaseResult<T> Success(T result) => new UseCaseResult<T>(result);

    public static UseCaseResult<T> NotFound(string errorMessage = "Entity not found") =>
        new UseCaseResult<T>(ErrorType.NotFound, errorMessage);
    
    public static UseCaseResult<T> BadRequest(string errorMessage = "Please provide the correct parameters") =>
        new UseCaseResult<T>(ErrorType.BadRequest, errorMessage);

    public static UseCaseResult<T> Unauthorized(string errorMessage = "Unauthorized") =>
        new UseCaseResult<T>(ErrorType.Unauthorized, errorMessage);
}
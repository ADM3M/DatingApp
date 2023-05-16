namespace API.Services.Models;

public class OperationResult<T> where T : class
{
    public bool Success { get; set; }

    public T Result { get; set; }

    public OperationResult(bool operationResult, T result)
    {
        Success = operationResult;
        Result = result;
    }
}
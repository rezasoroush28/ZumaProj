namespace Application.Common.Responses
{
    public class CommandResponse<T>
    {
        public bool Success { get; init; }
        public T? Result { get; init; }
        public string? Message { get; init; }

        public static CommandResponse<T> Ok(T result) => new()
        {
            Success = true,
            Result = result
        };

        public static CommandResponse<T> Fail(string message) => new()
        {
            Success = false,
            Message = message
        };
    }
}

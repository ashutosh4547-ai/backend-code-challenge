namespace CodeChallenge.Api.Logic.Results
{
    public class LogicResult<T>
    {
        public bool IsSuccess { get; private set; }
        public bool IsNotFound { get; private set; }
        public bool IsInvalid { get; private set; }
        public string? Error { get; private set; }
        public T? Value { get; private set; }

        public static LogicResult<T> Success(T value) =>
            new() { IsSuccess = true, Value = value };

        public static LogicResult<T> NotFound(string error) =>
            new() { IsNotFound = true, Error = error };

        public static LogicResult<T> Invalid(string error) =>
            new() { IsInvalid = true, Error = error };
    }
}

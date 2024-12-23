namespace EventManagementAPI.Domain.Entities;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<T> Success<T>(T payload) => Result<T>.Success(payload);

    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);
}

public class Result<T> : Result
{
    public Result(bool isSuccess, Error error, T payload)
        : base(isSuccess, error)
    {
        Payload = payload;
    }

    public T Payload { get; }

    public static Result<T> Success(T payload) => new(true, Error.None, payload);
    public static new Result<T> Failure(Error error) => new(false, error, default!);
}



using ULearn.Domain.Enums;

namespace ULearn.Domain.Shared;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    public static Result<TValue> Failure<TValue>(ErrorCodeEnum code,string message) => new(default, false, new Error(code,message));
    public static Result<TValue> FailureBadRequest<TValue>(string message) => new(default, false, new Error(ErrorCodeEnum.BadRequest,message));
    public static Result<TValue> FailureUnauthorized<TValue>() => new(default, false, new Error(ErrorCodeEnum.Unauthorized,"Unauthorized."));
    public static Result<TValue> FailureUnauthorized<TValue>(string message) => new(default, false, new Error(ErrorCodeEnum.Unauthorized,message));
    public static Result<TValue> FailureForbidden<TValue>() => new(default, false, new Error(ErrorCodeEnum.Forbidden,"Forbidden."));
    public static Result<TValue> FailureForbidden<TValue>(string message) => new(default, false, new Error(ErrorCodeEnum.Forbidden,message));
    public static Result<TValue> FailureNotFound<TValue>(string message) => new(default, false, new Error(ErrorCodeEnum.NotFound,message));

    public static Result<TValue> Create<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}


// Generic Result Value
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error) =>
        _value = value;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}



namespace ULearn.Domain.Enums;

public enum ErroCodeEnum
{
    Ok = 200,

    BadRequest = 400,
    Unauthorized = 401,
    PaymentRequired = 402,
    Forbidden = 403,
    NotFound = 404,
    TooManyRequest = 429,

    InternalServerError = 500,

    // Custom
    NullValueError = 1001
}
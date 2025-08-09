namespace ULearn.Domain.Exceptions
{
    public abstract class HttpException(int statusCode, string message) : Exception(message)
    {
        public int StatusCode { get; } = statusCode;
    }

    public class NotFoundException(string message = "Resource not found.") : HttpException(404, message);

    public class UnauthorizedException(string message = "Unauthorized.") : HttpException(401, message);

    public class BadRequestException(string message = "Bad request.") : HttpException(400, message);

    public class ForbiddenException(string message = "Forbidden.") : HttpException(403, message);
    public class InternalServerException(string message = "Something went wrong.") : HttpException(500, message);
}

using ULearn.Domain.Enums;

namespace ULearn.Domain.Shared
{
    public class Error : IEquatable<Error>
    {
        public static readonly Error None = new(ErroCodeEnum.Ok, string.Empty);
        public static readonly Error NullValue = new(ErroCodeEnum.NullValueError, "The specified result value is null.");

        public Error(ErroCodeEnum code, string message)
        {
            Code = code;
            Message = message;
        }

        public ErroCodeEnum Code { get; }

        public string Message { get; }

        public static implicit operator ErroCodeEnum(Error error) => error.Code;

        public static bool operator ==(Error? a, Error? b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Error? a, Error? b) => !(a == b);

        public virtual bool Equals(Error? other)
        {
            if (other is null)
            {
                return false;
            }

            return Code == other.Code && Message == other.Message;
        }

        public override bool Equals(object? obj) => obj is Error error && Equals(error);

        public override int GetHashCode() => HashCode.Combine(Code, Message);

        public override string ToString() => $"{Code} - {Message}";
    }
}

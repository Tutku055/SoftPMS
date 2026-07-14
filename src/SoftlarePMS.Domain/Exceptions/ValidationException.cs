using System;

namespace SoftlarePMS.Domain.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string message)
        : base(message)
    {
    }
}

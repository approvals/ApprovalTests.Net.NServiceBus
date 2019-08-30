using System;
using System.Collections.Generic;
using NServiceBus.Logging;

class LogMessage
{
    public LogMessage(LogLevel level, string message, Exception exception = null)
    {
        Guard.AgainstNull(message, nameof(message));
        Level = level;
        Message = message;
        Exception = exception;
        Args = Array.Empty<object>();
    }

    public LogMessage(LogLevel level, string format, IReadOnlyList<object> args, Exception exception = null)
    {
        Guard.AgainstNull(format, nameof(format));
        Guard.AgainstNull(args, nameof(args));
        Level = level;
        Format = format;
        Exception = exception;
        Message = string.Format(format, args);
        Args = args;
    }

    public readonly LogLevel Level;
    public readonly string Message;
    public readonly Exception Exception;
    public readonly string Format;
    public readonly IReadOnlyList<object> Args;
}
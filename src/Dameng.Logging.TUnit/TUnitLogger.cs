using System.Text;
using Microsoft.Extensions.Logging;

namespace Dameng.Logging.TUnit;

/// <summary>
///  TUnit logger implementation for Microsoft.Extensions.Logging.
/// </summary>
internal class TUnitLogger(Context context, string? category, LogLevel minLevel, bool includeScope,string? dateTimeFormat)
    : ILogger
{
    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel)
    {
        // Check if the log level is enabled
        return logLevel >= minLevel;
    }

    /// <inheritdoc />
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return includeScope ? TUnitLoggerScope.Push(state) : null;
    }

    private static string GetLogLevelString(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => "trce",
            LogLevel.Debug => "dbug",
            LogLevel.Information => "info",
            LogLevel.Warning => "warn",
            LogLevel.Error => "fail",
            LogLevel.Critical => "crit",
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel)),
        };
    }

    /// <inheritdoc />
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        if (IsEnabled(logLevel)==false)
        {
            return;
        }
        var message = GenerateMessage(formatter(state, exception), exception, logLevel, eventId);
        context.OutputWriter.WriteLine(message);
    }

    private string GenerateMessage(
        string message,
        Exception? exception,
        LogLevel logLevel,
        EventId eventId
    )
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append(DateTimeOffset.Now.ToString(dateTimeFormat));
        
        stringBuilder.Append($" {GetLogLevelString(logLevel)}");
        stringBuilder.Append($" {category}");
        if (eventId.Id != 0)
        {
            stringBuilder.Append($" [{eventId.Id}]");
        }

        stringBuilder.Append(": ");
        stringBuilder.Append(message);
        if (includeScope)
        {
            stringBuilder.AppendLine();
            //write scope information
            var scopes = TUnitLoggerScope.GetCurrentScopeChain();
            foreach (var scope in scopes)
            {
                stringBuilder.Append($" => {scope} ");
            }
        }

        if (exception is not null)
        {
            stringBuilder.AppendLine();
            stringBuilder.Append(exception);
        }

        return stringBuilder.ToString();
    }
}

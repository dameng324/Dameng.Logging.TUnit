using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Dameng.Logging.TUnit;

/// <summary>
/// TUnit logging extensions for Microsoft.Extensions.Logging.
/// </summary>
public static class TUnitLoggingExtensions
{
    /// <summary>
    /// Gets a <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance for the specified category using TUnit's logging context.
    /// </summary>
    /// <param name="testContext">TUnit test context, usually come from TUnit.Core.TestContext.Current</param>
    /// <param name="category">Category of the logger</param>
    /// <param name="minLevel">MinLevel of the logger</param>
    /// <param name="includeScope">value that indicates whether scopes are included.</param>
    /// <returns></returns>
    public static ILogger GetLogger(
        this TestContext testContext,
        string? category = null,
        LogLevel minLevel = LogLevel.Information,
        bool includeScope = false
    )
    {
        if (testContext == null)
        {
            throw new ArgumentNullException(nameof(testContext));
        }

        return new TUnitLogger(testContext, category, minLevel, includeScope);
    }

    /// <summary>
    /// Adds TUnit logger provider to the specified ILoggingBuilder.
    /// </summary>
    /// <param name="builder">The logging builder.</param>
    /// <param name="testContext">TUnit test context, default is TUnit.Core.TestContext.Current</param>
    /// <param name="includeScope">value that indicates whether scopes are included.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static ILoggingBuilder AddTUnit(this ILoggingBuilder builder, TestContext? testContext,
        bool includeScope = false)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        testContext ??= TestContext.Current;
        if (testContext == null)
        {
            throw new ArgumentNullException(nameof(testContext));
        }

        var loggerProvider = new TUnitLoggerProvider(testContext, includeScope);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider>(loggerProvider));
        return builder;
    }
}
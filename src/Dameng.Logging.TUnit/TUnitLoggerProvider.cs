using Microsoft.Extensions.Logging;

namespace Dameng.Logging.TUnit;

/// <summary>
/// TUnit logger provider for Microsoft.Extensions.Logging.
/// </summary>
/// <param name="testContext"></param>
/// <param name="includeScope"></param>
internal class TUnitLoggerProvider(TestContext testContext, bool includeScope) : ILoggerProvider
{
    /// <summary>
    /// Creates a new instance of <see cref="T:Microsoft.Extensions.Logging.ILogger" /> for the specified category name.
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns></returns>
    public ILogger CreateLogger(string categoryName)
    {
        return new TUnitLogger(testContext, categoryName, LogLevel.Trace, includeScope);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        // No resources to dispose
    }
}
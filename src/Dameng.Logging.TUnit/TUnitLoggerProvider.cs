using Microsoft.Extensions.Logging;

namespace Dameng.Logging.TUnit;

/// <summary>
/// TUnit logger provider for Microsoft.Extensions.Logging.
/// </summary>
internal class TUnitLoggerProvider(TestContext testContext, bool includeScope,string dateTimeFormat,bool includeDateTime) : ILoggerProvider
{
    /// <summary>
    /// Creates a new instance of <see cref="T:Microsoft.Extensions.Logging.ILogger" /> for the specified category name.
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns></returns>
    public ILogger CreateLogger(string categoryName)
    {
        return new TUnitLogger(testContext, categoryName, LogLevel.Trace, includeScope,dateTimeFormat,includeDateTime);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        // No resources to dispose
    }
}
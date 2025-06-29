using Microsoft.Extensions.Logging;

namespace Dameng.Logging.TUnit.Tests;

public class Tests
{
    [Test]
    [Arguments(LogLevel.Trace)]
    [Arguments(LogLevel.Information)]
    [Arguments(LogLevel.Error)]
    public void Basic(LogLevel minLevel)
    {
        var logger = TestContext.Current!.GetLogger("Basic", minLevel: minLevel);
        logger.LogCritical("This is a critical log message.");
        logger.LogError($"This is an error log message");
        logger.LogWarning("This is a warning log message.");
        logger.LogInformation("This is an information log message.");
        logger.LogDebug("This is a debug log message.");
        logger.LogTrace("This is a trace log message.");
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public void Scope(bool includeScope)
    {
        var logger = TestContext.Current!.GetLogger(
            "Scope",
            includeScope: includeScope
        );
        using (logger.BeginScope("Scope 1"))
        {
            logger.LogInformation("This is scope 1 message.");
            using (logger.BeginScope("Scope 2"))
            {
                logger.LogInformation($" This is scope 2 message");
            }

            logger.LogInformation("This is scope 1 message after scope 2.");
        }
    }

    [Test]
    public void Exception()
    {
        var logger = TestContext.Current!.GetLogger("Exception");
        try
        {
            throw new InvalidOperationException("This is a test exception.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the request.");
        }
    }

    [Test]
    public void ExceptionAndScope()
    {
        var logger = TestContext.Current!.GetLogger(
            "Exception",
            includeScope: true
        );
        using (logger.BeginScope("Scope for exception"))
        {
            try
            {
                throw new InvalidOperationException("This is a test exception with scope.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the request with scope.");
            }
        }
    }

    [Test]
    [Arguments(LogLevel.Trace)]
    [Arguments(LogLevel.Information)]
    [Arguments(LogLevel.Error)]
    public void LogBuilder(LogLevel minLevel)
    {
        var logger = LoggerFactory.Create(logging=>logging.AddTUnit(TestContext.Current!)
            .SetMinimumLevel(minLevel)).CreateLogger("ILogBuilder");
        
        logger.LogCritical("This is a critical log message.");
        logger.LogError($"This is an error log message");
        logger.LogWarning("This is a warning log message.");
        logger.LogInformation("This is an information log message.");
        logger.LogDebug("This is a debug log message.");
        logger.LogTrace("This is a trace log message.");
    }
    [Test]
    [Arguments(LogLevel.Trace)]
    [Arguments(LogLevel.Information)]
    [Arguments(LogLevel.Error)]
    public void LogBuilder_Generic(LogLevel minLevel)
    {
        var logger = LoggerFactory.Create(logging=>logging.AddTUnit(TestContext.Current!,includeScope:true)
            .SetMinimumLevel(minLevel)).CreateLogger<Tests>();
        
        logger.LogCritical("This is a critical log message.");
        logger.LogError($"This is an error log message");
        logger.LogWarning("This is a warning log message.");
        logger.LogInformation("This is an information log message.");
        logger.LogDebug("This is a debug log message.");
        logger.LogTrace("This is a trace log message.");
    }

}

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

    [Test]
    [Arguments(LogLevel.Trace)]
    [Arguments(LogLevel.Information)]
    [Arguments(LogLevel.Error)]
    public void GetLoggerFactory_Basic(LogLevel minLevel)
    {
        var loggerFactory = TestContext.Current!.GetLoggerFactory();
        var logger = loggerFactory.CreateLogger("GetLoggerFactory_Basic");
        
        logger.LogCritical("This is a critical log message from factory.");
        logger.LogError("This is an error log message from factory.");
        logger.LogWarning("This is a warning log message from factory.");
        logger.LogInformation("This is an information log message from factory.");
        logger.LogDebug("This is a debug log message from factory.");
        logger.LogTrace("This is a trace log message from factory.");
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public void GetLoggerFactory_WithScope(bool includeScope)
    {
        var loggerFactory = TestContext.Current!.GetLoggerFactory(includeScope: includeScope);
        var logger = loggerFactory.CreateLogger("GetLoggerFactory_WithScope");
        
        using (logger.BeginScope("Factory Scope 1"))
        {
            logger.LogInformation("This is factory scope 1 message.");
            using (logger.BeginScope("Factory Scope 2"))
            {
                logger.LogInformation("This is factory scope 2 message.");
            }
            logger.LogInformation("This is factory scope 1 message after scope 2.");
        }
    }

    [Test]
    public void GetLoggerFactory_WithCustomDateTime()
    {
        var customDateFormat = "HH:mm:ss";
        var loggerFactory = TestContext.Current!.GetLoggerFactory(dateTimeFormat: customDateFormat);
        var logger = loggerFactory.CreateLogger("GetLoggerFactory_CustomDateTime");
        
        logger.LogInformation("This message should have custom datetime format.");
    }

    [Test]
    [Arguments(LogLevel.Trace)]
    [Arguments(LogLevel.Information)]
    [Arguments(LogLevel.Error)]
    public void GetLoggerFactory_WithBuilderAction(LogLevel minLevel)
    {
        var loggerFactory = TestContext.Current!.GetLoggerFactory(
            includeScope: true,
            builderAction: builder => builder.SetMinimumLevel(minLevel)
        );
        var logger = loggerFactory.CreateLogger("GetLoggerFactory_WithBuilderAction");
        
        logger.LogCritical("This is a critical log message with builder action.");
        logger.LogError("This is an error log message with builder action.");
        logger.LogWarning("This is a warning log message with builder action.");
        logger.LogInformation("This is an information log message with builder action.");
        logger.LogDebug("This is a debug log message with builder action.");
        logger.LogTrace("This is a trace log message with builder action.");
    }

    [Test]
    public void GetLoggerFactory_WithComplexBuilderAction()
    {
        var loggerFactory = TestContext.Current!.GetLoggerFactory(
            includeScope: true,
            dateTimeFormat: "yyyy-MM-dd HH:mm:ss",
            builderAction: builder =>
            {
                builder.SetMinimumLevel(LogLevel.Debug);
                builder.AddFilter("GetLoggerFactory_Complex", LogLevel.Information);
            }
        );
        var logger = loggerFactory.CreateLogger("GetLoggerFactory_Complex");
        
        logger.LogInformation("This message should appear with complex builder configuration.");
        logger.LogDebug("This debug message might be filtered out.");
    }

    [Test]
    public void GetLoggerFactory_MultipleLoggers()
    {
        var loggerFactory = TestContext.Current!.GetLoggerFactory(includeScope: true);
        
        var logger1 = loggerFactory.CreateLogger("Logger1");
        var logger2 = loggerFactory.CreateLogger("Logger2");
        var logger3 = loggerFactory.CreateLogger<Tests>();
        
        logger1.LogInformation("Message from Logger1");
        logger2.LogWarning("Message from Logger2");
        logger3.LogError("Message from generic Logger3");
    }

    [Test]
    public void GetLoggerFactory_WithException()
    {
        var loggerFactory = TestContext.Current!.GetLoggerFactory(includeScope: true);
        var logger = loggerFactory.CreateLogger("GetLoggerFactory_Exception");
        
        using (logger.BeginScope("Exception handling scope"))
        {
            try
            {
                throw new InvalidOperationException("Test exception from logger factory.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred in logger factory test.");
            }
        }
    }

    [Test]
    public void GetLoggerFactory_NullTestContext_ThrowsArgumentNullException()
    {
        TestContext? nullContext = null;
        
        Assert.Throws<ArgumentNullException>(() => nullContext!.GetLoggerFactory());
    }
}

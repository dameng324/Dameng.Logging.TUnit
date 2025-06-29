# Introduction

Dameng.Logging.TUnit is a NuGet package that returns an `Microsoft.Extension.Logging.ILogger` that wraps around the `TestContext.Current.OutputWriter` supplied by [TUnit](https://github.com/thomhurst/TUnit).

You can use this logger to log messages in your TUnit tests, or set to user test cases, which will be captured and displayed in the test output.

## Installation

Run the following in the NuGet command line or visit the [NuGet package page](https://nuget.org/packages/Dameng.Logging.TUnit).

`Install-Package Dameng.Logging.TUnit`

## Usage

You can use `TestContext.Current.GetLogger()` to get an `ILogger` instance that writes to the TUnit output.

```csharp
using Dameng.Logging.TUnit;
using Microsoft.Extensions.Logging;

public class Tests
{
    [Test]
    public void Basic()
    {
        var logger = TestContext.Current!.GetLogger("Basic");
        logger.LogCritical("This is a critical log message.");
    }
}
```

this will output:

```
2025-06-29 15:20:13.198 crit: Basic [0]: This is a critical log message.
```

with scope:

```csharp
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
```

will output:
```
2025-06-29 15:26:54.394 info Scope: This is scope 1 message.
 => Scope 1 
2025-06-29 15:26:54.398 info Scope:  This is scope 2 message
 => Scope 1  => Scope 2 
2025-06-29 15:26:54.399 info Scope: This is scope 1 message after scope 2.
 => Scope 1
```

with Exception:

```csharp
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
```

will output:

```
2025-06-29 15:26:54.394 fail Exception: An error occurred while processing the request with scope.
 => Scope for exception 
System.InvalidOperationException: This is a test exception with scope.
   at Dameng.Logging.TUnit.Tests.Tests.ExceptionAndScope() in C:\Dameng.Logging.TUnit\tests\Dameng.Logging.TUnit.Tests\Tests.cs:line 68

```

### Using LoggerFactory

For more advanced scenarios, you can use the `GetLoggerFactory()` extension method to get an `ILoggerFactory` instance that's pre-configured with TUnit logging:

```csharp
using Dameng.Logging.TUnit;
using Microsoft.Extensions.Logging;

public class Tests
{
    [Test]
    public void LoggerFactoryBasic()
    {
        var loggerFactory = TestContext.Current!.GetLoggerFactory();
        var logger = loggerFactory.CreateLogger("MyCategory");
        logger.LogInformation("This is logged via LoggerFactory.");
    }
}
```

You can also configure additional logging providers alongside TUnit:

```csharp
[Test]
public void LoggerFactoryWithCustomConfiguration()
{
    var loggerFactory = TestContext.Current!.GetLoggerFactory(
        includeScope: true,
        dateTimeFormat: "HH:mm:ss",
        builderAction: builder =>
        {
            // Add additional logging providers or configuration
            builder.SetMinimumLevel(LogLevel.Debug);
            builder.AddFilter("MyCategory", LogLevel.Warning);
        }
    );
    
    var logger = loggerFactory.CreateLogger("MyCategory");
    logger.LogInformation("This message will be filtered out due to the filter above.");
    logger.LogWarning("This warning will be displayed.");
}
```

#### ILogBuilder

You can also use `ILogBuilder` to configure the logger with additional options.

```csharp
var logger = LoggerFactory.Create(logging=>
    logging.AddTUnit(TestContext.Current!)
    ).CreateLogger("Basic");
```

## Configuration

### Minimum Log Level

You can set the minimum log level for the logger when using `TestContext.Current!.GetLogger` or `ILogBuilder`.

```csharp
var logger = TestContext.Current!.GetLogger("Basic",minLevel: LogLevel.Trace);

var logger = LoggerFactory.Create(logging=>
    logging.AddTUnit(TestContext.Current!).SetMinimumLevel(LogLevel.Trace)
    ).CreateLogger("Basic");
```

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
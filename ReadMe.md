# Dameng.Logging.TUnit

[![NuGet](https://img.shields.io/nuget/v/Dameng.Logging.TUnit.svg)](https://www.nuget.org/packages/Dameng.Logging.TUnit/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Dameng.Logging.TUnit.svg)](https://www.nuget.org/packages/Dameng.Logging.TUnit/)
[![GitHub](https://img.shields.io/github/license/dameng324/Dameng.Logging.TUnit.svg)](https://github.com/dameng324/Dameng.Logging.TUnit/blob/master/LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%209.0%20%7C%20Standard%202.0-512BD4)](https://dotnet.microsoft.com/)

> 🧪 **Microsoft.Extensions.Logging integration for TUnit testing framework**

A modern NuGet package that provides an `ILogger` implementation seamlessly integrated with [TUnit](https://github.com/thomhurst/TUnit)'s `TestContext.Current.OutputWriter`. Perfect for capturing structured logging output directly in your test results.

## ✨ Features

- 🎯 **Native TUnit Integration** - Works seamlessly with TUnit's test context
- 📝 **Structured Logging** - Full support for Microsoft.Extensions.Logging features
- 🔍 **Scope Support** - Hierarchical logging scopes for better test organization
- ⚡ **Multiple Target Frameworks** - Supports .NET 8.0, .NET 9.0, and .NET Standard 2.0
- 🎛️ **Configurable** - Flexible configuration options for different testing scenarios
- 🚀 **Zero Dependencies** - Minimal footprint with only essential dependencies

## 📦 Installation

### Package Manager Console
```powershell
Install-Package Dameng.Logging.TUnit
```

### .NET CLI
```bash
dotnet add package Dameng.Logging.TUnit
```

Or visit the [NuGet package page](https://www.nuget.org/packages/Dameng.Logging.TUnit/) for more installation options.

## 🚀 Quick Start

Get started with `Dameng.Logging.TUnit` in seconds! Use `TestContext.Current.GetLogger()` to get an `ILogger` instance that writes directly to your TUnit test output.

### Basic Usage

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

**Output:**
```text
this will output:

```text
2025-06-29 15:20:13.198 crit: Basic [0]: This is a critical log message.
```

### Logging with Scopes

```csharp
var logger = TestContext.Current!.GetLogger(
    "Scope",
    includeScope: true
);
using (logger.BeginScope("Scope 1"))
{
    logger.LogInformation("This is scope 1 message.");
    using (logger.BeginScope("Scope 2"))
    {
        logger.LogInformation("This is scope 2 message");
    }
    logger.LogInformation("This is scope 1 message after scope 2.");
}
```

**Output:**
```text
2025-06-29 15:26:54.394 info Scope: This is scope 1 message.
 => Scope 1 
2025-06-29 15:26:54.398 info Scope: This is scope 2 message
 => Scope 1  => Scope 2 
2025-06-29 15:26:54.399 info Scope: This is scope 1 message after scope 2.
 => Scope 1
```

### Exception Logging

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

**Output:**
```text
2025-06-29 15:26:54.394 fail Exception: An error occurred while processing the request with scope.
 => Scope for exception 
System.InvalidOperationException: This is a test exception with scope.
   at Dameng.Logging.TUnit.Tests.Tests.ExceptionAndScope() in C:\...\Tests.cs:line 68
```

## 🔧 Advanced Usage

### Using LoggerFactory

For more complex scenarios, use the `GetLoggerFactory()` extension method to get a pre-configured `ILoggerFactory` with TUnit logging:

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

### Custom Configuration

Configure additional logging providers and filters alongside TUnit:

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

### Using ILoggerBuilder

Alternative approach using `ILoggerBuilder` for additional configuration options:

```csharp
var logger = LoggerFactory.Create(logging =>
    logging.AddTUnit(TestContext.Current!)
).CreateLogger("Basic");
```

## ⚙️ Configuration

### Minimum Log Level

Control the minimum log level when creating loggers:

#### Using GetLogger
```csharp
var logger = TestContext.Current!.GetLogger("Basic", minLevel: LogLevel.Trace);
```

### DateTime Format and Display

You can control the datetime format and whether to include datetime in the log output:

```csharp
// Custom datetime format
var logger = TestContext.Current!.GetLogger(
    "CustomDateTime",
    dateTimeFormat: "HH:mm:ss"
);
logger.LogInformation("This message uses custom datetime format.");
```

Output:

```text
15:30:45 info: CustomDateTime [0]: This message uses custom datetime format.
```

```csharp
// Disable datetime display
var logger = TestContext.Current!.GetLogger(
    "NoDateTime",
    includeDateTime: false
);
logger.LogInformation("This message has no datetime.");
```

Output:

```text
info: NoDateTime [0]: This message has no datetime.
```

You can also configure datetime settings when using `GetLoggerFactory()`:

```csharp
var loggerFactory = TestContext.Current!.GetLoggerFactory(
    dateTimeFormat: "yyyy-MM-dd HH:mm:ss",
    includeDateTime: true
);
var logger = loggerFactory.CreateLogger("MyCategory");
logger.LogInformation("Custom datetime format via factory.");
```

Or when using `AddTUnit` with `ILoggingBuilder`:

```csharp
var logger = LoggerFactory.Create(logging =>
    logging.AddTUnit(
        TestContext.Current!,
        dateTimeFormat: "HH:mm:ss.fff",
        includeDateTime: false
    )
).CreateLogger("NoDateTimeLogger");
```

## License

### Available Configuration Options

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `categoryName` | `string` | Required | The category name for the logger |
| `includeScope` | `bool` | `false` | Whether to include scope information in log output |
| `dateTimeFormat` | `string` | `"yyyy-MM-dd HH:mm:ss.fff"` | Custom date/time format |
| `minLevel` | `LogLevel` | `LogLevel.Information` | Minimum log level to output |
| `builderAction` | `Action<ILoggingBuilder>` | `null` | Additional configuration for the logging builder |

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

## 📋 Requirements

- .NET 8.0, .NET 9.0, or .NET Standard 2.0
- TUnit testing framework
- Microsoft.Extensions.Logging

## 📝 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## 🔗 Related Projects

- [TUnit](https://github.com/thomhurst/TUnit) - The modern testing framework this package integrates with
- [Microsoft.Extensions.Logging](https://docs.microsoft.com/en-us/dotnet/core/extensions/logging) - The logging abstraction this package implements

---

**Made with ❤️ by [dameng324](https://github.com/dameng324)**
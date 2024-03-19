using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Serilog;
namespace FileManagementBot;

/// <summary>
/// Class responsible for configuring and providing logging functionality.
/// </summary>
public class Logging
{
    // The service provider for managing dependencies.
    private static ServiceProvider _serviceProvider;
    
    // The logger instance for logging messages.
    private static ILogger? _logger;

    /// <summary>
    /// Retrieves the logger instance.
    /// </summary>
    /// <returns>The logger instance.</returns>
    internal static ILogger? GetLogger()
    {
        return _logger;
    }

    /// <summary>
    /// Initializes a new instance of the Logging class.
    /// </summary>
    public Logging()
    {
        Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File(Path.Combine("..", "..", "..", "var", "bot.log")).CreateLogger();

        var services = new ServiceCollection();
        
        services.AddLogging(loggerBuilder => loggerBuilder.AddConsole());
        services.AddLogging(loggerBuilder => loggerBuilder.AddSerilog(dispose: true));
        
        _serviceProvider = services.BuildServiceProvider();
        _logger = _serviceProvider.GetService<ILogger<Program>>();
    }

    /// <summary>
    /// Destructor of the Logging class.
    /// </summary>
    ~Logging()
    {
        _serviceProvider.Dispose();
    }
}
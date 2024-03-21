using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Serilog;
namespace FileManagementBot;

/// <summary>
/// Class responsible for configuring and providing logging functionality.
/// </summary>
internal class Logging
{
    // The service provider for managing dependencies.
    private static ServiceProvider s_serviceProvider;
    
    // The logger instance for logging messages.
    private static ILogger? s_logger;

    /// <summary>
    /// Retrieves the logger instance.
    /// </summary>
    /// <returns>The logger instance.</returns>
    internal static ILogger? GetLogger()
    {
        return s_logger;
    }

    /// <summary>
    /// Initializes a new instance of the Logging class.
    /// </summary>
    internal Logging()
    {
        Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File(Path.Combine("..", "..", "..", "var", "bot.log")).CreateLogger();

        var services = new ServiceCollection();
        
        services.AddLogging(loggerBuilder => loggerBuilder.AddConsole());
        services.AddLogging(loggerBuilder => loggerBuilder.AddSerilog(dispose: true));
        
        s_serviceProvider = services.BuildServiceProvider();
        s_logger = s_serviceProvider.GetService<ILogger<Program>>();
    }

    /// <summary>
    /// Destructor of the Logging class.
    /// </summary>
    ~Logging()
    {
        s_serviceProvider.Dispose();
    }
}
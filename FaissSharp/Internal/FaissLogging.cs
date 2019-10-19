using Microsoft.Extensions.Logging;

namespace FaissSharp.Internal
{
internal static class FaissLogging
{
    private static ILoggerFactory _loggerFactory;
    internal static ILoggerFactory LoggerFactory { get
    {
        if(_loggerFactory == null)
        {
            _loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("FaissSharp", LogLevel.Information)
                    .AddConsole();
            });
        }
        return _loggerFactory;
    } set
    {
        _loggerFactory = value;
    } }
    internal static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
    internal static ILogger CreateLogger(string categoryName) => LoggerFactory.CreateLogger(categoryName);
}
}
# Logger Example Snippet

This snippet shows example on how to configure log4net logger with SDK
```csharp
class LoggerAdapter: ILoggerAdapter
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ApiClient));

    public void Info(string message) => log.Info(message);
    public void Debug(string message) => log.Debug(message);
    public void Warning(string message) => log.Warn(message);
    public void Error(string message) => log.Error(message);
    public void Fatal(string message) => log.Fatal(message);

    public bool IsLevelEnabled(LogLevel level)
    {
        switch (level)
        {
            case LogLevel.INFO:
                return log.IsInfoEnabled;
            case LogLevel.DEBUG:
                return log.IsDebugEnabled;
            case LogLevel.WARNING:
                return log.IsWarnEnabled;
            case LogLevel.ERROR:
                return log.IsErrorEnabled;
            case LogLevel.FATAL:
                return log.IsFatalEnabled;
            default:
                return false;
        }
    }
}
```
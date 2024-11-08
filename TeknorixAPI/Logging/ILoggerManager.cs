namespace TeknorixAPI.Logging
{
    public interface ILoggerManager
    {
        void LogInformation(string message);
        void LogError(string message);
    }
}

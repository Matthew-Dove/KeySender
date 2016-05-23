namespace Web.KeySender.Models
{
    public class LogModel
    {
        public bool IsLoggingEnabled { get; }

        public LogModel(bool isLoggingEnabled)
        {
            IsLoggingEnabled = isLoggingEnabled;
        }
    }
}
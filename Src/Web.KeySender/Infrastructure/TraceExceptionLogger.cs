using System.Web.Http.ExceptionHandling;

namespace Web.KeySender.Infrastructure
{
    public class TraceExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context) => KeySender.Log.Error(context.ExceptionContext.Exception);
    }
}
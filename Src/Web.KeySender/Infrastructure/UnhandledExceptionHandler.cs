using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Web.KeySender.Infrastructure
{
    public class UnhandledExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context) => context.Result = new InternalServerErrorResult(context.Request);

        public override bool ShouldHandle(ExceptionHandlerContext context) => context.ExceptionContext.CatchBlock.IsTopLevel;
    }
}
using Newtonsoft.Json.Converters;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Web.KeySender.Infrastructure;

namespace Web.KeySender
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "WebApi",
                routeTemplate: "api/{controller}"
            );

            config.Routes.MapHttpRoute(
                name: "WebApiWithAction",
                routeTemplate: "api/{controller}/{action}"
            );

            // JSON converts enums to their names, not their int values.
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());

            // Global error handling.
            config.Services.Add(typeof(IExceptionLogger), new TraceExceptionLogger());
            config.Services.Replace(typeof(IExceptionHandler), new UnhandledExceptionHandler());
        }
    }
}
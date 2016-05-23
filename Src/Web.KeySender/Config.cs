using System.Web.Configuration;

namespace Web.KeySender
{
    public static class Config
    {
        /// <summary>Shared secret between this website, and the calling conole app "KeySender".</summary>
        public static string CommandSecretKey { get { return WebConfigurationManager.AppSettings["CommandSecretKey"]; } }

        /// <summary>Shared secret between this website, and client when they push commands through signalR from the browser.</summary>
        public static string ClientSecretKey { get { return WebConfigurationManager.AppSettings["ClientSecretKey"]; } }

        /// <summary>When enabled, logs are sent to the logging page.</summary>
        public static bool IsLoggingEnabled { get { return bool.Parse(WebConfigurationManager.AppSettings["IsLoggingEnabled"]); } }

    }
}
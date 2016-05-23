using Microsoft.AspNet.SignalR;
using System;
using Web.KeySender.Hubs;

namespace Web.KeySender
{
    /// <summary>Sends traces, and logs out to the client on the page "~/Log".</summary>
    public static class Log
    {
        public static void Error(Exception ex)
        {
            if (Config.IsLoggingEnabled)
            {
                GlobalHost.ConnectionManager.GetHubContext<LogHub>().Clients.All.sendError(ex.GetBaseException().Message);
            }
        }

        public static void Trace(string message, params object[] args)
        {
            if (Config.IsLoggingEnabled)
            {
                GlobalHost.ConnectionManager.GetHubContext<LogHub>().Clients.All.sendTrace(string.Format(message, args));
            }
        }
    }
}
using KeySender.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace KeySender.Core
{
    static class Client
    {
        public static async Task<KeyCommand> GetNextCommandAsync()
        {
            var command = KeyCommand.Nothing;
            var sw = Stopwatch.StartNew();
            Log.Trace("Asking the server for the next command.");

            try
            {
                bool isParsed = false;
                string json = null;
                using (var wc = new WebClient())
                {
                    wc.Headers.Add(HttpRequestHeader.Accept, "application/json");
                    json = await wc.DownloadStringTaskAsync(Config.KeyCommanderEndpoint + "/Api/KeyCommand");
                }

                sw.Stop();
                Log.Trace("New command received, time spent waiting: {0}.", sw.Elapsed);

                var jObject = JObject.Parse(json);
                var hashToken = jObject.GetValue("hash", StringComparison.OrdinalIgnoreCase);
                var serverHash = hashToken?.Value<string>();
                var commandToken = jObject.GetValue("command", StringComparison.OrdinalIgnoreCase);
                var serverCommand = commandToken?.Value<string>();

                if (!string.IsNullOrEmpty(serverHash) && !string.IsNullOrEmpty(serverCommand))
                {
                    var isVerified = Security.Verify(serverCommand, serverHash);
                    isParsed = isVerified && Enum.TryParse(serverCommand, out command) && Config.AllowedKeys.Contains(command);
                    if (!isParsed)
                    {
                        command = KeyCommand.Nothing;
                    }
                }

                Log.Trace(isParsed ? "The command ({0}) was successfully parsed." : "The command ({0}) is not a known command.", serverCommand);
            }
            catch (TimeoutException)
            {
                sw.Stop();
                Log.Trace("Client timed out ({0}) waiting on a key command.", sw.Elapsed);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return command;
        }
    }
}

using KeySender.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KeySender.Core
{
    static class Runner
    {
        public static async Task StartAsync(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Polling for commands aborted.");
                    break;
                }
                Log.Trace("\r\n"); // Some space in the console between loops.
                await Task.Delay(100); // Poor man's throttle, so we don't spam applications too much.

                var command = await Client.GetNextCommandAsync();
                if (command != KeyCommand.Nothing)
                {
                    Messenger.SendKey(command);
                }
            }
        }
    }
}

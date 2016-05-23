using KeySender.Core;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace KeySender
{
    class Program
    {
        private const string DEFAULT_COMMAND_SECRET_KEY = "ChangeToYourCustomSecretKey_ItCanBeAnyAsciiText";

        static void Main(string[] args)
        {
            if (args != null && args.Length == 1 && args[0] == "-log")
            {
                Config.EnableLogging();
                Log.Trace("Logging Enabled");
            }
            else
            {
                Console.WriteLine("To enable logging start the process with the argument \"-log\".");
            }

            using (var source = new CancellationTokenSource())
            {
                var task = Task.Factory.StartNew(() => Setup(source.Token));
                Console.ReadKey(true);
                source.Cancel();
                Console.WriteLine("Ending Process");
                task.Wait();
            }
        }

        static void Setup(CancellationToken token)
        {
            if (!Debugger.IsAttached)
            {
                if (Config.CommandSecretKey == DEFAULT_COMMAND_SECRET_KEY)
                {
                    throw new InvalidOperationException("You still have the default value set for \"CommandSecretKey\", please go to App.Config, and change it.");
                }
                else if (string.IsNullOrWhiteSpace(Config.CommandSecretKey))
                {
                    throw new InvalidOperationException("The App Setting \"CommandSecretKey\" requires a value, please go to App.Config, and change it.");
                }

                Console.WriteLine("Press anykey to end...");
                Thread.Sleep(3000); // Give the user some time to view the startup messages.
            }

            Runner.StartAsync(token).Wait();
        }
    }
}

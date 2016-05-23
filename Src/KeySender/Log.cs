using System;

namespace KeySender
{
    static class Log
    {
        public static void Error(Exception ex)
        {
            if (Config.IsLoggingEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: {0}", ex.GetBaseException().Message);
                Console.ResetColor();

                // Give the user a chance to read the error message.
                Console.WriteLine("Press anykey to continue...");
            }
        }

        public static void Trace(string message, params object[] args)
        {
            if (Config.IsLoggingEnabled)
            {
                Console.WriteLine(message, args);
            }
        }
    }
}

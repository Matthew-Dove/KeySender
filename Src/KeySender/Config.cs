using KeySender.Models;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace KeySender
{
    static class Config
    {
        public static void EnableLogging() => _isLoggingEnabled = true;

        /// <summary>When enabled, logs are sent to the console.</summary>
        public static bool IsLoggingEnabled { get { return _isLoggingEnabled; } }
        private static bool _isLoggingEnabled = false;

        /// <summary>Location of the Web.KeySender site.</summary>
        public static string KeyCommanderEndpoint { get { return ConfigurationManager.AppSettings["KeyCommanderEndpoint"]; } }

        /// <summary>A shared secret between the this app, and the web project "Web.KeySender".</summary>
        public static string CommandSecretKey { get { return ConfigurationManager.AppSettings["CommandSecretKey"]; } }

        /// <summary>A set of key commands this program will accept from the server.</summary>
        public static HashSet<KeyCommand> AllowedKeys { get { return _allowedKeys; } }
        private readonly static HashSet<KeyCommand> _allowedKeys = GetAllowedKeys();

        private static HashSet<KeyCommand> GetAllowedKeys()
        {
            var allowedKeys = new HashSet<KeyCommand>();
            var allKeys = ConfigurationManager.AppSettings["AllowedKeys"];

            if (string.IsNullOrWhiteSpace(allKeys))
            {
                Log.Trace("No allowed keys from the server have been set.");
            }
            else
            {
                var keys = allKeys.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < keys.Length; i++)
                {
                    var key = keys[i].Trim();
                    Log.Trace("Adding the key \"{0}\" to the list of allowed key commands.", key);
                    allowedKeys.Add((KeyCommand)Enum.Parse(typeof(KeyCommand), key));
                }
            }

            return allowedKeys;
        }
    }
}

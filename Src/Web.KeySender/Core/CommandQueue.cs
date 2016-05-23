using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Web.KeySender.Models;

namespace Web.KeySender.Core
{
    /// <summary>A thread-safe queue for keys sent from the client, they wait here until they're dequeued from the console app "KeySender".</summary>
    public static class CommandQueue
    {
        /// <summary>Number of milliseconds to wait between checking for new keys.</summary>
        private const int SLEEP_MILLISECONDS = 100;
        /// <summary>The total number of attempts to check for new keys commands before giving up.</summary>
        private const int MAX_ATTEMPTS = 150;

        private readonly static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly static ConcurrentQueue<KeyCommand> _commands = new ConcurrentQueue<KeyCommand>();

        public static void Enqueue(string key)
        {
            var command = KeyCommand.Nothing;
            if (Enum.TryParse(key, out command))
            {
                _commands.Enqueue(command);
                Log.Trace("Added the key command {0} the CommandQueue.", command);
            }
            else
            {
                Log.Trace("The key {0} isn't a known value, it won't be added to the CommandQueue.", key);
            }
        }

        public static async Task<KeyCommand> DequeueAsync()
        {
            var key = KeyCommand.Nothing;
            var entered = await _semaphore.WaitAsync(1); // Only allow one thread to take keys at a time (doesn't make sense to send some keys to device A, and others to device B).

            if (entered)
            {
                Log.Trace("Entered the semaphore, waiting for a new key command.");

                try
                {
                    key = await GetCommandKeyAsync(0);
                }
                finally
                {
                    _semaphore.Release();
                }

                Log.Trace(key == KeyCommand.Nothing ? "No key was found from the command queue, returning the key {0}." : "The key {0} was successfully dequeued.", key);
            }
            else
            {
                Log.Trace("Attempted to take a semaphore, and wait for a new key, but another process is already waiting.");
            }

            return key;
        }

        /// <summary>Periodically check the command queue for a new key, until the maximum attempts is reached, or a key is dequeued.</summary>
        private static async Task<KeyCommand> GetCommandKeyAsync(int attempts)
        {
            var key = KeyCommand.Nothing;

            if (attempts < MAX_ATTEMPTS && !_commands.TryDequeue(out key))
            {
                await Task.Delay(SLEEP_MILLISECONDS);
                key = await GetCommandKeyAsync(attempts + 1);
            }

            return key;
        }
    }
}
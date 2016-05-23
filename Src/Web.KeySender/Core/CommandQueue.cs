using System;
using System.Collections.Concurrent;
using Web.KeySender.Models;

namespace Web.KeySender.Core
{
    /// <summary>A thread-safe queue for keys sent from the client, they wait here until they're dequeued from the console app "KeySender".</summary>
    public static class CommandQueue
    {
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

        public static KeyCommand Dequeue()
        {
            var key = KeyCommand.Nothing;

            if (!_commands.TryDequeue(out key))
            {
                // TODO: Block the thread while we wait for a key send event.
            }

            return key;
        }
    }
}
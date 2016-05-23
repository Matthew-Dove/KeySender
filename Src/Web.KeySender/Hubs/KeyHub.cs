using Microsoft.AspNet.SignalR;
using System;
using Web.KeySender.Core;

namespace Web.KeySender.Hubs
{
    public class KeyHub : Hub
    {
        /// <summary>Keys are sent from the client, on the page "~/Key".</summary>
        /// <param name="key">The key the client wants to send.</param>
        /// <param name="hash">The hash of the key that was sent.</param>
        public void receiveKey(string key, string hash)
        {
            try
            {
                Log.Trace("Key received: {0}.", key);
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(hash))
                {
                    var isVerified = Security.Verify(key, hash);
                    Log.Trace(isVerified ? "The body and hash have been successfully verified." : "The verification of the body failed.");

                    if (isVerified)
                    {
                        CommandQueue.Enqueue(key);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
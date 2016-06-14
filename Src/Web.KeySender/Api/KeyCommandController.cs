using System;
using System.Threading.Tasks;
using System.Web.Http;
using Web.KeySender.Core;
using Web.KeySender.Models;

namespace Web.KeySender.Api
{
    public class KeyCommandController : ApiController
    {
        /// <summary>Client requests the server for the next key value.</summary>
        /// <returns>The key to send to the client's active application.</returns>
        public async Task<KeyCommand> Get()
        {
            var key = KeyCommand.Nothing;
            Log.Trace("A key was requested.");

            try
            {
                key = await CommandQueue.DequeueAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            Log.Trace("The key {0} is being returned.", key);
            return key;
        }
    }
}

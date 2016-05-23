using System;
using System.Security.Cryptography;
using System.Text;

namespace KeySender.Core
{
    static class Security
    {
        /// <summary>Makes sure the message we got from the server hasn't been modified.</summary>
        /// <param name="body">The command form the server.</param>
        /// <param name="base64Hash">The body's hash from the server.</param>
        /// <returns></returns>
        public static bool Verify(string body, string base64Hash)
        {
            var hash = Sign(body, Config.CommandSecretKey);
            var isVerified = SlowEquals(hash, Convert.FromBase64String(base64Hash));

            if (!isVerified)
            {
                Log.Trace("The body hash verification failed.");
            }

            return isVerified;
        }

        private static byte[] Sign(string body, string key)
        {
            var bodyBytes = Encoding.ASCII.GetBytes(body);
            var keyBytes = Encoding.ASCII.GetBytes(key);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                return hmac.ComputeHash(bodyBytes);
            }
        }

        /// <summary>A constant time equals check, so attackers can't guess individual characters.</summary>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;

            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }

            return diff == 0;
        }
    }
}

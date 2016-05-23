using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Web.KeySender.Core
{
    public static class Security
    {
        private const string DEFAULT_CLIENT_SECRET_KEY = "Mak1";

        /// <summary>Ensures the body hasn't been modified.</summary>
        /// <param name="body">The plaintext sent over.</param>
        /// <param name="hexHash">A hash of the plaintext.</param>
        /// <returns></returns>
        public static bool Verify(string body, string hexHash)
        {
            var isVerified = false;

            if (Debugger.IsAttached || Config.ClientSecretKey != DEFAULT_CLIENT_SECRET_KEY) // Don't allow successful verifications if the user never changed the secret key.
            {
                var hash = Sign(body, Config.ClientSecretKey);
                isVerified = SlowEquals(hash, HexStringToByteArray(hexHash.ToUpper()));
            }
            else
            {
                Log.Trace("Will not attempt to verify the body, as the ClientSecretKey hasn't been changed from the default value in the config.");
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

        private static byte[] HexStringToByteArray(string hex)
        {
            byte[] arr = null;

            if (hex.Length % 2 == 0) // Cannot have an odd number of digits.
            {
                arr = new byte[hex.Length >> 1];

                for (int i = 0; i < hex.Length >> 1; ++i)
                {
                    arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
                }
            }

            return arr ?? new byte[1] { 0 };
        }

        private static int GetHexVal(char hex)
        {
            int val = hex;
            return val - (val < 58 ? 48 : 55);
        }
    }
}
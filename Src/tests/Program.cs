using System;
using System.Security.Cryptography;
using System.Text;

namespace tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var hash = Sign("F5", "Abcd1234");
            var base64Hash = Convert.ToBase64String(hash);
            var isVerified = Verify("F5", "Abcd1234", base64Hash);
        }

        static byte[] Sign(string body, string key)
        {
            var bodyBytes = Encoding.ASCII.GetBytes(body);
            var keyBytes = Encoding.ASCII.GetBytes(key);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                return hmac.ComputeHash(bodyBytes);
            }
        }

        static bool Verify(string body, string key, string base64Hash)
        {
            var hash = Sign(body, key);
            return SlowEquals(hash, Convert.FromBase64String(base64Hash));
        }

        /// <summary>A constant time equals check, so attackers can't guess individual characters.</summary>
        static bool SlowEquals(byte[] a, byte[] b)
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

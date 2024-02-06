using System;
using System.Security.Cryptography;

namespace CSTool.Handlers
{
    internal class HashHandler
    {
        private static string salt = "k@39JRdNUF63Usf5";
        public static string GetHash(string text)
        {

            using (var sha = new SHA256Managed())
            {
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
        }
    }
}

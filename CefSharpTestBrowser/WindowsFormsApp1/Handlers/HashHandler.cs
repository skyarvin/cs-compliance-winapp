using System;
using System.Security.Cryptography;
using WindowsFormsApp1;

namespace CSTool.Handlers
{
    internal class HashHandler
    {
        public static string GetHash(string text)
        {

            using (var sha = new SHA256Managed())
            {
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + Globals.salt);
                byte[] hashBytes = sha.ComputeHash(textBytes); 

                return BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }
        }
    }
}

using System;
using System.Security.Cryptography;
using System.Text;

namespace PeD.Core.Extensions
{
    public static class StringExtension
    {
        public static string ToBase64(this string text)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(text));
        }
        public static string ToSHA256(this string text) => Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(text)));
    }
}
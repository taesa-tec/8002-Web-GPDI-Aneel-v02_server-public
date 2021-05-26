using System;
using System.Text;

namespace PeD.Core.Extensions
{
    public static class StringExtension
    {
        public static string ToBase64(this string text)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(text));
        }
    }
}
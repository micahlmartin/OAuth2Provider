using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider
{
    public static class EncodingExtensions
    {
        public static string ToBase65(this byte[] inArray)
        {
            var encoding = new Base65Encoding();
            return encoding.Encode(inArray);
        }

        public static byte[] FromBase65(this string inString)
        {
            var encoding = new Base65Encoding();
            return encoding.Decode(inString);
        }
    }
}

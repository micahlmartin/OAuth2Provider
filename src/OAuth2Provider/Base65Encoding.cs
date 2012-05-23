using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider
{
    public class Base65Encoding
    {
        private static string Characters = "0123456789AaBbCcDdEeFfGgHhIiJjKklLMmNnOoPpQqRrSsTtUuVvWwXxYyZz._-";

        public string Encode(byte[] inArray)
        {
            var sb = new StringBuilder();

            foreach (var b in inArray)
            {
                var val = IntToString(b, Characters);
                if (val.Length == 1)
                    sb.Append("0").Append(val);
                else
                    sb.Append(val);
            }

            return sb.ToString();
        }

        public byte[] Decode(string inString)
        {
            if (string.IsNullOrEmpty(inString))
                throw new ArgumentNullException("inString");

            if (inString.Length % 2 != 0)
                throw new ArgumentException("Invalid string length", "inString");

            var bytes = new List<byte>();

            for (int i = 0; i < inString.Length; i += 2)
            {
                var ch = inString[i];
                var ch2 = inString[i + 1];
                var index = Characters.IndexOf(ch);
                var index2 = Characters.IndexOf(ch2);

                if (index < 0 || index2 < 0)
                    throw new ArgumentException("Invalid character: " + ch);

                int val = 0;
                val += (index * Characters.Length) + index2;

                bytes.Add((byte)val);
            }

            return bytes.ToArray();
        }

        public static string IntToString(int value, string baseChars)
        {
            // 32 is the worst cast buffer size for base 2 and int.MaxValue
            int i = 32;
            char[] buffer = new char[i];
            int targetBase = baseChars.Length;

            do
            {
                buffer[--i] = baseChars[value % targetBase];
                value = value / targetBase;
            }
            while (value > 0);

            char[] result = new char[32 - i];
            Array.Copy(buffer, i, result, 0, 32 - i);

            return new string(result);
        }
        public static string Convert(int val, int baseLength)
        {
            if (val < baseLength)
                return Characters[val].ToString();

            return Convert(val / baseLength, baseLength) + Characters[val % baseLength].ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Contracts
{
    public static class KebabStringFormatter
    {
        public static string ToKebab(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            var sb = new StringBuilder(s.Length + 5);
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                bool needsDash = char.IsUpper(c) && i > 0 &&
                    (!char.IsUpper(s[i - 1]) ||
                     (i + 1 < s.Length && char.IsLower(s[i + 1])));
                if (needsDash) sb.Append('-');
                sb.Append(char.ToLowerInvariant(c));
            }
            return sb.ToString();
        }
    }
}

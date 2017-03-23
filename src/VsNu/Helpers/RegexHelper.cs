using System;
using System.Text.RegularExpressions;

namespace VsNu.Helpers
{
    class RegexHelper
    {
        public static String WildCardToRegEx(String wildcard)
        {
            return "^" + Regex.Escape(wildcard).Replace("\\?", ".").Replace("\\*", ".*") + "$";
        }
    }
}

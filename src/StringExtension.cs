using System;
using System.Text.RegularExpressions;

namespace Starcraft_BO_helper
{
    // Extension of String classe to add a method
    public static class StringExtension
    {
        // Return cleared string of whiteSpace not including single space
        public static String ClearWhiteSpace(this String str)
        {
            return Regex.Replace(str, @"^\s+|\s+$|\s+(?=\s)", "");
        }
    }
}

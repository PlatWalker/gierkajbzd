using System.Text.RegularExpressions;

namespace jbzd.Common.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Converts a camelCase or PascalCase string into a more readable format by adding spaces before uppercase letters.
        /// The first letter of the result is capitalized, and the rest are in lowercase.
        /// </summary>
        /// <param name="text">The input string in camelCase or PascalCase format.</param>
        /// <returns>A human-readable string with spaces before uppercase letters, with the first letter capitalized.</returns>
        public static string MakeReadableText(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            string result = Regex.Replace(text, "(?<!^)([A-Z])", " $1");

            result = char.ToUpper(result[0]) + result[1..].ToLower();

            return result;
        }
    }
}

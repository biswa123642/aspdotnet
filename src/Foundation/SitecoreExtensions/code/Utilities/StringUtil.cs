using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CGP.Foundation.SitecoreExtensions.Utilities
{
    public static class StringUtil
    {
        public static string Humanize(this string input)
        {
            return Regex.Replace(input, "(\\B[A-Z])", " $1");
        }

        public static string ToCssUrlValue(this string url)
        {
            return string.IsNullOrWhiteSpace(url) ? "none" : $"url('{url}')";
        }

        public static string RemoveSpecialCharactersExceptSpaceQuotes(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return string.Empty;

            return Regex.Replace(inputString.ToLower(), @"[^0-9a-zàâçéèêëîïôûùüÿñæœA-Z\s|""+.-]*", string.Empty);
        }
        public static string RemoveSpecialCharactersExceptSpace(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return string.Empty;

            return Regex.Replace(inputString.ToLower(), @"[^0-9a-zàâçéèêëîïôûùüÿñæœA-Z\s|""+.-]*", string.Empty);
        }
        public static string RemoveSpecialCharacters(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return string.Empty;

            return Regex.Replace(inputString, @"[^0-9a-zA-Z]+", string.Empty);
        }

        public static bool IdEqualGuid(Sitecore.Data.ID id, string guidString)
        {
            var strId = GetTemplateGuidString(id);
            return strId.Equals(guidString, System.StringComparison.InvariantCultureIgnoreCase);
        }

        public static string GetTemplateGuidString(Sitecore.Data.ID id)
        {
            var strId = id.ToString().ToLower();
            return Regex.Replace(strId, @"[{}]+", string.Empty);
        }
        public static string ToTitleCase(string str)
        {
            return !string.IsNullOrWhiteSpace(str) ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower()) : string.Empty;
        }

        public static string TruncateAtWord(string input, int limit)
        {
            if (input == null || input.Length < limit)
            {
                return input;
            }
            var iNextSpace = input.LastIndexOf(" ", limit, StringComparison.Ordinal);
            var trimmedInput = string.Format("{0}", input.Substring(0, (iNextSpace > 0) ? iNextSpace : limit).Trim()) + "..."; ;
            return trimmedInput;
        }
    }
}
using System.Globalization;

namespace RecruitingChallenge.Common.Extensions
{
    public static class StringExtensions
    {
        public static string TryFormatAsDate(this string value, string format = "yyyy-MM-dd")
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                return date.ToString(format);

            return value;
        }
    }
}

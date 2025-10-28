using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingChallenge.Common.Helpers
{
    public class EnumHelper
    {
        public static bool TryParseEnumStrict<TEnum>(string? input, out TEnum result)
            where TEnum : struct, Enum
        {
            result = default;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            if (int.TryParse(input, out var numericValue))
            {
                if (Enum.IsDefined(typeof(TEnum), numericValue))
                {
                    result = (TEnum)Enum.ToObject(typeof(TEnum), numericValue);
                    return true;
                }
                return false;
            }

            if (Enum.TryParse<TEnum>(input, true, out var parsed) &&
                Enum.IsDefined(typeof(TEnum), parsed))
            {
                result = parsed;
                return true;
            }

            return false;
        }

        public static string GetEnumAllowedValues<TEnum>() where TEnum : struct, Enum
        {
            return string.Join(", ", Enum.GetNames(typeof(TEnum)));
        }
    }
}

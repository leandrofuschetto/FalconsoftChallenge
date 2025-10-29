using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace RecruitingChallenge.API.Filters
{
    public class DateFormatValidationAttribute : ValidationAttribute
    {
        private readonly string _format;

        public DateFormatValidationAttribute(string format)
        {
            _format = format;
            ErrorMessage = $"Date must be in format {_format}";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is string str)
            {
                if (DateTime.TryParseExact(str, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}

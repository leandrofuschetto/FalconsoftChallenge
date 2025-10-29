using System.ComponentModel.DataAnnotations;

namespace OrderNowChallenge.API.Filters
{
    public class EnumValidationAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public EnumValidationAttribute(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("Type must be an enum.");

            _enumType = enumType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (Enum.IsDefined(_enumType, value))
                return ValidationResult.Success;

            if (value is string str && Enum.TryParse(_enumType, str, true, out _))
                return ValidationResult.Success;

            return new ValidationResult($"'{value}' is not a valid value for enum '{_enumType.Name}'.");
        }
    }
}

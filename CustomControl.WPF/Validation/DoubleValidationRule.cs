using System.Globalization;
using System.Windows.Controls;

namespace CustomControl.Validation
{
    public class DoubleValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (double.TryParse(value.ToString(), out double temp))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "Please input number.");
            }
        }
    }
}

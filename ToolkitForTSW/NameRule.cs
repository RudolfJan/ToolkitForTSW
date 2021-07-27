using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace ToolkitForTSW
  {
  /// <summary>
  /// Interaction logic for FormCloneScenario.xaml
  /// </summary>
  ///
  ///
  // https://stackoverflow.com/questions/58717392/validation-rule-for-checking-if-text-has-only-ascii
  public class NameRule : ValidationRule
  {
  public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
    return value is string str && str.All(ch => ch < 128)
      ? ValidationResult.ValidResult
      : new ValidationResult(false, "The name contains illegal characters");
    }
  }
  }

using System.Globalization;
using Avalonia.Data.Converters;

namespace ContactApp.Wpf.Convertors;

/// <summary>
/// Converts a phone number number into a readable string (+92-345-5776662) and vice-versa 
/// </summary>
public class PhoneNumberConvertor : IValueConverter {
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    if (value == null) return null;
    
    var phone = (string)value;
    return string.Concat(phone[..3], "-", phone[3..6], "-", phone[6..]);
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    return value == null
      ? string.Empty
      : (value as string)?.Replace("-", string.Empty);
  }
}
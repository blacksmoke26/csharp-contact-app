using System;
using System.ComponentModel;
using System.Globalization;
using Avalonia.Data.Converters;
using Dumpify;

namespace ContactApp.Wpf.Convertors;

public class SidebarItemTypeBooleanConvertor : IValueConverter {
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    ArgumentNullException.ThrowIfNull(value);
    ArgumentNullException.ThrowIfNull(parameter);

    if (!Enum.TryParse<SidebarItemType>((string)parameter, out var paramType)) {
      throw new InvalidEnumArgumentException(nameof(parameter));
    }
    
    var valueType = (SidebarItemType)value;
    return valueType == paramType;
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
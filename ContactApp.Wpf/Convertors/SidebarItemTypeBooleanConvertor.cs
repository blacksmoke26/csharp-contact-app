// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.ComponentModel;
using System.Globalization;
using Avalonia.Data.Converters;

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
// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.Globalization;
using Avalonia.Data.Converters;

namespace ContactApp.Wpf.Convertors;

/// <summary>
/// Return a placeholder string nullable string 
/// </summary>
public class NullableStringPlaceholderConvertor : IValueConverter {
  /// <inheritdoc/>
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    var placeholder = parameter ?? "-";
    
    if (value == null) return placeholder;
    
    return string.IsNullOrWhiteSpace((string)value) ? placeholder : value;
  }

  /// <inheritdoc/>
  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    throw new NotImplementedException();
  }
}
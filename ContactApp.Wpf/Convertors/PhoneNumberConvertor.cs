// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.Globalization;
using System.Text.RegularExpressions;
using Avalonia.Data.Converters;

namespace ContactApp.Wpf.Convertors;

/// <summary>
/// Converts a phone number number into a readable string (+92-345-5776662) and vice-versa 
/// </summary>
public class PhoneNumberConvertor : IValueConverter {
  /// <inheritdoc/>
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
    var placeholder = parameter ?? "-";

    return value == null
      ? placeholder
      : Contact.FormatPhoneNumber((string)value, PhoneNumberFormat.International);
  }

  /// <inheritdoc/>
  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
    return value == null
      ? null
      : Contact.FormatPhoneNumber((string)value, PhoneNumberFormat.E123);
  }
}
// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Avalonia.Data.Converters;

namespace ContactApp.Wpf.Convertors;

/// <summary>
/// Provides a set of useful <see cref="IValueConverter"/>s for working with integers.
/// </summary>
public class IntegerConverters {
  /// <summary>
  /// Checks that the given arguments are not null and are integers by type. 
  /// </summary>
  /// <returns>True when all passed, false otherwise</returns>
  private static bool AllInt(params object?[] a) => a.All(x => x is not null && int.TryParse($"{x}", out _));

  /// <summary>
  /// A value converter that returns true if the input is an integer
  /// </summary>
  public static readonly IValueConverter IsInt =
    new FuncValueConverter<object?, bool>(a => AllInt(a));

  /// <summary>
  /// A value converter that returns true if the input is not an integer
  /// </summary>
  public static readonly IValueConverter IsNotInt =
    new FuncValueConverter<object?, bool>(a => !AllInt(a));

  /// <summary>
  /// A value converter that returns true if the input object is a null reference.
  /// </summary>
  public static readonly IValueConverter IsNull =
    new FuncValueConverter<object?, bool>(x => x is null);

  /// <summary>
  /// A value converter that returns true if the input object is not null.
  /// </summary>
  public static readonly IValueConverter IsNotNull =
    new FuncValueConverter<object?, bool>(x => x is not null);

  /// <summary>
  /// A value converter that returns true if the input object is an integer and greater than to a parameter integer object.
  /// </summary>
  public static readonly IValueConverter IsGreaterThan =
    new FuncValueConverter<object?, object, bool>((a, b) => AllInt(a, b) && (int)a! > (int)b!);

  /// <summary>
  /// A value converter that returns true if the input object is an integer and lesser than to a parameter integer object.
  /// </summary>
  public static readonly IValueConverter IsLesserThan =
    new FuncValueConverter<object?, object, bool>((a, b) => AllInt(a, b) && (int)a! < (int)b!);

  /// <summary>
  /// A value converter that returns true if the input object is an integer and greater than and equal to a parameter integer object.
  /// </summary>
  public static readonly IValueConverter IsGreaterThanOrEqual =
    new FuncValueConverter<object?, object, bool>((a, b) => AllInt(a, b) && (int)a! >= (int)b!);

  /// <summary>
  /// A value converter that returns true if the input object is an integer and lesser than and equal to a parameter integer object.
  /// </summary>
  public static readonly IValueConverter IsLesserThanOrEqual =
    new FuncValueConverter<object?, object, bool>((a, b) => AllInt(a, b) && (int)a! <= (int)b!);

  /// <summary>
  /// A value converter that returns true if the input object is an integer and equal to a parameter integer object.
  /// </summary>
  public static readonly IValueConverter IsEqual =
    new FuncValueConverter<object?, object?, bool>((a, b) => AllInt(a, b) && (int)a! == (int)b!);

  /// <summary>
  /// A value converter that returns true if the input object is an integer and not equal to a parameter integer object.
  /// </summary>
  public static readonly IValueConverter IsNotEqual =
    new FuncValueConverter<object?, object?, bool>((a, b) => AllInt(a, b) && (int)a! != (int)b!);
}
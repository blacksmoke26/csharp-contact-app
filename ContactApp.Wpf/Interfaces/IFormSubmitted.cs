// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace ContactApp.Wpf.Interfaces;

/// <summary>
/// An interface to implement when a form with fields used within an overlay/model-dialog.
/// </summary>
/// <typeparam name="TFormData"></typeparam>
public interface IFormSubmitted<out TFormData> where TFormData : class {
  /**
   * Get the populated form inputs values
   */
  public TFormData? GetFormData();
  
  /**
   * Check whatever the form is submitted or not 
   */
  public bool IsFormSubmitted();

  /// <summary>
  /// Command: Triggers when the submit button is clicked
  /// </summary>
  public Task FormSubmit();

  /// <summary>
  /// Command: Triggers when the cancel button is clicked
  /// </summary>
  public Task FormDiscard();
}
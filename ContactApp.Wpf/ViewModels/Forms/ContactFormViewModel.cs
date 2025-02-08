// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ContactApp.Wpf.Interfaces;
using Irihi.Avalonia.Shared.Contracts;

namespace ContactApp.Wpf.ViewModels.Forms;

public partial class ContactFormViewModel : ObservableValidator, IDialogContext, IFormSubmitted<Contact> {
  public event EventHandler<object?>? RequestClose;

  /// <summary>
  /// Checks that form is submitted or not
  /// </summary>
  private bool _isFormSubmitted;

  public ObservableCollection<Department> Departments { get; init; } = [];

  [ObservableProperty] [Required] [StringLength(35, MinimumLength = 3)] [NotifyDataErrorInfo]
  private string? _firstName;

  [ObservableProperty] [Required] [StringLength(35, MinimumLength = 3)] [NotifyDataErrorInfo]
  private string? _lastName;

  [ObservableProperty] [Required] [NotifyDataErrorInfo]
  private Department? _department;

  [ObservableProperty] [NotifyDataErrorInfo] [DataType(DataType.Text)]
  private string? _company;

  [ObservableProperty] [DataType(DataType.Text)] [NotifyDataErrorInfo]
  private string? _phone;

  [EmailAddress] [ObservableProperty] [NotifyDataErrorInfo]
  private string? _email;

  [ObservableProperty] [StringLength(100)] [NotifyDataErrorInfo]
  private string? _address;

  [ObservableProperty] [StringLength(150)] [NotifyDataErrorInfo]
  private string? _notes;

  public ContactFormViewModel() {
    _ = FetchDepartments();
  }

  /// <summary>
  /// Close the current dialog
  /// </summary>
  public void Close() {
    RequestClose?.Invoke(this, null);
  }

  /// <summary>
  /// Fetches the departments for a source
  /// </summary>
  private async Task FetchDepartments() {
    var list = await Department.FetchPredefinedAsync();
    list.ForEach(Departments.Add);
  }

  /// <inheritdoc/>
  public Contact? GetFormData() {
    if (!_isFormSubmitted) return null;

    return new(
      FirstName!,
      LastName!,
      Department!
    ) {
      Company = Company,
      Phone = string.IsNullOrWhiteSpace(Phone) ? null : Regex.Replace(Phone, @"/[^\+\d]+/", string.Empty),
      Email = Email,
      Notes = Notes,
      Address = Address,
    };
  }

  /// <inheritdoc/>
  public bool IsFormSubmitted() {
    return _isFormSubmitted;
  }

  /// <inheritdoc/>
  [RelayCommand]
  public async Task FormSubmit() {
    await Task.Yield();

    ValidateAllProperties();
    if (HasErrors) return;

    _isFormSubmitted = true;

    Close(); // close dialog
  }

  /// <inheritdoc/>
  [RelayCommand]
  public Task FormDiscard() {
    Close(); // close dialog
    return Task.FromResult(0);
  }
}
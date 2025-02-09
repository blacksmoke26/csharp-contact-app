// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.ComponentModel.DataAnnotations;
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

  [ObservableProperty] private int? _id;

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

  [ObservableProperty] [EmailAddress] [NotifyDataErrorInfo]
  private string? _email;

  [ObservableProperty] private string? _address;

  [ObservableProperty] private string? _notes;

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
  /// Populate current fields by using the contact object
  /// </summary>
  /// <param name="contact">The contact object</param>
  public void PopulateFrom(Contact contact) {
    Id = contact.Id;
    FirstName = contact.FirstName;
    LastName = contact.LastName;
    Company = contact.Company;
    Phone = contact.GetFormattedPhoneNumber(PhoneNumberFormat.International);
    Email = contact.Email;
    Address = contact.Address;
    Notes = contact.Notes;
    Department = Departments.FirstOrDefault(x => x.Id.Equals(contact.Department.Id));
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
      Id = Id,
      Company = Company,
      Phone = Contact.FormatPhoneNumber(Phone, PhoneNumberFormat.E123),
      Email = Email,
      Notes = Notes,
      Address = Address,
    };
  }

  /// <inheritdoc/>
  public bool IsFormSubmitted() => _isFormSubmitted;

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
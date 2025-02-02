using System.ComponentModel.DataAnnotations;

namespace ContactApp.Wpf.ViewModels.Forms;

public partial class ContactFormViewModel : ObservableValidator {
  public ObservableCollection<Department> Departments { get; init; } = [];

  public ContactFormViewModel() {
    _ = FetchDepartments();
  }

  [ObservableProperty]
  [Required]
  [StringLength(35, MinimumLength = 3)]
  [NotifyDataErrorInfo]
  [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
  private string? _firstName;

  [ObservableProperty]
  [Required]
  [StringLength(35, MinimumLength = 3)]
  [NotifyDataErrorInfo]
  [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
  private string? _lastName;

  [ObservableProperty] [Required] [NotifyDataErrorInfo] [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
  private Department? _department;


  [ObservableProperty] [NotifyDataErrorInfo] [DataType(DataType.Text)] [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
  private string? _company;

  [ObservableProperty] [DataType(DataType.Text)] [NotifyDataErrorInfo] [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
  private string? _phone;


  [EmailAddress] [ObservableProperty] [NotifyDataErrorInfo] [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
  private string? _email;

  [ObservableProperty] [StringLength(100)] [NotifyDataErrorInfo] [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
  private string? _address;

  [ObservableProperty] [StringLength(150)] [NotifyDataErrorInfo] [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
  private string? _notes;

  /// <summary>
  /// Fetches the departments for a source
  /// </summary>
  private async Task FetchDepartments() {
    var list = await Department.FetchPredefinedAsync();
    list.ForEach(Departments.Add);
  }

  [RelayCommand]
  private async Task<Contact?> Save() {
    await Task.Yield();

    ValidateAllProperties();

    if (HasErrors) return null;

    Contact contact = new() {
      FirstName = FirstName!,
      LastName = LastName!,
      Department = Department?.Slug ?? string.Empty,

      Company = Company,
      Phone = Phone,
      Email = Email,
      Notes = Notes,
      Address = Address,
    };

    return contact;
  }
}
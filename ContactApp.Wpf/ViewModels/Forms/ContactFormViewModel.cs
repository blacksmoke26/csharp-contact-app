using CommunityToolkit.Mvvm.ComponentModel;
using ContactApp.Wpf.Models;

namespace ContactApp.Wpf.ViewModels.Forms;

public partial class ContactFormViewModel : ObservableValidator {
  [ObservableProperty] private string? _firstName;
  [ObservableProperty] private string? _lastName;
  [ObservableProperty] private Department? _department;
  [ObservableProperty] private string? _company;
  [ObservableProperty] private string? _phone;
  [ObservableProperty] private string? _email;
  [ObservableProperty] private string? _address;
  [ObservableProperty] private string? _notes;
}
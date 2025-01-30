using CommunityToolkit.Mvvm.ComponentModel;

namespace ContactApp.Wpf.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
  [ObservableProperty] private string _label = string.Empty;

  [ObservableProperty] private string? _sidebarSelected = "All";

  public MainWindowViewModel() {
  }
}
using Avalonia.Media;
using ContactApp.Wpf.ViewModels.Forms;
using Dumpify;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia.DialogHost;

namespace ContactApp.Wpf.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
  [ObservableProperty] private string _label = string.Empty;

  [ObservableProperty] private string? _sidebarSelected = "All";
  [ObservableProperty] private ObservableCollection<SidebarItem> _sidebarItems = [];

  public MainWindowViewModel() {
    InitializeSidebarItems();
  }

  /// <summary>
  /// Prepare sidebar list items
  /// </summary>
  private void InitializeSidebarItems() {
    var sidebarItems = SidebarItem.GetPredefinedList();

    Department.GetPredefinedList()
      .ForEach(x => sidebarItems.Add(SidebarItem.FromDepartment(x)));

    foreach (var sidebarItem in sidebarItems) {
      SidebarItems.Add(sidebarItem);
    }
  }

  /// <summary>
  /// Opens the new contact dialog
  /// </summary>
  [RelayCommand]
  private async Task OpenNewContactDialog() {
    var dialogService = Ioc.Default.GetRequiredService<IDialogService>();
    var dialogViewModel = dialogService.CreateViewModel<ContactFormViewModel>();

    Application.Current!.Resources.TryGetResource("DialogOverlayBackground", Application.Current.ActualThemeVariant,
      out var dialogOverlayBackground);

    var results = await dialogService
      .ShowDialogHostAsync(this, new DialogHostSettings(dialogViewModel) {
        DialogMargin = new Thickness(0),
        OverlayBackground = (SolidColorBrush)dialogOverlayBackground!
      }).ConfigureAwait(true);

    results?.Dump();
  }
}
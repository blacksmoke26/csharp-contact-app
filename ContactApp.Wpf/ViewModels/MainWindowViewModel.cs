using Avalonia.Media;
using ContactApp.Wpf.ViewModels.Forms;
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

  private void InitializeSidebarItems() {
    SidebarItems.Add(new SidebarItem {
      Id = "All",
      Label = "All",
      IconText = "\ue214",
      IconColor = SolidColorBrush.Parse("#5e5f60"),
    });
    SidebarItems.Add(new SidebarItem {
      Id = "Frequent",
      Label = "Frequent",
      IconText = "\ue398",
      IconColor = SolidColorBrush.Parse("#5e5f60"),
    });
    SidebarItems.Add(new SidebarItem {
      Id = "Starred",
      Label = "Starred",
      IconText = "\ue46a",
      IconColor = SolidColorBrush.Parse("#5e5f60"),
    });
    SidebarItems.Add(new SidebarItem {
      Label = "-",
      ItemType = SidebarItemType.Divider,
    });
    SidebarItems.Add(new SidebarItem {
      Label = "Categories",
      ItemType = SidebarItemType.Header,
    });
    SidebarItems.Add(new SidebarItem {
      Id = "Engineering",
      Label = "Engineering",
      IconText = "\ue25a",
      IconColor = SolidColorBrush.Parse("#5d87ff"),
    });
    SidebarItems.Add(new SidebarItem {
      Id = "Support",
      Label = "Support",
      IconText = "\ue25a",
      IconColor = SolidColorBrush.Parse("#fa896b"),
    });
    SidebarItems.Add(new SidebarItem {
      Id = "Sales",
      Label = "Sales",
      IconText = "\ue25a",
      IconColor = SolidColorBrush.Parse("#13deb9"),
    });
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

    await dialogService
      .ShowDialogHostAsync(this, new DialogHostSettings(dialogViewModel) {
        DialogMargin = new Thickness(0),
        OverlayBackground = (SolidColorBrush)dialogOverlayBackground!
      }).ConfigureAwait(true);
  }
}
using Avalonia.Media;
using ContactApp.Wpf.ViewModels.Forms;
using ContactApp.Wpf.Views;
using Dumpify;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia.DialogHost;

namespace ContactApp.Wpf.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
  [ObservableProperty] private string _label = string.Empty;

  [ObservableProperty] private string? _sidebarSelected = "All";
  [ObservableProperty] private Contact? _contactSelected;
  [ObservableProperty] private ObservableCollection<Contact> _contactItems = [];
  [ObservableProperty] private ObservableCollection<SidebarItem> _sidebarItems = [];

  [ObservableProperty] private ContentControl _detailsView = new NoContactView();

  public MainWindowViewModel() {
    _ = InitializeSidebarItems();
  }

  /// <summary>
  /// Prepare sidebar
  /// </summary>
  private async Task InitializeSidebarItems() {
    var sidebarItems = await SidebarItem.FetchPredefinedAsync();

    var departments = await Department.FetchPredefinedAsync();
    sidebarItems.AddRange(departments.Select(x => SidebarItem.FromDepartment(x)));

    sidebarItems.ForEach(SidebarItems.Add);
    // contacts
    var contacts = await Contact.FetchPredefinedAsync();
    contacts.ForEach(ContactItems.Add);
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

  public void ContactSelectionChange(object? sender, RoutedEventArgs e) {
    //(e.Source as ContactListingControl)!.Selected.Dump();
  }

  public void ContactStarClick(object? sender, RoutedEventArgs e) {
    //(e.Source as ContactListingControl)!.Selected.Dump(label: "Star Click");
  }

  public void ContactRemoveClick(object? sender, RoutedEventArgs e) {
    //(e.Source as ContactListingControl)!.Selected.Dump(label: "Remove Click");
  }
}
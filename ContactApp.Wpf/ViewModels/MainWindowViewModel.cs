using Avalonia.Media;
using ContactApp.Wpf.Controls;
using ContactApp.Wpf.ViewModels.Forms;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia.DialogHost;

namespace ContactApp.Wpf.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
  [ObservableProperty] private string _label = string.Empty;

  [ObservableProperty] private string? _sidebarSelected = "all";
  [ObservableProperty] private Contact? _contactSelected;
  [ObservableProperty] private ObservableCollection<Contact> _contactItems = [];
  [ObservableProperty] private ObservableCollection<SidebarItem> _sidebarItems = [];

  [ObservableProperty] private Control? _detailsView;

  public MainWindowViewModel() {
    DetailsView = Ioc.Default.GetRequiredService<ViewLocator>().CreateView<NoContactViewModel>();
    _ = InitializeSidebarItems();
  }

  partial void OnContactSelectedChanged(Contact? value) {
    var viewLocator = Ioc.Default.GetRequiredService<ViewLocator>();
    
    // No contact selected, sets the placeholder view
    if (value == null) {
      DetailsView = viewLocator.CreateView<NoContactViewModel>();
      return;
    }

    // Contact details
    DetailsView = viewLocator.CreateView<ContactDetailsViewModel>
      (vm => vm.Contact = value);
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

    _ = await dialogService
      .ShowDialogHostAsync(this, new DialogHostSettings(dialogViewModel) {
        DialogMargin = new Thickness(0),
        OverlayBackground = (SolidColorBrush)dialogOverlayBackground!
      }).ConfigureAwait(true);
  }

  /// <summary>
  /// Event: Triggered when sidebar item is clicked
  /// </summary>
  public void SidebarSelectionChange(object? _, RoutedEventArgs e) {
    SidebarSelected = (e.Source as SidebarControl)?.Selected;
  }

  /// <summary>
  /// Event: Triggered when contact selection is changed
  /// </summary>
  public void ContactSelectionChange(object? _, RoutedEventArgs e) {
    ContactSelected = (e.Source as ContactListingControl)?.Selected;
  }

  /// <summary>
  /// Event: Triggered when contact star button is clicked
  /// </summary>
  public void ContactStarClick(object? _, RoutedEventArgs e) {
    ContactSelected = (e.Source as ContactListingControl)?.Selected;
  }

  /// <summary>
  /// Event: Triggered when contact remove button is clicked
  /// </summary>
  public void ContactRemoveClick(object? _, RoutedEventArgs e) {
    ContactSelected = (e.Source as ContactListingControl)?.Selected;
  }
}
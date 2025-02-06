// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using ContactApp.Wpf.Controls;
using ContactApp.Wpf.ViewModels.Forms;
using ContactApp.Wpf.Views.Forms;
using Dumpify;
using Ursa.Controls;

namespace ContactApp.Wpf.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
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

    // fetch contacts and add them to existing list
    var contacts = await Contact.FetchPredefinedAsync();
    contacts.ForEach(ContactItems.Add);
  }

  [RelayCommand]
  private async Task ShowNewContactDialog() {
    var instance = Ioc.Default.GetRequiredService<ContactFormViewModel>();

    await OverlayDialog.ShowModal<ContactFormView, ContactFormViewModel>(instance,
      options: new OverlayDialogOptions() {
        Title = "New Contact",
        Buttons = DialogButton.None,
        HorizontalAnchor = HorizontalPosition.Center,
      }
    );

    if (instance.IsFormSubmitted()) {
      instance.GetFormData().Dump();
    }
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
    if (ContactSelected != null) {
      ContactSelected.IsStarred = !ContactSelected.IsStarred;
    }
  }

  /// <summary>
  /// Event: Triggered when contact remove button is clicked
  /// </summary>
  public void ContactRemoveClick(object? _, RoutedEventArgs e) {
    ContactSelected = (e.Source as ContactListingControl)?.Selected;
  }
}
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

  [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsContactSelected))]
  private Contact? _contactSelected;

  [ObservableProperty] private ObservableCollection<Contact> _contactItems = [];
  [ObservableProperty] private ObservableCollection<SidebarItem> _sidebarItems = [];

  public bool IsContactSelected {
    get => ContactSelected != null;
  }

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
    ContactSelected = GetContactFromEventArgs(e);
  }

  /// <summary>
  /// Event: Triggered when contact edit button is clicked
  /// </summary>
  public void ContactEditClick(object? _, RoutedEventArgs e) {
    ContactSelected = GetContactFromEventArgs(e);
    Console.WriteLine("Contact edit button clicked");
  }

  /// <summary>
  /// Event: Triggered when contact star button is clicked
  /// </summary>
  public void ContactStarClick(object? _, RoutedEventArgs e) {
    ContactSelected = GetContactFromEventArgs(e);
    if (ContactSelected != null) {
      ContactSelected.IsStarred = !ContactSelected.IsStarred;
    }
    Console.WriteLine("Contact start button clicked");
  }

  /// <summary>
  /// Event: Triggered when contact remove button is clicked
  /// </summary>
  public async Task ContactRemoveClick(object? _, RoutedEventArgs e) {
    ContactSelected = GetContactFromEventArgs(e);
    
    var result = await OverlayDialog.ShowModal(new TextBlock {
      Text = "Do you really want to delete this contact?",
      Margin = new Thickness(0, 5, 0, 10)
    }, null, null, new OverlayDialogOptions {
      Buttons = DialogButton.YesNo,
      Title = "Delete Contact",
      Mode = DialogMode.Warning
    });

    if (result == DialogResult.No) return;

    //TODO: Implement contact deletion logic here
  }

  /// <summary>
  /// Get contact object from routed event arguments
  /// </summary>
  /// <param name="e">Routed Event args</param>
  /// <returns>Whatever the contact instance or null </returns>
  private Contact? GetContactFromEventArgs(RoutedEventArgs e) {
    return e.Source switch {
      ContactListingControl control => control.Selected,
      ContactDetailViewControl model => model.ItemSource,
      _ => null
    };
  }
}
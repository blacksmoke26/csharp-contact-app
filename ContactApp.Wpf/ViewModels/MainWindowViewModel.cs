// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.ComponentModel;
using ContactApp.Wpf.Controls;
using ContactApp.Wpf.Filters;
using ContactApp.Wpf.ViewModels.Forms;
using ContactApp.Wpf.Views.Forms;
using Ursa.Controls;

namespace ContactApp.Wpf.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
  [ObservableProperty] private string _sidebarSelected = SidebarItem.ItemIdAll;

  [ObservableProperty] private string _searchQuery = string.Empty;

  [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsContactSelected))]
  private Contact? _contactSelected;

  private readonly string[] _filterProps = [nameof(SidebarSelected), nameof(SearchQuery)];

  private readonly ContactsFilter _recordsFilter = new() {
    CategoryId = SidebarItem.ItemIdAll,
  };

  [ObservableProperty] private ObservableCollection<Contact> _contactItems = [];
  [ObservableProperty] private ObservableCollection<SidebarItem> _sidebarItems = [];

  public bool IsContactSelected => ContactSelected != null;

  public MainWindowViewModel() {
    _ = InitializeData();
  }

  /// <summary>
  /// Prepare data for controls 
  /// </summary>
  private async Task InitializeData() {
    var sidebarItems = await SidebarItem.FetchPredefinedAsync();

    var departments = await Department.FetchPredefinedAsync();
    sidebarItems.AddRange(departments.Select(x => SidebarItem.FromDepartment(x)));

    sidebarItems.ForEach(SidebarItems.Add);

    // fetch contacts and add them to existing list
    var contacts = await Contact.FetchPredefinedAsync();
    _recordsFilter.Items.AddRange(contacts);

    await ApplyFiltersCommand.ExecuteAsync(null);
  }

  #region Records filteration process

  [RelayCommand]
  private async Task ApplyFilters() {
    _recordsFilter.CategoryId = SidebarSelected;
    _recordsFilter.Query = SearchQuery;

    var records = await _recordsFilter.FetchFiltered() as List<Contact>;

    ContactItems.Clear();

    records?.ForEach(ContactItems.Add);
    ContactSelected = ContactItems.First();
  }

  protected override void OnPropertyChanged(PropertyChangedEventArgs e) {
    base.OnPropertyChanged(e);
    // trigger filters changes
    if (_filterProps.Contains(e.PropertyName))
      ApplyFiltersCommand.ExecuteAsync(null);
  }

  #endregion

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
      ContactItems.Add(instance.GetFormData()!);
      ContactSelected = ContactItems.LastOrDefault();
      _recordsFilter.Items.Add(instance.GetFormData()!);
    }
  }

  /// <summary>
  /// Event: Triggered when sidebar item is clicked
  /// </summary>
  public void SidebarSelectionChange(object? _, RoutedEventArgs e) {
    SidebarSelected = (e.Source as SidebarControl)?.Selected!;
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
  public async Task ContactEditClick(object? _, RoutedEventArgs e) {
    ContactSelected = GetContactFromEventArgs(e)!;

    var instance = Ioc.Default.GetRequiredService<ContactFormViewModel>();
    instance.PopulateFrom(ContactSelected);

    await OverlayDialog.ShowModal<ContactFormView, ContactFormViewModel>(instance,
      options: new OverlayDialogOptions {
        Title = "Edit Contact",
        Buttons = DialogButton.None,
        HorizontalAnchor = HorizontalPosition.Center,
      }
    );

    if (instance.IsFormSubmitted())
      ContactSelected.CopyFrom(instance.GetFormData()!);
  }

  /// <summary>
  /// Event: Triggered when contact star button is clicked
  /// </summary>
  public void ContactStarClick(object? _, RoutedEventArgs e) {
    ContactSelected = GetContactFromEventArgs(e);
    if (ContactSelected != null) {
      ContactSelected.IsStarred = !ContactSelected.IsStarred;
    }
  }

  /// <summary>
  /// Event: Triggered when contact remove button is clicked
  /// </summary>
  public async Task ContactRemoveClick(object? _, RoutedEventArgs e) {
    ContactSelected = GetContactFromEventArgs(e);

    var result = await OverlayDialog.ShowModal(new TextBlock {
      Text = "Do you really want to remove this contact?",
      Margin = new Thickness(0, 5, 0, 10)
    }, null, null, new OverlayDialogOptions {
      Buttons = DialogButton.YesNo,
      Title = "Remove Contact",
      Mode = DialogMode.Warning
    });

    if (result == DialogResult.Yes) {
      var index = ContactItems.IndexOf(ContactSelected!);
      ContactItems.Remove(ContactSelected!);

      ContactSelected = index switch {
        -1 => ContactItems.FirstOrDefault(),
        0 => ContactItems.FirstOrDefault(),
        > 0 => ContactItems[index - 1],
        _ => null,
      };
    }
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
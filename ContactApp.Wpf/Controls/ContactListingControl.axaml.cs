// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using ContactApp.Wpf.Helpers;
using Ursa.Controls;

namespace ContactApp.Wpf.Controls;

public partial class ContactListingControl : ContentControl {
  #region Property: Selected

  private Contact? _selected;

  public static readonly DirectProperty<ContactListingControl, Contact?> SelectedProperty =
    AvaloniaProperty.RegisterDirect<ContactListingControl, Contact?>(
      nameof(Selected), o => o.Selected, (o, v) => o.Selected = v);

  public Contact? Selected {
    get => _selected;
    set {
      if (SetAndRaise(SelectedProperty, ref _selected, value))
        RaiseEvent(new RoutedEventArgs(SelectionChangeEvent, value));
    }
  }

  #endregion

  #region Property: ItemsSource

  private ObservableCollection<Contact> _itemsSource = [];

  public static readonly DirectProperty<ContactListingControl, ObservableCollection<Contact>> ItemsSourceProperty =
    AvaloniaProperty.RegisterDirect<ContactListingControl, ObservableCollection<Contact>>(
      nameof(ItemsSource), o => o.ItemsSource, (o, v) => o.ItemsSource = v);

  public ObservableCollection<Contact> ItemsSource {
    get => _itemsSource;
    set => SetAndRaise(ItemsSourceProperty, ref _itemsSource, value);
  }

  #endregion

  #region Event: Change

  public static readonly RoutedEvent<RoutedEventArgs> SelectionChangeEvent =
    RoutedEvent.Register<ContactListingControl, RoutedEventArgs>(nameof(SelectionChange), RoutingStrategies.Direct);

  /// <summary>
  /// Trigger the event when Selected value changes
  /// </summary>
  public event EventHandler<RoutedEventArgs> SelectionChange {
    add => AddHandler(SelectionChangeEvent, value);
    remove => RemoveHandler(SelectionChangeEvent, value);
  }

  #endregion

  public static readonly RoutedEvent<RoutedEventArgs> StarClickEvent =
    RoutedEvent.Register<ContactListingControl, RoutedEventArgs>(nameof(StarClick), RoutingStrategies.Direct);

  public event EventHandler<RoutedEventArgs> StarClick {
    add => AddHandler(StarClickEvent, value);
    remove => RemoveHandler(StarClickEvent, value);
  }

  public static readonly RoutedEvent<RoutedEventArgs> RemoveClickEvent =
    RoutedEvent.Register<ContactListingControl, RoutedEventArgs>(nameof(RemoveClick), RoutingStrategies.Direct);

  public event EventHandler<RoutedEventArgs> RemoveClick {
    add => AddHandler(RemoveClickEvent, value);
    remove => RemoveHandler(RemoveClickEvent, value);
  }


  public ContactListingControl() {
    InitializeComponent();
    _ = InitializeDesignMode();
  }

  private async Task InitializeDesignMode() {
    if (Design.IsDesignMode) {
      var contacts = await Contact.FetchPredefinedAsync();
      contacts.ForEach(ItemsSource.Add);
    }
  }

  [RelayCommand]
  private void StarButtonClick(Contact contact) {
    RaiseEvent(new RoutedEventArgs(StarClickEvent, contact));
  }

  [RelayCommand]
  private void RemoveButtonClick(Contact contact) {
    RaiseEvent(new RoutedEventArgs(RemoveClickEvent, contact));
  }

  // ReSharper disable once AsyncVoidMethod
  private async void AvatarElement_OnDataContextChanged(object? sender, EventArgs e) {
    var control = (Avatar)sender!;
    var contact = control.DataContext as Contact;
    control.Source = null;
    control.Content = AvatarHelper.RenderContent(contact?.FirstName, contact?.LastName);

    var result = await AvatarHelper.RenderSource(contact?.FirstName, contact?.LastName, contact?.ProfileImage);

    if (result.Source != null)
      control.Source = result.Source;
    else
      control.Content = result.Content;
  }

  private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) {
    Selected = (e.Source as ListBox)!.SelectedItem as Contact;
  }
}
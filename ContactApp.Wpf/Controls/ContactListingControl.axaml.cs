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
  private async void StyledElement_OnInitialized(object? sender, EventArgs e) {
    var obj = (Avatar)sender!;
    var contact = (Contact)obj.DataContext!;
    var avatarContent = string.Concat(contact.FirstName[0], contact.LastName[0]);

    if (contact.ProfileImage == null) {
      obj.Content = avatarContent;
      return;
    }

    var image = await ImageHelper.LoadFromUrlCacheAsync(contact.ProfileImage);

    if (image != null)
      obj.Source = image;
    else
      obj.Content = avatarContent;
  }

  private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e) {
    Selected = (e.Source as ListBox)!.SelectedItem as Contact;
  }
}
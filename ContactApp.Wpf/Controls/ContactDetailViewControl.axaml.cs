namespace ContactApp.Wpf.Controls;

public partial class ContactDetailViewControl : ContentControl {
  #region Property: ItemSource

  private Contact? _itemSource;

  public static readonly DirectProperty<ContactDetailViewControl, Contact?> ItemSourceProperty =
    AvaloniaProperty.RegisterDirect<ContactDetailViewControl, Contact?>(
      nameof(ItemSource), o => o.ItemSource, (o, v) => o.ItemSource = v);

  /// <summary>
  /// Contact to display the details 
  /// </summary>
  public Contact? ItemSource {
    get => _itemSource;
    set => SetAndRaise(ItemSourceProperty, ref _itemSource, value);
  }

  #endregion

  #region Event: RemoveClick

  public static readonly RoutedEvent<RoutedEventArgs> RemoveClickEvent =
    RoutedEvent.Register<ContactDetailViewControl, RoutedEventArgs>(nameof(RemoveClick), RoutingStrategies.Direct);

  /// <summary>
  /// An event fired when the contact remove button is clicked
  /// </summary>
  public event EventHandler<RoutedEventArgs> RemoveClick {
    add => AddHandler(RemoveClickEvent, value);
    remove => RemoveHandler(RemoveClickEvent, value);
  }

  #endregion

  #region Event: EditClick

  /// <summary>
  /// An event fired when the edit button is clicked
  /// </summary>
  public static readonly RoutedEvent<RoutedEventArgs> EditClickEvent =
    RoutedEvent.Register<ContactDetailViewControl, RoutedEventArgs>(nameof(EditClick), RoutingStrategies.Direct);

  /// <summary>
  /// An event fired when the contact edit button is clicked
  /// </summary>
  public event EventHandler<RoutedEventArgs> EditClick {
    add => AddHandler(EditClickEvent, value);
    remove => RemoveHandler(EditClickEvent, value);
  }

  #endregion

  #region Event: StarClick

  public static readonly RoutedEvent<RoutedEventArgs> StarClickEvent =
    RoutedEvent.Register<ContactDetailViewControl, RoutedEventArgs>(nameof(StarClick), RoutingStrategies.Direct);

  /// <summary>
  /// An event fired when the contact Star button is clicked
  /// </summary>
  public event EventHandler<RoutedEventArgs> StarClick {
    add => AddHandler(StarClickEvent, value);
    remove => RemoveHandler(StarClickEvent, value);
  }

  #endregion

  public ContactDetailViewControl() {
    InitializeComponent();
    _ = InitializeDesignMode();
  }

  private async Task InitializeDesignMode() {
    if (Design.IsDesignMode) {
      var contacts = await Contact.FetchPredefinedAsync();
      ItemSource = contacts.FirstOrDefault();
    }
  }

  /// <summary>
  /// Event: Triggered when contact edit button is clicked
  /// </summary>
  [RelayCommand]
  private void EditButton(Contact selected) {
    RaiseEvent(new RoutedEventArgs(EditClickEvent, selected));
  }

  /// <summary>
  /// Event: Triggered when contact star button is clicked
  /// </summary>
  [RelayCommand]
  private void StarButton(Contact selected) {
    RaiseEvent(new RoutedEventArgs(StarClickEvent, selected));
  }

  /// <summary>
  /// Event: Triggered when contact remove button is clicked
  /// </summary>
  [RelayCommand]
  private void RemoveButton(Contact selected) {
    RaiseEvent(new RoutedEventArgs(RemoveClickEvent, selected));
  }
}
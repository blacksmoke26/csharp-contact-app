using Avalonia.Data;

namespace ContactApp.Wpf.Controls;

/// <summary>
/// A custom sidebar navigation control 
/// </summary>
public partial class SidebarControl : ContentControl {
  #region Property: Selected

  private string? _selected = string.Empty;

  public static readonly DirectProperty<SidebarControl, string?> SelectedProperty =
    AvaloniaProperty.RegisterDirect<SidebarControl, string?>(
      nameof(Selected), o => o.Selected, (o, v) => o.Selected = v, defaultBindingMode: BindingMode.TwoWay);

  /// <summary>
  /// Item ID to be highlighted
  /// </summary>
  public string? Selected {
    get => _selected;
    set {
      if (!SetAndRaise(SelectedProperty, ref _selected, value)) return;

      foreach (var item in ItemsSource)
        // Reason: To reflect the two-way bindings and clear the current selection
        item.IsSelected = value == item.Id;

      // trigger event
      RaiseEvent(new RoutedEventArgs(SelectionChangeEvent));
    }
  }

  #endregion

  #region Property: Items

  private ObservableCollection<SidebarItem> _itemsSource = [];

  public static readonly DirectProperty<SidebarControl, ObservableCollection<SidebarItem>> ItemsSourceProperty =
    AvaloniaProperty.RegisterDirect<SidebarControl, ObservableCollection<SidebarItem>>(
      nameof(ItemsSource), o => o.ItemsSource, (o, v) => o.ItemsSource = v);

  /// <summary>
  /// Items List
  /// </summary>
  public ObservableCollection<SidebarItem> ItemsSource {
    get => _itemsSource;
    set => SetAndRaise(ItemsSourceProperty, ref _itemsSource, value);
  }

  #endregion

  public static readonly RoutedEvent<RoutedEventArgs> SelectionChangeEvent =
    RoutedEvent.Register<SidebarControl, RoutedEventArgs>(nameof(SelectionChange), RoutingStrategies.Direct);

  /// <summary>
  /// Trigger the event when selected value changes
  /// </summary>
  public event EventHandler<RoutedEventArgs> SelectionChange {
    add => AddHandler(SelectionChangeEvent, value);
    remove => RemoveHandler(SelectionChangeEvent, value);
  }

  public SidebarControl() {
    InitializeComponent();
  }

  [RelayCommand]
  private void ItemOnClick(SidebarItem? itemId) {
    Selected = itemId?.Id ?? string.Empty;
  }
}
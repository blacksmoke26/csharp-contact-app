using System;
using Avalonia.Data;

namespace ContactApp.Wpf.Controls;

/// <summary>
/// A custom sidebar navigation control 
/// </summary>
public partial class SidebarControl : ContentControl {
  #region Property: Selected

  private string _selected = string.Empty;

  public static readonly DirectProperty<SidebarControl, string> SelectedProperty =
    AvaloniaProperty.RegisterDirect<SidebarControl, string>(
      nameof(Selected), o => o.Selected, (o, v) => o.Selected = v, defaultBindingMode: BindingMode.TwoWay);

  /// <summary>
  /// Item ID to be highlighted
  /// </summary>
  public string Selected {
    get => _selected;
    set {
      if (SetAndRaise(SelectedProperty, ref _selected, value)) {
        foreach (var item in Items)
          // Reason: To reflect the two-way bindings and clear the current selection
          item.IsSelected = value == item.Id;
        RaiseEvent(new RoutedEventArgs(SelectedChangeEvent));
      }
    }
  }

  #endregion

  #region Property: Items

  private ObservableCollection<SidebarItem> _items = [];

  public static readonly DirectProperty<SidebarControl, ObservableCollection<SidebarItem>> ItemsProperty =
    AvaloniaProperty.RegisterDirect<SidebarControl, ObservableCollection<SidebarItem>>(
      nameof(Items), o => o.Items, (o, v) => o.Items = v);

  /// <summary>
  /// Items List
  /// </summary>
  public ObservableCollection<SidebarItem> Items {
    get => _items;
    set => SetAndRaise(ItemsProperty, ref _items, value);
  }

  #endregion

  public static readonly RoutedEvent<RoutedEventArgs> SelectedChangeEvent =
    RoutedEvent.Register<SidebarControl, RoutedEventArgs>(nameof(SelectedChange), RoutingStrategies.Bubble);

  /// <summary>
  /// Trigger the event when Selected value changes
  /// </summary>
  public event EventHandler<RoutedEventArgs> SelectedChange {
    add => AddHandler(SelectedChangeEvent, value);
    remove => RemoveHandler(SelectedChangeEvent, value);
  }

  public SidebarControl() {
    InitializeComponent();
  }

  [RelayCommand]
  private void ItemOnClick(SidebarItem itemId) {
    Selected = itemId.Id ?? string.Empty;
  }
}
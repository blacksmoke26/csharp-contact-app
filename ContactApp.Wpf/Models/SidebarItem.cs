using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ContactApp.Wpf.Models;

public enum SidebarItemType {
  Item,
  Divider,
  Header,
}

public partial class SidebarItem : ObservableObject {
  public string? Id { get; set; }
  public SidebarItemType ItemType { get; set; } = SidebarItemType.Item;
  public required string Label { get; set; }
  public string? IconText { get; set; }
  public IBrush? IconColor { get; set; }
  [ObservableProperty] private bool _isSelected;
}
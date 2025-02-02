using Avalonia.Media;

namespace ContactApp.Wpf.Models;

public enum SidebarItemType {
  Item,
  Divider,
  Header,
}

public partial class SidebarItem : ObservableObject {
  public string? Id { get; set; }
  public SidebarItemType ItemType { get; set; } = SidebarItemType.Item;

  /// <summary>
  /// The label
  /// </summary>
  public required string Label { get; set; }

  /// <summary>
  /// Icon symbol (Phosphor unicode) 
  /// </summary>
  public string? IconText { get; set; }

  public IBrush? IconColor { get; set; }

  /// <summary>
  /// Whatever state is selected or not
  /// </summary>
  [ObservableProperty] private bool _isSelected;

  /// <summary>
  /// Converts department instance into a sidebar item
  /// </summary>
  /// <param name="department">Department class instance</param>
  /// <param name="iconSymbol">Icon symbol (Phosphor unicode) </param>
  /// <returns>The sidebar item</returns>
  public static SidebarItem FromDepartment(Department department, string? iconSymbol = null) {
    return new() {
      Id = department.Slug,
      Label = department.Name,
      ItemType = SidebarItemType.Item,
      IconColor = SolidColorBrush.Parse(department.Color),
      IconText = iconSymbol ?? "\ue25a",
    };
  }

  /// <summary>
  /// Return the predefined sidebar-item 
  /// </summary>
  /// <returns>The Sidebar-item list</returns>
  public static List<SidebarItem> GetPredefinedList() {
    return [
      new() {
        Id = "all",
        Label = "All",
        IconText = "\ue214",
        IconColor = SolidColorBrush.Parse("#5e5f60"),
      },
      new() {
        Id = "frequent",
        Label = "Frequent",
        IconText = "\ue398",
        IconColor = SolidColorBrush.Parse("#5e5f60"),
      },
      new() {
        Id = "starred",
        Label = "Starred",
        IconText = "\ue46a",
        IconColor = SolidColorBrush.Parse("#5e5f60"),
      },
      new() {
        Label = "-",
        ItemType = SidebarItemType.Divider,
      },
      new() {
        Label = "Categories",
        ItemType = SidebarItemType.Header,
      },
    ];
  }
}
using Avalonia.Media;

namespace ContactApp.Wpf.Models;

public enum SidebarItemType {
  Item,
  Divider,
  Header,
}

public partial class SidebarItem : ObservableObject {
  /// <summary>
  /// Sidebar item id for all category
  /// </summary>
  public const string ItemIdAll = "_all";

  /// <summary>
  /// Sidebar item id for frequent category
  /// </summary>
  public const string ItemIdFrequent = "_frequent";

  /// <summary>
  /// Sidebar item id for starred category
  /// </summary>
  public const string ItemIdStarred = "_starred";

  /// <summary>
  /// Unique ID
  /// </summary>
  public string? Id { get; set; }
  
  /// <summary>
  /// Item Type
  /// </summary>
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
  /// Check that the given sidebar item id is predefined or not
  /// </summary>
  /// <param name="id">The item id</param>
  /// <returns>Returns true if matched, false otherwise</returns>
  public static bool IsPredefinedId(string id) {
    return id is ItemIdAll or ItemIdStarred or ItemIdFrequent;
  }

  /// <summary>
  /// Return the predefined sidebar-item 
  /// </summary>
  /// <returns>The Sidebar-item list</returns>
  public static Task<List<SidebarItem>> FetchPredefinedAsync() {
    var iconColor = SolidColorBrush.Parse("#B5B5B5");

    return Task.FromResult<List<SidebarItem>>([
      new() {
        Id = ItemIdAll,
        Label = "All",
        IconText = "\ue214",
        IconColor = iconColor,
      },
      new() {
        Id = ItemIdFrequent,
        Label = "Frequent",
        IconText = "\ue398",
        IconColor = iconColor,
      },
      new() {
        Id = ItemIdStarred,
        Label = "Starred",
        IconText = "\ue46a",
        IconColor = iconColor,
      },
      new() {
        Label = "-",
        ItemType = SidebarItemType.Divider,
      },
      new() {
        Label = "Categories",
        ItemType = SidebarItemType.Header,
      },
    ]);
  }
}
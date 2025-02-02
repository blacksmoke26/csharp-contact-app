namespace ContactApp.Wpf.Models;

public class Department {
  public const string DefaultColor = "#5e5f60";

  public int Id { get; set; }
  public required string Name { get; set; }
  public required string Slug { get; set; }
  public string Color { get; set; } = DefaultColor;

  /// <summary>
  /// Return the predefined departments 
  /// </summary>
  /// <returns>The department list</returns>
  public static List<Department> GetPredefinedList() {
    return [
      new() {
        Name = "Engineering",
        Slug = "engineering",
        Color = "#5d87ff",
      },
      new() {
        Name = "Support",
        Slug = "support",
        Color = "#fa896b",
      },
      new() {
        Name = "Sales",
        Slug = "sales",
        Color = "#13deb9",
      }
    ];
  }
}
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
  public static Task<List<Department>> FetchPredefinedAsync() {
    return Task.FromResult<List<Department>>([
      new() {
        Id = 1,
        Name = "Engineering",
        Slug = "engineering",
        Color = "#5d87ff",
      },
      new() {
        Id = 2,
        Name = "Support",
        Slug = "support",
        Color = "#fa896b",
      },
      new() {
        Id = 3,
        Name = "Sales",
        Slug = "sales",
        Color = "#13deb9",
      }
    ]);
  }
}
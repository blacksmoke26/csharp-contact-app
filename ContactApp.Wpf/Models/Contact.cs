namespace ContactApp.Wpf.Models;

public class Contact {
  public int? Id { get; set; }
  public required string Department { get; set; }

  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public string? Company { get; set; }
  public string? Phone { get; set; }
  public string? Email { get; set; }
  public string? Address { get; set; }
  public string? Notes { get; set; }
  public string? ProfileImage { get; set; }

  public bool IsStarred { get; set; }
}
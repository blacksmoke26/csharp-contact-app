namespace ContactApp.Wpf.Models;

public class Contact {
  public int Id { get; set; }
  public int DepartmentId { get; set; }

  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public string Company { get; set; } = string.Empty;
  public string Phone { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Address { get; set; } = string.Empty;
  public string Notes { get; set; } = string.Empty;
  public string ProfileImage { get; set; } = string.Empty;

  public bool IsStarred { get; set; }
}
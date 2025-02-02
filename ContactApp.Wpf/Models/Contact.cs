using Bogus;

namespace ContactApp.Wpf.Models;

public partial class Contact(string firstName, string lastName, Department department) : ObservableObject {
  [ObservableProperty] private int? _id;

  [ObservableProperty] private string _firstName = firstName;
  [ObservableProperty] private string _lastName = lastName;
  [ObservableProperty] private Department _department = department;
  
  [ObservableProperty] private string? _company;
  [ObservableProperty] private string? _phone;
  [ObservableProperty] private string? _email;
  [ObservableProperty] private string? _address;
  [ObservableProperty] private string? _notes;
  [ObservableProperty] private string? _profileImage;

  [ObservableProperty] private bool _isStarred;

  /// <summary>
  /// Return the predefined departments 
  /// </summary>
  /// <returns>The department list</returns>
  public static async Task<List<Contact>> FetchPredefinedAsync() {
    var contactId = 1;

    var departments = await Department.FetchPredefinedAsync();
    var users = new Faker<Contact>()
      .CustomInstantiator(f => new Contact(f.Name.FirstName(), f.Name.LastName(), f.PickRandom(departments)))
      .RuleFor(c => c.Id, () => contactId++)
      .RuleFor(c => c.Company, f => f.Company.CompanyName())
      .RuleFor(c => c.Email, f => f.Internet.Email())
      .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("+############"))
      .RuleFor(c => c.Address, f => f.Address.FullAddress())
      .RuleFor(c => c.Notes, f => f.Lorem.Sentence(35))
      //.RuleFor(c => c.ProfileImage, f => f.Image.PicsumUrl())
      .RuleFor(c => c.ProfileImage, f => null)
      .RuleFor(c => c.IsStarred, f => f.PickRandomParam([true, false]));

    return users.Generate(10).ToList();
  }
}
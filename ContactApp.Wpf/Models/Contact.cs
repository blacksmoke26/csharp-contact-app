using System.Text.RegularExpressions;
using Bogus;

namespace ContactApp.Wpf.Models;

public enum PhoneNumberFormat {
  /// <summary>
  /// Output as "+034042239532'
  /// </summary>
  E123 = 0,
  
  /// <summary>
  /// Output as "(+03) 404 2239532'
  /// </summary>
  International = 1,
}

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

  [ObservableProperty] private DateTime _createdAt = DateTime.Now;
  [ObservableProperty] private DateTime _updatedAt = DateTime.Now;

  /// <summary>
  /// Formats a given phone number<br/>
  /// Reference: <see href="https://en.wikipedia.org/wiki/National_conventions_for_writing_telephone_numbers"/>
  /// </summary>
  /// <param name="phone">The phone number</param>
  /// <param name="format">The format</param>
  /// <returns>Returns the formatted phone number</returns>
  public static string? FormatPhoneNumber(string? phone, PhoneNumberFormat format) {
    if ( phone == null) return null;
    
    return format switch {
      PhoneNumberFormat.International => string.Concat("(", phone[..3], ") ", phone[3..6], " ", phone[6..]),
      _ => Regex.Replace(phone, @"[^\+\d]+", String.Empty),
    };
  }

  /// <summary>
  /// Formats a phone number<br/>
  /// Reference: <see href="https://en.wikipedia.org/wiki/National_conventions_for_writing_telephone_numbers"/>
  /// </summary>
  /// <param name="format">The format</param>
  /// <returns>Returns the formatted phone number</returns>
  public string? GetFormattedPhoneNumber(PhoneNumberFormat format) {
    return FormatPhoneNumber(Phone, format);
  }

  /// <summary>
  /// Copy the given object data into the current one
  /// </summary>
  /// <param name="contact">The contact object</param>
  public void CopyFrom(Contact contact) {
    FirstName = contact.FirstName;
    LastName = contact.LastName;
    Company = contact.Company;
    Phone = contact.GetFormattedPhoneNumber(PhoneNumberFormat.E123);
    Email = contact.Email;
    Address = contact.Address;
    Notes = contact.Notes;
    ProfileImage = contact.ProfileImage;
    UpdatedAt = DateTime.Now;
    IsStarred = contact.IsStarred;
    Id = contact.Id;
    Department = contact.Department;
  }

  /// <summary>
  /// Return the predefined departments 
  /// </summary>
  /// <param name="count">The number of records to fetch</param>
  /// <returns>The department list</returns>
  public static async Task<List<Contact>> FetchPredefinedAsync(int count = 20) {
    var contactId = 1;

    var departments = await Department.FetchPredefinedAsync();
    var users = new Faker<Contact>()
      .CustomInstantiator(f => new Contact(f.Name.FirstName(), f.Name.LastName(), f.PickRandom(departments)))
      .RuleFor(c => c.Id, () => contactId++)
      .RuleFor(c => c.CreatedAt, f => f.Date.Past())
      .RuleFor(c => c.UpdatedAt, f => f.Date.Soon())
      .RuleFor(c => c.Company, f => f.Company.CompanyName())
      .RuleFor(c => c.Email, f => f.Internet.Email())
      .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("+############"))
      .RuleFor(c => c.Address, f => f.Address.FullAddress())
      .RuleFor(c => c.Notes, f => f.Lorem.Sentence(35))
      .RuleFor(c => c.ProfileImage, f => f.PickRandom(null, f.Image.LoremFlickrUrl(250, 250)))
      .RuleFor(c => c.IsStarred, f => f.PickRandomParam([true, false]));

    return users.Generate(count).ToList();
  }
}
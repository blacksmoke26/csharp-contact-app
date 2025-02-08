// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using ContactApp.Wpf.Interfaces;

namespace ContactApp.Wpf.Filters;

public class ContactsFilter : IListFilter {
  /// <summary>
  /// Filter: The category id
  /// </summary>
  public required string CategoryId { get; set; }
  
  /// <summary>
  /// Filter: The query string to search in contact
  /// </summary>
  public string Query { get; set; } = string.Empty;
  
  /// <summary>
  /// Contacts list to search within
  /// </summary>
  public List<object> Items { get; } = [];

  /// <inheritdoc />
  public async Task<object> FetchFiltered() {
    await Task.Delay(100);

    var query = Items.Cast<Contact>()
      .Where(FilterSearch)
      .Where(FilterCategory);

    if (CategoryId == SidebarItem.ItemIdFrequent) {
      query = query.OrderBy(x => x.UpdatedAt);
    }

    return query.ToList();
  }

  private bool FilterSearch(Contact contact) {
    if (string.IsNullOrEmpty(Query)) return true;

    return
      contact.FirstName.Contains(Query, StringComparison.InvariantCulture)
      || contact.LastName.Contains(Query, StringComparison.InvariantCulture);
  }

  private bool FilterCategory(Contact contact) {
    return CategoryId switch {
      SidebarItem.ItemIdAll => true,
      SidebarItem.ItemIdStarred => contact.IsStarred,
      SidebarItem.ItemIdFrequent => true,
      _ => contact.Department.Slug.Equals(CategoryId, StringComparison.InvariantCulture),
    };
  }
}
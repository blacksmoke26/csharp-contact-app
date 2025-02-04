using ContactApp.Wpf.ViewModels;

namespace ContactApp.Wpf.Views;

public partial class ContactDetailsView : ContentControl {
  public ContactDetailsView() {
    InitializeComponent();
    _ = InitializeDesignMode();
  }

  /// <summary>
  /// Initializes and populate the current instance for design mode only 
  /// </summary>
  private async Task InitializeDesignMode() {
    if (Design.IsDesignMode) {
      var contacts = await Contact.FetchPredefinedAsync();
      DataContext = new ContactDetailsViewModel() {
        Contact = contacts.FirstOrDefault()
      };
    }
  }
}
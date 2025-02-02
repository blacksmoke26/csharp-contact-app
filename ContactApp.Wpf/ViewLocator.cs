using ContactApp.Wpf.ViewModels;
using ContactApp.Wpf.ViewModels.Forms;
using ContactApp.Wpf.Views;
using ContactApp.Wpf.Views.Forms;
using HanumanInstitute.MvvmDialogs.Avalonia;

namespace ContactApp.Wpf;

public class ViewLocator : ViewLocatorBase {
  /// <summary>
  /// Lookup ViewModel~View Dictionary for view instances 
  /// </summary>
  private readonly Dictionary<Type, Func<Control>> _locator = new();
  
  /// <inheritdoc />
  protected override string GetViewName(object viewModel) => viewModel.GetType().FullName!.Replace("ViewModel", "View");

  /// <inheritdoc />
  public ViewLocator() {
    RegisterViewFactory<MainWindowViewModel, MainWindow>();
    RegisterViewFactory<ContactFormViewModel, ContactFormView>();
    RegisterViewFactory<ContactDetailsViewModel, ContactDetailsView>();
    RegisterViewFactory<NoContactViewModel, NoContactView>();
  }

  /// <summary>
  /// Creates a view instance against the view model 
  /// </summary>
  /// <param name="data">View Model instance</param>
  /// <returns>View instance</returns>
  public override Control Build(object? data) {
    if (data is null || !_locator.TryGetValue(data.GetType(), out var factory))
      return new TextBlock { Text = "ViewModel Not Found:" };

    return factory.Invoke();
  }
  
  /// <summary>
  /// Checks whatever the given object is a ViewModel instance
  /// </summary>
  /// <param name="data">The object</param>
  /// <returns>True when matched, false otherwise</returns>
  public override bool Match(object? data) {
    return data is ViewModelBase or ObservableObject;
  }

  /// <summary>
  /// Adds the ViewModel / View into the <see cref="_locator"/> dictionary
  /// </summary>
  private void RegisterViewFactory<TViewModel, TView>()
    where TViewModel : class
    where TView : Control {
    _locator.Add(typeof(TViewModel), Activator.CreateInstance<TView>);
  }
  
  /// <inheritdoc cref="Build"/>
  public override object Create(object viewModel) => Build(viewModel);
}